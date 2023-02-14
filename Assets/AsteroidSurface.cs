using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSurface : MonoBehaviour
{
    public DestroyableRock[] Rocks;

    private void Awake()
    {

        for (int i = 0; i < Rocks.Length; i++)
        {
            Rocks[i].gameObject.SetActive(false);
        }

        int amountOfRocks = Rocks.Length;

        int toSpawn = 0;

        if (amountOfRocks == 7)
        {
            switch (AsteroidLauncher.SpawnablesAmount)
            {
                case AsteroidLauncher.MineralDensity.None:
                    break;
                case AsteroidLauncher.MineralDensity.Scarce:
                    toSpawn = 1;
                    break;
                case AsteroidLauncher.MineralDensity.Medium:
                    toSpawn = 3;
                    break;
                case AsteroidLauncher.MineralDensity.High:
                    toSpawn = 5;
                    break;
                case AsteroidLauncher.MineralDensity.Highest:
                    toSpawn = 7;
                    break;
                default:
                    break;
            }
        }

        else if (amountOfRocks == 14)
        {
            switch (AsteroidLauncher.SpawnablesAmount)
            {
                case AsteroidLauncher.MineralDensity.None:
                    break;
                case AsteroidLauncher.MineralDensity.Scarce:
                    toSpawn = 3;
                    break;
                case AsteroidLauncher.MineralDensity.Medium:
                    toSpawn = 8;
                    break;
                case AsteroidLauncher.MineralDensity.High:
                    toSpawn = 10;
                    break;
                case AsteroidLauncher.MineralDensity.Highest:
                    toSpawn = 14;
                    break;
                default:
                    break;
            }
        }

        int spawned = 0;

        while (true)
        {

            for (int i = 0; i < Rocks.Length; i++) 
            {
                int rand = Random.Range(0, Rocks.Length / 2);

                if (rand == 0
                    && spawned < toSpawn
                    && !Rocks[i].gameObject.activeSelf)
                {
                    Rocks[i].gameObject.SetActive(true);
                    spawned++;
                }
            }

            if (spawned >= toSpawn)
            {
                break;
            }

        }

        Debug.Log("Amount of rocks to spawn " + toSpawn);

    }
}
