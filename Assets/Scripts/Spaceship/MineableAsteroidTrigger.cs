using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableAsteroidTrigger : MonoBehaviour
{
    public Transform shipPosition;
    [field: SerializeField]
    public Transform CharacterPosition { get; private set; }
    [SerializeField]
    ActorManager actorManager;
    [SerializeField]
    Target targetScript;
    [field: SerializeField]
    public CenterOfGravity CenterOfGravity { get; private set; }
    [SerializeField]
    GameObject[] mineableRockPrefabs;
    [SerializeField]
    int amountOfRocks;

    [SerializeField]
    Item resourceType;
    string successText = "Press %landKey% to land on the asteroid.";
    string failureText = "Clear the area of hostiles before landing on the asteroid.";
    string currentText;
    bool playerInTriggerArea = false;
    MeshFilter meshFilter;
    List<GameObject> spawnedRocks = new List<GameObject>();
    float minSpawnRange = 2f;
    int allowedSearches = 5;


    private void Awake()
    {
        GameEvents.Instance.EventEnemiesKilled.AddListener(OnEnemiesKilled);
        GameEvents.Instance.EventPlayerTriedLanding.AddListener(OnLandingAttempt);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
        GameEvents.Instance.EventToggleIndicators.AddListener(OnToggleIndicators);
    }

    private void Start()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        SetupTargetScript();
        SpawnRocks(amountOfRocks);
    }

    void SetupTargetScript()
    {
        Target targetScript = GetComponent<Target>();
        if(targetScript != null)
        {
            targetScript.descriptionText = $"Asteroid ({resourceType.plural})";
        }
    }

    private void SpawnRocks(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            int searchCount = allowedSearches;

            // Keep searching to our limit.
            while (searchCount-- > 0)
            {
                // Choose a random position.
                Vector3 spawnPos = FindVertexOnMesh();

                // Is the position is empty?
                if (IsPositionEmpty(spawnPos))
                {
                    // Yes, so add to the list.
                    GameObject rockPrefab = mineableRockPrefabs[Random.Range(0, mineableRockPrefabs.Length)];
                    GameObject rock = Instantiate(rockPrefab, spawnPos, Random.rotation, transform);
                    spawnedRocks.Add(rock);
                    DestroyableRock destroyableRock = rock.GetComponent<DestroyableRock>();
                    destroyableRock.Init(resourceType, CenterOfGravity);

                    break;
                }
            }


        }
    }

    private bool IsPositionEmpty(Vector3 position)
    {
        // This will increase in time as we get more items.
        foreach (GameObject existingRock in spawnedRocks)
        {
            if (Vector3.Distance(position, existingRock.transform.position) < minSpawnRange)
                return false;
        }

        return true;
    }

    Vector3 FindVertexOnMesh()
    {
        Vector3[] allVerts = meshFilter.mesh.vertices;
        Vector3 randomVert = allVerts[Random.Range(0, allVerts.Length)];
        return meshFilter.transform.TransformPoint(randomVert);
    }

    void OnLandingAttempt()
    {
        if (playerInTriggerArea && actorManager.SceneCleared)
        {
            //LaunchAsteroidScene(AsteroidType.Asteroid01a, ResourceInventory.ResourceType.Iron, MineralDensity.Medium, true);
            Land();
        }
    }

    void Land()
    {
        GameEvents.Instance.CallEventPlayerLanded(this);
        CenterOfGravity.enabled = true;
    }

    void OnLeaveAsteroid(MineableAsteroidTrigger asteroid)
    {
        if(asteroid == this)
        {
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
            CenterOfGravity.enabled = false;
        }
    }

    void OnEnemiesKilled()
    {
        currentText = successText;
        if (playerInTriggerArea)
        {
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
        }
    }

    void OnToggleIndicators(bool showIndicator)
    {
        targetScript.enabled = showIndicator;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            playerInTriggerArea = true;
            if (actorManager.SceneCleared)
            {
                currentText = successText;
            } else
            {
                currentText = failureText;
            }
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            playerInTriggerArea = false;
            GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
        }
    }
}
