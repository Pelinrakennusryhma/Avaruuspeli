using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POISpawner : MonoBehaviour
{
    [SerializeField]
    GameObject asteroidPOIPrefab;
    [SerializeField]
    StarSystemOnFocus starSystem;

    List<Transform> possibleSpawnPoints;
    // Start is called before the first frame update
    void Start()
    {
        GetPossibleSpawnPoints();
        SpawnPOIs();
    }

    void SpawnPOIs()
    {
        Vector3 pos = GetPosition();
        Instantiate(asteroidPOIPrefab, pos, Quaternion.identity, transform);
    }

    void GetPossibleSpawnPoints()
    {
        List<Transform> activeChildren = new List<Transform>();
        foreach (AsteroidFieldOnWorldMap asteroidField in starSystem.AsteroidFields)
        {
            if (asteroidField.gameObject.activeSelf)
            {
                foreach (Transform child in asteroidField.transform)
                {
                    if (child.gameObject.activeSelf)
                    {
                        activeChildren.Add(child);
                    }
                }
            }
        }
        possibleSpawnPoints = activeChildren;
    }

    Vector3 GetPosition()
    {
        Transform randomAsteroid = possibleSpawnPoints[Random.Range(0, possibleSpawnPoints.Count)];
        Vector3 spawnPoint = randomAsteroid.position;
        possibleSpawnPoints.Remove(randomAsteroid);
        return spawnPoint;
    }
}
