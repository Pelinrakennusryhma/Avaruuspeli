using UnityEngine;

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
    private BoxCollider asteroidArea;
    [SerializeField]
    private BoxCollider reachableArea;
    [SerializeField]
    private int amountOfAsteroids = 500;
    [SerializeField]
    private int amountOfMineables = 1;
    [SerializeField]
    private AnimationCurve sizeCurve;
    [SerializeField]
    private AnimationCurve mineableSizeCurve;
    [SerializeField]
    private AnimationCurve mineableDistanceCurve;
    [SerializeField]
    private ActorManager actorManager;
    [SerializeField]
    MineableRockDensity mineableRockDensity;
    [SerializeField]
    Resource resourceType;

    float reach = 3000;

    // Start is called before the first frame update
    void Start()
    {
        SpawnNonMineableAsteroids();
        SpawnMineableAsteroids();
    }

    void SpawnNonMineableAsteroids()
    {
        for (int i = 0; i < amountOfAsteroids; i++)
        {
            GameObject asteroidToSpawn = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];


            Vector3 spawnPos = GetPositionInSpawnArea(asteroidArea.bounds);
            Quaternion spawnRot = Random.rotation;

            GameObject asteroid = Instantiate(asteroidToSpawn, spawnPos, spawnRot, asteroidParent.transform);
            float distanceFromOrigo = Vector3.Distance(Vector3.zero, spawnPos);
            if (distanceFromOrigo > reach * 1.5f)
            {
                Destroy(asteroid.GetComponent<Rigidbody>());
                Destroy(asteroid.GetComponent<MeshCollider>());
            }

            float randomValue = Random.Range(0f, 1f);
            float scale = sizeCurve.Evaluate(randomValue);
            asteroid.transform.localScale *= scale;
        }
    }

    void SpawnMineableAsteroids()
    {
        reachableArea.size = new Vector3(reach, reachableArea.size.y, reachableArea.size.z);
        for (int i = 0; i < amountOfMineables; i++)
        {
            Vector3 spawnPos = GetPositionInSpawnArea(reachableArea.bounds, true);
            Quaternion spawnRot = Random.rotation;
            GameObject mineable = Instantiate(mineableAsteroidPrefab, spawnPos, spawnRot, asteroidParent.transform);
            MineableAsteroidTrigger mineableScript = mineable.GetComponent<MineableAsteroidTrigger>();
            GameObject asteroidModel = asteroidPrefabs[Random.Range(0, asteroidPrefabs.Length)];
            float randomValue = Random.Range(0f, 1f);
            float scale = mineableSizeCurve.Evaluate(randomValue);
            mineableScript.Init(asteroidModel, scale, mineableRockDensity, resourceType, actorManager);
        }
    }

    Vector3 GetPositionInSpawnArea(Bounds bounds, bool useCurve=false)
    {
        if (useCurve)
        {
            float x = Mathf.Lerp(bounds.min.x, bounds.max.x, mineableDistanceCurve.Evaluate(Random.value));
            float y = Mathf.Lerp(bounds.min.y, bounds.max.y, mineableDistanceCurve.Evaluate(Random.value));
            float z = Mathf.Lerp(bounds.min.z, bounds.max.z, mineableDistanceCurve.Evaluate(Random.value));
            return new Vector3(x, y, z);
        } else
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
                );
        }
    }
}
