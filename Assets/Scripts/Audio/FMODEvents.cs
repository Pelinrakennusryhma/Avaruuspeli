using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    [field: Header("Music")]
    [field:SerializeField] public EventReference Music { get; private set; }
    [field: Header("Ship SFX")]
    [field: SerializeField] public EventReference ShipEngine { get; private set; }
    [field: SerializeField] public EventReference Shield { get; private set; }
    [field: SerializeField] public EventReference LaserShoot { get; private set; }
    [field: SerializeField] public EventReference Explosion { get; private set; }
    [field: SerializeField] public EventReference MissileLaunch { get; private set; }
    [field: Header("Player Ship SFX")]
    [field: SerializeField] public EventReference Alarm { get; private set; }
    [field: Header("First Person SFX")]
    [field: SerializeField] public EventReference Drill { get; private set; }

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
