using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    Rigidbody rb;
    float force;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(float shootForce, float lifetime, Vector3 shooterVelocity)
    {
        force = shootForce;
        rb.velocity = shooterVelocity;
        StartCoroutine(DestroySelf(lifetime));

    }

    IEnumerator DestroySelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    private void Update()
    {

    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * force * Time.fixedDeltaTime);
    }
}
