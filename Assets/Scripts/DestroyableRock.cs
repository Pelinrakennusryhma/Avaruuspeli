using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyableRock : MonoBehaviour
{
    [field: SerializeField]
    public Resource ResourceType { get; private set; }
    [SerializeField]
    int minResourceAmount;
    [SerializeField]
    int maxResourceAmount;
    [field: SerializeField]
    public int ResourceAmount { get; private set; }

    public float Health;

    public GameObject Graphics;
    public GameObject Rubble;
    public GameObject Spawnables;

    public SpawnablePickups Pickups;

    private MeshCollider OwnCollider;

    private RubblePiece[] RubblePieces;

    public ParticleSystem DeathParticles;

    public VisualEffect Explosion;

    public Vector3 OriginalGraphicsLocalPosition;

    private List<GatherableObject> spawnedResources = new List<GatherableObject>();
    CenterOfGravity _centerOfGravity;
    MeshFilter meshFilter;

    // Start is called before the first frame update

    public void Awake()
    {
        OriginalGraphicsLocalPosition = Graphics.transform.localPosition;
        Health = 1.0f;
        Graphics.SetActive(true);
        Rubble.SetActive(false);
        Spawnables.SetActive(false);
        OwnCollider = GetComponent<MeshCollider>();
        OwnCollider.enabled = true;

        RubblePieces = GetComponentsInChildren<RubblePiece>(true);
        DeathParticles.Stop(true);
        Explosion = GetComponentInChildren<VisualEffect>(true);
        Explosion.playRate = 1.5f;
        Explosion.Stop();
        meshFilter = GetComponentInChildren<MeshFilter>();

        // Here could be a random chance?

        //if (AsteroidLauncher.ResourceType == ResourceInventory.ResourceType.None)
        //{
        //    AsteroidLauncher.Setup(false);
        //    //Debug.Log("Had to setup asteroid launcher");
        //}

        //ResourceType = AsteroidLauncher.ResourceType;
        //ResourceAmount = Pickups.Setup(AsteroidLauncher.ResourceType);
    }

    public void Init(Resource resourceType, CenterOfGravity centerOfGravity)
    {
        _centerOfGravity = centerOfGravity;
        ResourceType = resourceType;
        ResourceAmount = Random.Range(minResourceAmount, maxResourceAmount + 1);
        //ResourceAmount = Pickups.Setup(resourceType, mineralDensity, centerOfGravity);
        //Pickups.Decorate(gameObject);
        //Debug.Log("Initting destroyable rock " + Time.time);

        SpawnResources();
    }

    void SpawnResources()
    {
        for (int i = 0; i < ResourceAmount; i++)
        {

            GameObject resourceVariant = GetResourceToSpawn(ResourceType);
            Vector3 spawnPos = FindVertexOnMesh();
            GameObject spawnedResource = Instantiate(resourceVariant, spawnPos, Random.rotation, Graphics.transform);
            GatherableObject gatherableObject = spawnedResource.GetComponent<GatherableObject>();


            gatherableObject.Init(ResourceType);
            gatherableObject.enabled = false;
            spawnedResources.Add(gatherableObject);
        }
    }

    GameObject GetResourceToSpawn(Resource resourceType)
    {
        Resource resourceToSpawn = resourceType;
        if (ResourceType.rare != null)
        {
            float randomValue = Random.value;
            if (randomValue < ResourceType.rareChance)
            {
                resourceToSpawn = resourceType.rare;
            }
        }
        int numVariants = resourceToSpawn.itemPrefabVariants.Length;
        return resourceToSpawn.itemPrefabVariants[Random.Range(0, numVariants)];
    }

    Vector3 FindVertexOnMesh()
    {
        Vector3[] allVerts = meshFilter.mesh.vertices;
        Vector3 randomVert = allVerts[Random.Range(0, allVerts.Length)];
        return meshFilter.transform.TransformPoint(randomVert);
    }

    void EnableResources()
    {
        foreach (GatherableObject gatherable in spawnedResources)
        {
            gatherable.transform.parent = transform;
            gatherable.enabled = true;
            gatherable.Activate(_centerOfGravity);
        }
    }

    public void ReduceHealth(float amount, 
                             ResourceGatherer.ToolType tool)
    {
        Health -= amount;

        //Debug.Log("Current health is " + Health);

        ShakeRock(tool);
      
        if(Health <= 0.0f)
        {
            //Debug.Log("Rock destroyed. Spawn shit");
            Invoke("DestroySelf", 15f);
            Graphics.SetActive(false);
            Rubble.SetActive(true);
            EnableResources();
            //Spawnables.SetActive(true);
            OwnCollider.enabled = false;
            DeathParticles.Play(true);
            Explosion.Play();

            //Pickups.Spawn();

            for (int i = 0; i < RubblePieces.Length; i++)
            {
                RubblePieces[i].Spawn(transform.position, _centerOfGravity);
            }
        }
    }

    private void ShakeRock(ResourceGatherer.ToolType tool)
    {
        float severity = 1.0f;

        if (tool == ResourceGatherer.ToolType.BasicDrill)
        {
            severity = 1.4f;
        }

        else if (tool == ResourceGatherer.ToolType.AdvancedDrill)
        {
            severity = 4.0f;
        }

        else if (tool == ResourceGatherer.ToolType.DiamondDrill)
        {
            severity = 6.5f;
        }

        severity *= (1.0f - Health) * 4.0f;

        Graphics.gameObject.transform.localPosition = Vector3.Lerp(Graphics.gameObject.transform.localPosition,
                                                                   (OriginalGraphicsLocalPosition +
                                                                    new Vector3(Random.Range(-severity, severity), 
                                                                                Random.Range(-severity, severity), 
                                                                                Random.Range(-severity, severity))), 
                                                                    2.9f * Time.deltaTime);
    }

    private void DestroySelf()
    {
        Destroy(gameObject);
    }
}
