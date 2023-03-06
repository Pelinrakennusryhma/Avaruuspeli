using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class DestroyableRock : MonoBehaviour
{
    public ResourceInventory.ResourceType ResourceType;
    public int ResourceAmount;

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

        // Here could be a random chance?

        if (AsteroidLauncher.ResourceType == ResourceInventory.ResourceType.None)
        {
            AsteroidLauncher.Setup(false);
            //Debug.Log("Had to setup asteroid launcher");
        }

        ResourceType = AsteroidLauncher.ResourceType;
        ResourceAmount = Pickups.Setup(AsteroidLauncher.ResourceType);
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
            Graphics.SetActive(false);
            Rubble.SetActive(true);
            Spawnables.SetActive(true);
            OwnCollider.enabled = false;
            DeathParticles.Play(true);
            Explosion.Play();

            Pickups.Spawn();

            for (int i = 0; i < RubblePieces.Length; i++)
            {
                RubblePieces[i].Spawn(transform.position);
            }
        }
    }

    private void ShakeRock(ResourceGatherer.ToolType tool)
    {
        float severity = 1.0f;

        if (tool == ResourceGatherer.ToolType.Blowtorch)
        {
            severity = 1.4f;
        }

        else if (tool == ResourceGatherer.ToolType.Drill)
        {
            severity = 4.0f;
        }

        severity *= (1.0f - Health) * 4.0f;

        Graphics.gameObject.transform.localPosition = Vector3.Lerp(Graphics.gameObject.transform.localPosition,
                                                                   (OriginalGraphicsLocalPosition +
                                                                    new Vector3(Random.Range(-severity, severity), 
                                                                                Random.Range(-severity, severity), 
                                                                                Random.Range(-severity, severity))), 
                                                                    2.9f * Time.deltaTime);
    }
}
