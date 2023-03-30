using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POISpawner : MonoBehaviour
{
    [SerializeField]
    GameObject asteroidPOIPrefab;
    [SerializeField]
    StarSystemOnFocus starSystem;
    [SerializeField]
    POISceneData[] allPOISceneData;

    // Variables for spawning new POIs
    [SerializeField]
    int minPois = 1;
    [SerializeField]
    int maxPois = 3;
    [SerializeField] int maxAllowedPois;
    [SerializeField]
    float minSpawnDelay = 10f;
    [SerializeField]
    float maxSpawnDelay = 30f;
    private List<PointOfInterest> activePOIs = new List<PointOfInterest>();
    float nextSpawnTime = 10f;
    float spawnTimer = 0f;

    List<Transform> possibleSpawnPoints;

    void Start()
    {
        maxAllowedPois = Random.Range(minPois, maxPois + 1);
        GetPossibleSpawnPoints();
    }

    private void OnEnable()
    {
        spawnTimer = 0f;
        nextSpawnTime = GetSpawnTime();
    }

    private void Update()
    {
        spawnTimer += Time.deltaTime;
        if(spawnTimer > nextSpawnTime && activePOIs.Count < maxAllowedPois)
        {
            SpawnPOI();
            nextSpawnTime = GetSpawnTime();
            spawnTimer = 0f;
        }
    }

    float GetSpawnTime()
    {
        return Random.Range(minSpawnDelay, maxSpawnDelay);
    }

    void SpawnPOI()
    {
        Vector3 pos = GetPosition();
        GameObject instantiatedPOI = Instantiate(asteroidPOIPrefab, pos, Quaternion.identity, transform);
        PointOfInterest pointOfInterest = instantiatedPOI.GetComponent<PointOfInterest>();
        activePOIs.Add(pointOfInterest);
        pointOfInterest.Init(GetSceneData(), this);
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

    POISceneData GetSceneData()
    {
        int index = Random.Range(0, allPOISceneData.Length);
        return allPOISceneData[index];
    }

    public void RemovePOI(PointOfInterest POI) 
    { 
        activePOIs.Remove(POI);
    }
}
