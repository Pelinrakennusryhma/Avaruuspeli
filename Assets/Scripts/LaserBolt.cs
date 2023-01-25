using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    Rigidbody rb;
    GameObject shooterShip;
    float force;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Init(float shootForce, float lifetime, GameObject _shooterShip, Vector3 shooterVelocity)
    {
        force = shootForce;
        rb.velocity = shooterVelocity;
        shooterShip = _shooterShip;
        StartCoroutine(DestroySelf(lifetime));

    }

    IEnumerator DestroySelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    void FixedUpdate()
    {
        rb.AddRelativeForce(Vector3.forward * force * Time.fixedDeltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != shooterShip)
        {
            Debug.Log("Laser hit: " + other.gameObject.name);
        }
    }
}
