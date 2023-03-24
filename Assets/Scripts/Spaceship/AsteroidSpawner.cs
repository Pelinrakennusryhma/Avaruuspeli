using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum MineableRockDensity
{
    Scarce,
    Medium,
    High
}

public class AsteroidSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] asteroidPrefabs;
    [SerializeField]
    private GameObject mineableAsteroidPrefab;
    [SerializeField]
    private GameObject asteroidParent;
    [SerializeField]
    private int amountOfAsteroids = 500;
    [SerializeField]
    private int amountOfMineables = 1;
    [SerializeField]
    private AnimationCurve sizeCurve;
    [SerializeField]
    private AnimationCurve mineableCurve;
    [SerializeField]
    private float spawnArea = 1500f;
    [SerializeField]
    private ActorManager actorManager;
    [SerializeField]
    MineableRockDensity mineableRockDensity;
    [SerializeField]
    Resource resourceType;
    // Start is called before the first frame update
    void Start()
    {
        SpawnAsteroids();
        SpawnMineableAsteroids();
    }

    void SpawnAsteroids()
    {
        //SceneManager.LoadScene(mineableScene, LoadSceneMode.Additive);

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

    void SpawnMineableAsteroids()
    {
        for (int i = 0; i < amountOfMineables; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * spawnArea;
            Quaternion spawnRot = Random.rotation;
            GameObject mineable = Instantiate(mineableAsteroidPrefab, spawnPos, spawnRot, asteroidParent.transform);
            MineableAsteroidTrigger mineableScript = mineable.GetComponent<MineableAsteroidTrigger>();
            GameObject asteroidModel = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            float randomValue = Random.Range(0f, 1f);
            float scale = mineableCurve.Evaluate(randomValue);
            mineableScript.Init(asteroidModel, scale, mineableRockDensity, resourceType, actorManager);
        }
    }
}
