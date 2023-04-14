using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorSpaceship : MonoBehaviour
{
    public Faction faction;
    public GameObject ship;
    public SpaceshipMovement spaceshipMovement;
    public SpaceshipBoost spaceshipBoost;
    public SpaceshipShoot spaceshipShoot;
    public SpaceshipEvents spaceshipEvents;
    public SpaceshipMissile spaceshipMissile;
    public TargetProjection targetProjection;

    virtual protected void OnEnable()
    {
        ship = transform.GetChild(0).gameObject;
        spaceshipMovement = ship.GetComponent<SpaceshipMovement>();
        spaceshipBoost = ship.GetComponent<SpaceshipBoost>();
        spaceshipShoot = ship.GetComponent<SpaceshipShoot>();
        spaceshipEvents = ship.GetComponent<SpaceshipEvents>();
        spaceshipMissile = ship.GetComponent<SpaceshipMissile>();
        targetProjection = ship.GetComponent<TargetProjection>();

        spaceshipEvents.EventSpaceshipDied.AddListener(OnDeath);
    }

    virtual protected void Start()
    {
        GameEvents.Instance.CallEventSpaceshipSpawned(this);
    }

    virtual protected void OnDeath()
    {
        Debug.Log("spaceship died: " + transform.name + " faction: " + faction.factionName);
        GameEvents.Instance.CallEventSpaceshipDied(this);
    }

    public Vector3 GetProjectedPosition(float projectileSpeed, Vector3 shooterPosition)
    {
        return targetProjection.GetPosition(projectileSpeed, shooterPosition);
    }

    public virtual void LockMissile(ActorSpaceship shooter)
    {
        Debug.Log("missile locked");
    }

    public virtual void UnlockMissile(ActorSpaceship shooter)
    {
        Debug.Log("missile unlocked");
    }
}
