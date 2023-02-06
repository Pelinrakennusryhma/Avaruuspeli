using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableRock : MonoBehaviour
{
    public float Health;

    public GameObject Graphics;
    public GameObject Rubble;
    public GameObject Spawnables;

    private MeshCollider OwnCoollider;
    // Start is called before the first frame update

    public void Awake()
    {
        Health = 1.0f;
        Graphics.SetActive(true);
        Rubble.SetActive(false);
        Spawnables.SetActive(false);
        OwnCoollider = GetComponent<MeshCollider>();
        OwnCoollider.enabled = true;
    }

    public void ReduceHealth(float amount)
    {
        Health -= amount;

        Debug.Log("Current health is " + Health);

        if(Health <= 0.0f)
        {
            Debug.Log("Rock destroyed. Spawn shit");
            Graphics.SetActive(false);
            Rubble.SetActive(true);
            Spawnables.SetActive(true);
            OwnCoollider.enabled = false;
        }
    }
}
