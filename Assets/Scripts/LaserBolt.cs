using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    GameObject shooterShip;
    float force = 0f;
    ParticleSystem explosionEffect;

    public void Init(float shootForce, float lifetime, GameObject _shooterShip, Vector3 shooterVelocity)
    {
        force = shootForce;
        shooterShip = _shooterShip;
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        StartCoroutine(DestroySelf(lifetime));

    }

    IEnumerator DestroySelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(this.gameObject);
    }

    private void Update()
    {
        transform.position += transform.forward * Time.deltaTime * force;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != shooterShip)
        {
            Debug.Log("Laser hit: " + other.gameObject.name);
            explosionEffect.Play();
            float destroyDelay = explosionEffect.main.duration + 0.1f;
            StartCoroutine(DestroySelf(destroyDelay));
        }
    }
}
