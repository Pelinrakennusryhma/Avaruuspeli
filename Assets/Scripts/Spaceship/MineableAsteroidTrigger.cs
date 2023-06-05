using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineableAsteroidTrigger : MonoBehaviour
{
    public Transform shipPosition;
    [field: SerializeField]
    public Transform CharacterPosition { get; private set; }
    ActorManager _actorManager;
    [field: SerializeField]
    public CenterOfGravity CenterOfGravity { get; private set; }
    [field: SerializeField]
    public MeshCollider Collider { get; private set; }
    [field: SerializeField]
    public SphereCollider TriggerCollider { get; private set; }
    [SerializeField]
    GameObject[] mineableRockPrefabs;

    [SerializeField]
    Resource _resourceType;
    string successText = "Press %landKey% to land on the asteroid.";
    string failureText = "Clear the area of hostiles before landing on the asteroid.";


    string noSpacesuitText = "Equip a spacesuit before landing on the asteroid.";
    string noOxygenBottlesText = "You need oxygen bottles before landing on the asteroid.";
    string noSpacesuitNoOxygenBottlesText = "Equip space suit and get some oxygen bottles before landing on the asteroid.";

    string currentText;
    bool playerInTriggerArea = false;
    [SerializeField]
    MeshFilter meshFilter;
    List<GameObject> spawnedRocks = new List<GameObject>();
    float minSpawnRange = 2f;
    int allowedSearches = 5;

    private bool HadSpaceSuitOnFirstTriggerEnterAttempt = false;

    private bool playerHasLanded;


    private void Awake()
    {
        GameEvents.Instance.EventEnemiesKilled.AddListener(OnEnemiesKilled);
        GameEvents.Instance.EventPlayerTriedLanding.AddListener(OnLandingAttempt);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);

        //GameEvents.Instance.EventToggleIndicators.AddListener(OnToggleIndicators);
        Debug.LogWarning("On toggle indicators removed, because the method doesn't exist anymore");
        GameEvents.Instance.EventInventoryClosed.AddListener(OnInventoryClose);
        //Debug.Log("Listener added to on leftasteroid" + Time.time + " gameobject is " + gameObject.name);
        

    }

    public void Init(GameObject asteroidPrefab, float scale, MineableRockDensity mineableRockDensity, Resource resourceType, ActorManager actorManager)
    {
        _actorManager = actorManager;
        GameObject asteroid = Instantiate(asteroidPrefab, transform);
        Collider = asteroid.GetComponent<MeshCollider>();
        meshFilter = asteroid.GetComponentInChildren<MeshFilter>();
        CenterOfGravity = asteroid.AddComponent<CenterOfGravity>();
        CenterOfGravity.enabled = false;
        Collider.transform.localScale *= scale;
        float colSize = Collider.bounds.size.magnitude;
        TriggerCollider.radius = colSize;

        int amountOfRocks = GetRockAmount(mineableRockDensity);
        _resourceType = resourceType;
        SpawnRocks(amountOfRocks, resourceType);
        SetupTargetScript();
    }

    void SetupTargetScript()
    {
        Target targetScript = GetComponent<Target>();
        if(targetScript != null)
        {
            targetScript.descriptionText = $"Asteroid ({_resourceType.plural})";
        }
    }

    private void SpawnRocks(int amount, Resource resourceType)
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
        if (playerInTriggerArea 
            && _actorManager.SceneCleared
            && GameManager.Instance.LifeSupportSystem.CheckIfWeCanEnterUnbreathableArea())
        {
            //LaunchAsteroidScene(AsteroidType.Asteroid01a, ResourceInventory.ResourceType.Iron, MineralDensity.Medium, true);
            Land();
        }
    }

    void Land()
    {
        GameEvents.Instance.CallEventPlayerLanded(this);
        CenterOfGravity.enabled = true;
        playerHasLanded = true;
    }

    void OnLeaveAsteroid(MineableAsteroidTrigger asteroid)
    {
        if(asteroid == this)
        {
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
            CenterOfGravity.enabled = false;
        }

        playerHasLanded = false;
    }

    void OnEnemiesKilled()
    {
        currentText = successText;
        if (playerInTriggerArea)
        {
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {

            Debug.LogWarning("We triggered here, scene cleared: " + _actorManager.SceneCleared);

            playerInTriggerArea = true;
            if (_actorManager.SceneCleared)
            {
                // Check if we have spacesuit equipped and some oxygen

                if (!GameManager.Instance.LifeSupportSystem.HasSpaceSuitEquipped
                    && GameManager.Instance.LifeSupportSystem.AmountOfOxygenTanks <= 0)
                {
                    currentText = noSpacesuitNoOxygenBottlesText;
                    HadSpaceSuitOnFirstTriggerEnterAttempt = false;
                }

                else if (!GameManager.Instance.LifeSupportSystem.HasSpaceSuitEquipped)
                {
                    currentText = noSpacesuitText;
                    HadSpaceSuitOnFirstTriggerEnterAttempt = false;
                    //Debug.Log("Setting the current text to " + currentText);
                }

                else if (GameManager.Instance.LifeSupportSystem.AmountOfOxygenTanks <= 0)
                {
                    currentText = noOxygenBottlesText;
                    HadSpaceSuitOnFirstTriggerEnterAttempt = true;
                }

                else 
                {

                    HadSpaceSuitOnFirstTriggerEnterAttempt = true;
                    currentText = successText;
                }
            } 
            else
            {
                currentText = failureText;        
            }

            //if (GameManager.Instance.LifeSupportSystem.JustRanOutOfOxygen)
            //{

            //    Debug.Log("We just ran out of oxygen. Make the UI reflect that");
            //    currentText = ranOutOfOxygenText;
            //    //GameManager.Instance.LifeSupportSystem.ClearJustRanOutOfOxygenBool();
            //}

            //else
            //{
            //    Debug.LogWarning("Do we have oxygen");
            //}
            Debug.Log("currentText: " + currentText);
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);

        }
        // TODO: destroy overlapping asteroids somehow
        //else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        //{
        //    MineableAsteroidTrigger mineableAsteroid = other.transform.parent.GetComponent<MineableAsteroidTrigger>();
        //    if (mineableAsteroid != null)
        //    {
        //        if(mineableAsteroid != this)
        //        {
        //            Destroy(other.transform.parent.gameObject);
        //        }
        //    } 
        //    else
        //    {
        //        Destroy(other.gameObject);
        //    }
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            playerInTriggerArea = false;
            GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
        }
    }

    private int GetRockAmount(MineableRockDensity density)
    {
        switch (density)
        {
            case MineableRockDensity.Scarce:
                return Random.Range(1, 2);
            case MineableRockDensity.Medium:
                return Random.Range(3, 6);
            case MineableRockDensity.High:
                return Random.Range(7, 9);
            default:
                return 0;
        }
    }

    private void Update()
    {
        if (playerInTriggerArea
            && !HadSpaceSuitOnFirstTriggerEnterAttempt
            && GameManager.Instance.LifeSupportSystem.HasSpaceSuitEquipped)
        {
            if (GameManager.Instance.LifeSupportSystem.AmountOfOxygenTanks > 0) 
            {
                if (_actorManager.SceneCleared)
                {
                    currentText = successText;
                }
                else
                {
                    currentText = failureText;
                }
            }
            else
            {
                currentText = noOxygenBottlesText;
            }
            
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
            Debug.Log("We are here, should put a suitable text out there. Text is " + currentText);
            HadSpaceSuitOnFirstTriggerEnterAttempt = true;
        }
    }

    private void OnInventoryClose()
    {
        if (playerInTriggerArea          
            && !playerHasLanded 
            &&  GameManager.Instance.LifeSupportSystem.HasSpaceSuitEquipped
            && GameManager.Instance.LifeSupportSystem.AmountOfOxygenTanks > 0
            && _actorManager.SceneCleared)
        {
            currentText = successText;
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
            //Debug.Log("Invenotry closed, showing prompt ");
        }
    }

}
