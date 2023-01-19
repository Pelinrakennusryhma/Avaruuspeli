using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject asteroidParent;
    [SerializeField]
    private int amountOfAsteroids = 500;
    [SerializeField]
    private float spawnArea = 1500f;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAsteroids();
    }

    void SpawnAsteroids()
    {
        for (int i = 0; i < amountOfAsteroids; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * spawnArea;
            Quaternion spawnRot = Random.rotation;

            Instantiate(asteroidPrefab, spawnPos, spawnRot, asteroidParent.transform);
        }
    }
}
