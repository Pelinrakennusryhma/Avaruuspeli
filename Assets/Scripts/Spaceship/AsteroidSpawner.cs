using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] asteroidPrefabs;
    [SerializeField]
    private GameObject asteroidParent;
    [SerializeField]
    private int amountOfAsteroids = 500;
    [SerializeField]
    private AnimationCurve sizeCurve;
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
            GameObject asteroidToSpawn = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];


            Vector3 spawnPos = Random.insideUnitSphere * spawnArea;
            Quaternion spawnRot = Random.rotation;

            GameObject asteroid = Instantiate(asteroidToSpawn, spawnPos, spawnRot, asteroidParent.transform);

            float randomValue = Random.Range(0f, 1f);
            float scale = sizeCurve.Evaluate(randomValue);
            asteroid.transform.localScale *= scale;
        }
    }
}
