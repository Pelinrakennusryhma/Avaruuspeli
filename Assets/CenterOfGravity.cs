using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfGravity : MonoBehaviour
{
    public float GravityMultiplier = 1.0f;

    public static CenterOfGravity Instance;

    public Collider Collider;

    public void Awake()
    {
        Instance = this;
        Collider = GetComponent<Collider>();
    }

}
