using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSurface : MonoBehaviour
{
    //public DestroyableRock[] Rocks;

    //private void Awake()
    //{

    //    if (AsteroidLauncher.ResourceType == ResourceInventory.ResourceType.None)
    //    {
    //        AsteroidLauncher.Setup(false);
    //        //Debug.Log("Had to setup asteroid launcher");
    //    }

    //    for (int i = 0; i < Rocks.Length; i++)
    //    {
    //        Rocks[i].gameObject.SetActive(false);
    //    }

    //    int amountOfRocks = Rocks.Length;

    //    int toSpawn = 0;

    //    if (amountOfRocks == 7)
    //    {
    //        switch (AsteroidLauncher.SpawnablesAmount)
    //        {
    //            case AsteroidLauncher.MineralDensity.None:
    //                break;
    //            case AsteroidLauncher.MineralDensity.Scarce:
    //                //toSpawn = 1;
    //                toSpawn = Random.Range(1, 3);
    //                break;
    //            case AsteroidLauncher.MineralDensity.Medium:
    //                //toSpawn = 3;
    //                toSpawn = Random.Range(3, 5);
    //                break;
    //            case AsteroidLauncher.MineralDensity.High:
    //                //toSpawn = 5;
    //                toSpawn = Random.Range(5, 7);
    //                break;
    //            case AsteroidLauncher.MineralDensity.Highest:
    //                toSpawn = 7;
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    else if (amountOfRocks == 14)
    //    {
    //        switch (AsteroidLauncher.SpawnablesAmount)
    //        {
    //            case AsteroidLauncher.MineralDensity.None:
    //                break;
    //            case AsteroidLauncher.MineralDensity.Scarce:
    //                //toSpawn = 3;
    //                toSpawn = Random.Range(2, 6);
    //                break;
    //            case AsteroidLauncher.MineralDensity.Medium:
    //                //toSpawn = 8;
    //                toSpawn = Random.Range(6, 9);
    //                break;
    //            case AsteroidLauncher.MineralDensity.High:
    //                //toSpawn = 10;
    //                toSpawn = Random.Range(9, 13);
    //                break;
    //            case AsteroidLauncher.MineralDensity.Highest:
    //                //toSpawn = 14;
    //                toSpawn = Random.Range(12, 15);
    //                break;
    //            default:
    //                break;
    //        }
    //    }

    //    int spawned = 0;

    //    while (true)
    //    {

    //        for (int i = 0; i < Rocks.Length; i++) 
    //        {
    //            int rand = Random.Range(0, Rocks.Length / 2);

    //            if (rand == 0
    //                && spawned < toSpawn
    //                && !Rocks[i].gameObject.activeSelf)
    //            {
    //                Rocks[i].gameObject.SetActive(true);
    //                spawned++;
    //            }
    //        }

    //        if (spawned >= toSpawn)
    //        {
    //            break;
    //        }

    //    }

    //    //Debug.Log("Amount of rocks to spawn " + toSpawn);

    //}

    //public void CompareCollider(Collider collider,
    //                            out ResourceInventory.ResourceType resourceType,
    //                            out int amount)
    //{
    //    resourceType = ResourceInventory.ResourceType.None;
    //    amount = 0;

    //    if (collider == null)
    //    {
    //        resourceType = ResourceInventory.ResourceType.None;
    //        amount = 0;
    //        //Debug.Log("Null collider");
    //        return;
    //    }

    //    for (int i = 0; i < Rocks.Length; i++)
    //    {
    //        if (collider.gameObject != null && collider.gameObject
    //            == Rocks[i].gameObject)
    //        {
    //            Debug.Log("Found a matching collider " + Rocks[i].gameObject.name + " resource type is " + Rocks[i].ResourceType + " amount is " + Rocks[i].ResourceAmount);
    //            //resourceType = Rocks[i].ResourceType;
    //            amount = Rocks[i].ResourceAmount;
    //            break;
    //        }
    //    }
    //}
}
