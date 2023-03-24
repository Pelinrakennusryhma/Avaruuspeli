using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterOfGravity : MonoBehaviour
{
    public float GravityMultiplier = 1.0f;

    public Collider Collider;

    public void Start()
    {
        Collider = GetComponent<Collider>();
    }

}
