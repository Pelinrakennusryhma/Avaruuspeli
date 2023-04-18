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

    private void Awake()
    {
        GameEvents.Instance.EventLeavingSceneStarted.AddListener(OnEventLeavingSceneStarted);
        GameEvents.Instance.EventLeavingSceneCancelled.AddListener(OnEventLeavingSceneCancelled);

        AsteroidPOISceneData data = null;

        Debug.Log("manager: " + GameManager.Instance);
        Debug.Log("currentPOI: " + GameManager.Instance.currentPOI);
        if (GameManager.Instance != null && GameManager.Instance.currentPOI != null)
        {
            data = (AsteroidPOISceneData)GameManager.Instance.currentPOI.Data;
            Debug.Log("data: " + data);
        }

        if (data == null)
        {
            data = fallbackSceneData;
        }

        asteroidSpawner.SpawnNonMineableAsteroids(data.NumAsteroids);
        asteroidSpawner.SpawnMineableAsteroids(data.NumMineables, data.MineableResources);
        enemySpaceshipSpawner.SpawnEnemies(data.NumEnemies);
    }

    void OnEventLeavingSceneStarted()
    {
        if (actorManager.SceneCleared || mothershipHangar.PlayerShipInHangar)
        {
            StartCoroutine(LeaveScene(Globals.Instance.leaveSpaceshipSceneDelay));
        }
    }

    void OnEventLeavingSceneCancelled()
    {
        StopAllCoroutines();
    }

    IEnumerator LeaveScene(float delay) 
    { 
        yield return new WaitForSeconds(delay);
        GameManager.Instance.GoBackToWorldMap();
    }
}
