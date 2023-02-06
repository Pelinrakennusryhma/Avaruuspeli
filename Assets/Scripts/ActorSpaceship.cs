using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorSpaceship : MonoBehaviour
{
    public SpaceshipMovement spaceshipMovement;
    public SpaceshipBoost spaceshipBoost;
    public SpaceshipShoot spaceshipShoot;
    public SpaceshipEvents spaceshipEvents;

    void Awake()
    {
        spaceshipMovement = GetComponentInChildren<SpaceshipMovement>();
        spaceshipBoost = GetComponentInChildren<SpaceshipBoost>();
        spaceshipShoot = GetComponentInChildren<SpaceshipShoot>();
        spaceshipEvents = GetComponentInChildren<SpaceshipEvents>();

        spaceshipEvents.EventSpaceshipDied.AddListener(OnDeath);
    }

    abstract protected void OnDeath();
}
