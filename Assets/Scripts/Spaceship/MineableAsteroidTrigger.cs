using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AsteroidLauncher;

public class MineableAsteroidTrigger : MonoBehaviour
{
    [field: SerializeField]
    public Transform ShipPosition { get; private set; }
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
    Transform rockPositionsParent;
    [SerializeField]
    int amountOfRocks = 1;

    [SerializeField]
    ResourceInventory.ResourceType resourceType;
    string successText = "Press %landKey% to land on the asteroid";
    string failureText = "Clear the area of hostiles before landing on the asteroid.";
    string currentText;
    bool playerInTriggerArea = false;
    MeshFilter meshFilter;


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
        SpawnRocks();
    }

    private void SpawnRocks()
    {
        for (int i = 0; i < amountOfRocks; i++)
        {
            Vector3 spawnPos = FindVertexOnMesh();
            Debug.Log("spawnPos: " + spawnPos);
            GameObject rock = Instantiate(mineableRockPrefabs[0], spawnPos, Random.rotation, transform);
            DestroyableRock destroyableRock = rock.GetComponent<DestroyableRock>();
            destroyableRock.Init(resourceType, MineralDensity.Highest, CenterOfGravity);
        }

        //int maxAmount = Mathf.Min(amountOfRocks, rockPositionsParent.childCount);
        //for (int i = 0; i < maxAmount; i++)
        //{
        //    int rockId = 0;
        //    Transform spawnPos = rockPositionsParent.GetChild(i);
        //    GameObject rock = Instantiate(mineableRockPrefabs[rockId], spawnPos.position, spawnPos.rotation, transform);
        //    DestroyableRock destroyableRock = rock.GetComponent<DestroyableRock>();
        //    destroyableRock.Init(resourceType, MineralDensity.Highest, CenterOfGravity);
        //}
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
