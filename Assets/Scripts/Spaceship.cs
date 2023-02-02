using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour, IDamageable
{
    SpaceshipEvents spaceshipEvents;
    SpaceshipHealth spaceshipHealth;

    void Awake()
    {
        spaceshipEvents = GetComponent<SpaceshipEvents>();
        spaceshipHealth = GetComponent<SpaceshipHealth>();
        spaceshipEvents.EventSpaceshipCollided.AddListener(OnCollision);
    }

    private void OnCollisionEnter(Collision collision)
    {
        spaceshipEvents.CallEventSpaceshipCollided(collision.relativeVelocity.magnitude);
    }

    private void OnCollision(float magnitude)
    {
        // temporary collision damage calculations
        if(magnitude > 10f)
        {
            float damageTaken = magnitude / 3f;
            spaceshipHealth.DecreaseHealth((int)damageTaken);
        }
    }

    public void Damage(float damage)
    {
        spaceshipHealth.DecreaseHealth((int)damage);
    }
}
