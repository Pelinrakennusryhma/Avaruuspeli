using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjection : MonoBehaviour
{
    Vector3 projectedPos;
    public Vector3 ProjectedPosition
    {
        get { return projectedPos; }
        private set { projectedPos = value; }
    }

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public Vector3 GetPosition(float projectileSpeed, Vector3 shooterPosition)
    {
        float distance = Vector3.Distance(shooterPosition, transform.position);
        float factor = distance / projectileSpeed;
        Debug.Log("dist: " + distance + "factor: " + factor);
        return transform.position + (rb.velocity * factor);
    }
}
