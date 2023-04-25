using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AsteroidLauncher : MonoBehaviour
{
    //public enum AsteroidType
    //{
    //    None = 0,        
    //    Random = 1,
    //    Asteroid01a = 2,
    //    Asteroid01b = 3,
    //    Asteroid01c = 4,
    //    Asteroid01d = 5,
    //    Asteroid01e = 6,
    //    Asteroid01f = 7,
    //    Asteroid01g = 8,
    //    Asteroid01b_big = 9,
    //    Asteroid01e_big = 10,
    //    Asteroid01f_big = 11,
    //    Asteroid01g_big = 12,
    //    Asteroid01c_big = 13,
    //    RandomShuffledList = 14

    //}

    //public enum MineralDensity
    //{
    //    None = 0,
    //    Scarce = 1,
    //    Medium = 2,
    //    High = 3,
    //    Highest = 4
    //}

    //public static AsteroidType CurrentAsteroid;
    //public static MineralDensity SpawnablesAmount;
    //public static ResourceInventory.ResourceType ResourceType;


    //public static int[] Order = new int[12];
    //public static bool HasBeenShuffled;
    //public static int RunningIndex;

    //public void Awake()
    //{
    //    Setup(true);
    //}

    //public static void Setup(bool launchScene)
    //{
    //    LaunchAsteroidScene(AsteroidType.RandomShuffledList,
    //                        ResourceInventory.ResourceType.Diamond,
    //                        MineralDensity.Highest,
    //                        launchScene);

    //    //AsteroidLauncher.LaunchAsteroidScene(AsteroidType.RandomShuffledList,
    //    //                                     ResourceInventory.ResourceType.Diamond,
    //    //                                     MineralDensity.Highest,
    //    //                                     launchScene);
    //}


    //public static void LaunchAsteroidScene(AsteroidType asteroidType,
    //                                       ResourceInventory.ResourceType resourceType,
    //                                       MineralDensity spawnAmount,
    //                                       bool launchScene)
    //{
    //    CurrentAsteroid = asteroidType;
    //    SpawnablesAmount = spawnAmount;
    //    ResourceType = resourceType;

    //    if (!launchScene)
    //    {
    //        return;
    //    }

    //    switch (CurrentAsteroid)
    //    {
    //        case AsteroidType.None:
    //            Debug.LogError("Use a valid asteroid type! Asteroid type can not be NONE");
    //            break;

    //        case AsteroidType.Random:
    //            LaunchRandomAsteroid(resourceType, spawnAmount);
    //            break;

    //        case AsteroidType.Asteroid01a:
    //            SceneManager.LoadScene("Asteroid1", LoadSceneMode.Additive);
    //            break;

    //        case AsteroidType.Asteroid01b:
    //            SceneManager.LoadScene("Asteroid2");
    //            break;

    //        case AsteroidType.Asteroid01c:
    //            SceneManager.LoadScene("Asteroid3");
    //            break;

    //        case AsteroidType.Asteroid01d:
    //            SceneManager.LoadScene("Asteroid4");
    //            break;

    //        case AsteroidType.Asteroid01e:
    //            SceneManager.LoadScene("Asteroid5");
    //            break;

    //        case AsteroidType.Asteroid01f:
    //            SceneManager.LoadScene("Asteroid6");
    //            break;

    //        case AsteroidType.Asteroid01g:
    //            SceneManager.LoadScene("Asteroid7");
    //            break;

    //        case AsteroidType.Asteroid01b_big:
    //            SceneManager.LoadScene("Asteroid8");
    //            break;

    //        case AsteroidType.Asteroid01e_big:
    //            SceneManager.LoadScene("Asteroid9");
    //            break;

    //        case AsteroidType.Asteroid01f_big:
    //            SceneManager.LoadScene("Asteroid10");
    //            break;

    //        case AsteroidType.Asteroid01g_big:
    //            SceneManager.LoadScene("Asteroid11");
    //            break;

    //        case AsteroidType.Asteroid01c_big:
    //            SceneManager.LoadScene("Asteroid12");
    //            break;

    //        case AsteroidType.RandomShuffledList:
    //            LaunchRandomShuffledAsteroid(spawnAmount, resourceType);
    //            break;

    //        default:
    //            break;
    //    }

    //    Debug.Log("CurrentAsterois is " + CurrentAsteroid);
    //}

    //// Launches asteroid from a readily shuffled list, so that no asteroid
    //// can come back to back and all asteroids get their change to shine
    //public static void LaunchRandomShuffledAsteroid(MineralDensity spawnAmount,
    //                                                ResourceInventory.ResourceType resourceType)
    //{
    //    if (!HasBeenShuffled)
    //    {
    //        ShuffleAsteroids();
    //        RunningIndex = 0;
    //    }

    //    AsteroidType rando = AsteroidType.None;

    //    // Choose index

    //    int sceneNumber = Order[RunningIndex];

    //    rando = ChooseWithNumber(rando, sceneNumber);

    //    LaunchAsteroidScene(rando, resourceType, spawnAmount, true);

    //    if (RunningIndex + 1 >= Order.Length)
    //    {
    //        ShuffleAsteroids();
    //        RunningIndex = 0;
    //    }

    //    else
    //    {
    //        RunningIndex++;
    //    }



    //}

    //public static void ShuffleAsteroids()
    //{
    //    int excludeFirst = -1;


    //    if (!HasBeenShuffled)
    //    {
    //        excludeFirst = Random.Range(0, Order.Length);
    //    }

    //    else
    //    {
    //        excludeFirst = Order[Order.Length - 1];
    //    }

    //    for (int i = 0; i < Order.Length; i++)
    //    {
    //        Order[i] = -1;
    //    }

    //    for (int i = 0; i < Order.Length; i++)
    //    {
    //        while (true)
    //        {
    //            bool foundAUnique = false;

    //            int toTry = Random.Range(0, Order.Length);

    //            if (i == 0
    //                && toTry != excludeFirst)
    //            {
    //                foundAUnique = true;
    //            }

    //            else if(i != 0)
    //            {
    //                bool contains = false;

    //                for (int j = 0; j < Order.Length; j++)
    //                {
    //                    if (Order[j] == toTry)
    //                    {
    //                        contains = true;
    //                        break;
    //                    }
    //                }

    //                if (!contains)
    //                {
    //                    foundAUnique = true;
    //                }
    //            }

    //            if (foundAUnique)
    //            {
    //                Order[i] = toTry;
    //                break;
    //            }
    //        }

    //    }

    //    for (int i = 0; i < Order.Length; i++)
    //    {
    //        Debug.Log("Scene at index " + i + " is " + Order[i].ToString());
    //    }

    //    HasBeenShuffled = true;
    //    RunningIndex = 0;
    //}

    ////Launches a simple random asteroid.
    //public static void LaunchRandomAsteroid(ResourceInventory.ResourceType resourceType, 
    //                                        MineralDensity spawnAmount)
    //{
    //    AsteroidType rando = AsteroidType.None;

    //    int rand = Random.Range(0, 12);

    //    rando = ChooseWithNumber(rando, rand);

    //    LaunchAsteroidScene(rando, resourceType, spawnAmount, true);
    //}

    //private static AsteroidType ChooseWithNumber(AsteroidType rando, int rand)
    //{
    //    switch (rand)
    //    {
    //        case 0:
    //            rando = AsteroidType.Asteroid01a;
    //            break;

    //        case 1:
    //            rando = AsteroidType.Asteroid01b;
    //            break;

    //        case 2:
    //            rando = AsteroidType.Asteroid01c;
    //            break;

    //        case 3:
    //            rando = AsteroidType.Asteroid01d;
    //            break;

    //        case 4:
    //            rando = AsteroidType.Asteroid01e;
    //            break;

    //        case 5:
    //            rando = AsteroidType.Asteroid01f;
    //            break;

    //        case 6:
    //            rando = AsteroidType.Asteroid01g;
    //            break;

    //        case 7:
    //            rando = AsteroidType.Asteroid01b_big;
    //            break;

    //        case 8:
    //            rando = AsteroidType.Asteroid01c_big;
    //            break;

    //        case 9:
    //            rando = AsteroidType.Asteroid01e_big;
    //            break;

    //        case 10:
    //            rando = AsteroidType.Asteroid01f_big;
    //            break;

    //        case 11:
    //            rando = AsteroidType.Asteroid01g_big;
    //            break;

    //    }

    //    return rando;
    //}
}
