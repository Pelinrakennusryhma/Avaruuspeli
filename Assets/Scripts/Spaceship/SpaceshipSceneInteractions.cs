using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipSceneInteractions : MonoBehaviour
{
    [SerializeField]
    EnemySpaceshipSpawner enemySpaceshipSpawner;
    [SerializeField]
    AsteroidSpawner asteroidSpawner;
    [SerializeField]
    ActorManager actorManager;
    [SerializeField]
    MothershipHangar mothershipHangar;

    // Just so the scene can be ran directly without coming from worldmap
    [SerializeField]
    AsteroidPOISceneData fallbackSceneData;

    private void Start()
    {

        AsteroidPOISceneData data = null;

        if (GameManager.Instance != null && GameManager.Instance.currentPOI != null)
        {
            data = (AsteroidPOISceneData)GameManager.Instance.currentPOI.Data;
            //Debug.Log("data: " + data);
        }

        if (data == null)
        {
            data = fallbackSceneData;
        }

        asteroidSpawner.SpawnNonMineableAsteroids(data.NumAsteroids);
        asteroidSpawner.SpawnMineableAsteroids(data.NumMineables, data.MineableResources);
        enemySpaceshipSpawner.SpawnEnemies(data.NumEnemies);
    }
}
