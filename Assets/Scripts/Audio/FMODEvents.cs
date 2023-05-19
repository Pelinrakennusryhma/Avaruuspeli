using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Ship SFX")]
    [field: SerializeField] public EventReference ShipEngine { get; private set; }

    [field: Header("Laser SFX")]
    [field: SerializeField] public EventReference LaserShoot { get; private set; }
    public static FMODEvents Instance { get; private set; }

    void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("More than one FMODEvents around!");
        }
        Instance = this;
    }
}