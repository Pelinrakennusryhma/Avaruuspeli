using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCollision : MonoBehaviour
{
    private Rigidbody rb;
    private bool collided = false;
    private float collisionTimer = 1f;
    private float originalDrag;
    private float originalAngularDrag;
    [SerializeField]
    private float collisionDrag = 100f;
    [SerializeField]
    private float collisionAngularDrag = 100f;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalDrag = rb.drag;
        originalAngularDrag = rb.angularDrag;
    }

    private void FixedUpdate()
    {
        if (collided)
        {
            if(collisionTimer > 0f)
            {
                rb.drag = collisionDrag;
                rb.angularDrag = collisionAngularDrag;
                collisionTimer -= Time.fixedDeltaTime;
            } else
            {
                rb.drag = originalDrag;
                rb.angularDrag = originalAngularDrag;
                collided = false;
                collisionTimer = 1f;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        collided = true;
        Debug.Log("collided");
    }
}
