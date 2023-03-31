using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpaceshipSpawner : MonoBehaviour
{
    [SerializeField]
    BoxCollider enemySpawnArea;
    [SerializeField]
    GameObject enemyPrefab;
    [SerializeField]
    GameObject shipPrefab;
    public void SpawnEnemies(int amount)
    {
        //Get amount from POISceneData

        for (int i = 0; i < amount; i++)
        {
            Vector3 pos = GetPositionInSpawnArea(enemySpawnArea.bounds);
            GameObject enemy = Instantiate(enemyPrefab, pos, Quaternion.identity, enemySpawnArea.transform);
            GameObject ship = Instantiate(shipPrefab, pos, Random.rotation, enemy.transform);

            EnemyControls enemyControls = enemy.GetComponent<EnemyControls>();
            enemyControls.enabled = true;
            SpaceshipBT spaceshipBT = enemy.GetComponent<SpaceshipBT>();
            spaceshipBT.enabled = true;

        }
    }

    Vector3 GetPositionInSpawnArea(Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
            );
    }
}
