using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Laser SFX")]
    [field: SerializeField] public EventReference LaserShoot { get; private set; }
    public static FMODEvents Instance { get; private set; }

    void Start()
    {
        if (Instance == null)
        {
            Debug.LogWarning("More than one FMODEvents around!");
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
