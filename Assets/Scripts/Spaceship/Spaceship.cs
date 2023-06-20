using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spaceship : MonoBehaviour, IDamageable
{
    [field: SerializeField] 
    public Transform BehindCamera { get; private set; }
    [field: SerializeField] 
    public Transform CockpitCamera { get; private set; }
    SpaceshipEvents spaceshipEvents;
    SpaceshipHealth spaceshipHealth;
    ActorSpaceship actor;

    void Awake()
    {
        spaceshipEvents = GetComponent<SpaceshipEvents>();
        spaceshipHealth = GetComponent<SpaceshipHealth>();
        spaceshipEvents.EventSpaceshipCollided.AddListener(OnCollision);
        actor = GetComponentInParent<ActorSpaceship>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Avoid collision with first person character, mostly when landing/leaving asteroid
        if (!collision.gameObject.CompareTag("Player"))
        {
            spaceshipEvents.CallEventSpaceshipCollided(collision.relativeVelocity.magnitude);
        }     
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

    public void Damage(float damage, GameObject source)
    {
        if(actor != null)
        {
            if (Utils.ListContains<SpaceshipShield>(actor.ActiveUtils))
            {
                Debug.Log("Shield up, damage blocked");
                return;
            }
        }

        spaceshipHealth.DecreaseHealth((int)damage);

        if(source != null)
        {
            ActorSpaceship sourceActor = source.transform.GetComponentInParent<ActorSpaceship>();

            // flash projection icon when hit by player(faction)
            if (sourceActor != null && sourceActor.faction.factionName == "Player")
            {
                spaceshipEvents.CallEventSpaceshipHitByPlayer();
            }
        }
    }
}
