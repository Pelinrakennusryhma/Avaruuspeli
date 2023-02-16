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
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ProjectedPosition = GetPosition(10f);
    }

    public Vector3 GetPosition(float projectileSpeed)
    {
        return transform.position + (rb.velocity * projectileSpeed * Time.deltaTime);
    }
}
