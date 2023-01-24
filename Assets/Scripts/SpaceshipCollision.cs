using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipCollision : MonoBehaviour
{
    private Rigidbody rb;
    private bool collided = false;
    [SerializeField]
    private float dragDuration = 1f;
    [SerializeField]
    private float velocityThreshold = 1000f;
    private float collisionTimer;
    private float originalAngularDrag;
    [SerializeField]
    private float collisionAngularDrag = 100f;

    private float currentSpeed;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalAngularDrag = rb.angularDrag;
        collisionTimer = dragDuration;
    }

    private void FixedUpdate()
    {
        currentSpeed = rb.velocity.sqrMagnitude;
        if (collided)
        {
            if(collisionTimer > 0f)
            {
                rb.angularDrag = collisionAngularDrag;
                collisionTimer -= Time.fixedDeltaTime;
            } else
            {
                rb.angularDrag = originalAngularDrag;
                collided = false;
                collisionTimer = dragDuration;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(currentSpeed > velocityThreshold)
        {
            collided = true;
            //Debug.Log("Collided: " + currentSpeed);
        }
    }
}
