using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public abstract class ActorSpaceship : MonoBehaviour
{
    public Faction faction;
    public GameObject ship;
    public SpaceshipData spaceshipData;
    public SpaceshipMovement spaceshipMovement;
    public SpaceshipBoost spaceshipBoost;
    public SpaceshipShoot spaceshipShoot;
    public SpaceshipEvents spaceshipEvents;
    public SpaceshipMissile spaceshipMissile;
    public TargetProjection targetProjection;
    public bool InDanger { get { return lockedMissiles.Count > 0; } }

    protected List<Missile> lockedMissiles = new List<Missile>();

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

        InitUtilities();
    }

    void InitUtilities()
    {
        if(spaceshipData != null && spaceshipData.utilities != null)
        {
            foreach (ShipUtility utility in spaceshipData.utilities)
            {
                if (utility != null)
                {
                    Debug.Log("Adding class: " + utility.scriptToAdd.GetClass());
                    ship.AddComponent(utility.scriptToAdd.GetClass());
                }
            }
        }
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

    public virtual void LockMissile(ActorSpaceship shooter, Missile missile)
    {
        //Debug.Log("missile locked");
        lockedMissiles.Add(missile);
    }

    //public virtual void UnlockMissile(ActorSpaceship shooter)
    //{
    //    //Debug.Log("missile unlocked");
    //    //lockedMissiles.Remove(shooter);
    //}

    public virtual void UnlockMissile(Missile missile)
    {
        lockedMissiles.Remove(missile);
    }

    public virtual void FocusShip(ActorSpaceship shooter)
    {
        //Debug.Log("ship focused");
    }

    public virtual void UnfocusShip(ActorSpaceship shooter)
    {
        //Debug.Log("ship unfocused");
    }
}
