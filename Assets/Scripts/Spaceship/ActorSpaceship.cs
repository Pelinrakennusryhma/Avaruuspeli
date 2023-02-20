using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorSpaceship : MonoBehaviour
{
    public FactionEnum faction;
    public SpaceshipMovement spaceshipMovement;
    public SpaceshipBoost spaceshipBoost;
    public SpaceshipShoot spaceshipShoot;
    public SpaceshipEvents spaceshipEvents;
    public TargetProjection targetProjection;

    virtual protected void Awake()
    {
        spaceshipMovement = GetComponentInChildren<SpaceshipMovement>();
        spaceshipBoost = GetComponentInChildren<SpaceshipBoost>();
        spaceshipShoot = GetComponentInChildren<SpaceshipShoot>();
        spaceshipEvents = GetComponentInChildren<SpaceshipEvents>();
        targetProjection = GetComponentInChildren<TargetProjection>();

        spaceshipEvents.EventSpaceshipDied.AddListener(OnDeath);
    }

    virtual protected void Start()
    {
        GameEvents.instance.CallEventSpaceshipSpawned(this);
    }

    virtual protected void OnDeath()
    {
        Debug.Log("spaceship died: " + transform.name + " faction: " + faction);
        GameEvents.instance.CallEventSpaceshipDied(this);
    }
}
