using System;
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
    public bool Protected { get; set; }

    protected List<Missile> lockedMissiles = new List<Missile>();

    protected List<IUseable> shipUtilityScripts = new List<IUseable>();

    virtual protected void Start()
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
        GameEvents.Instance.CallEventSpaceshipSpawned(this);
    }

    protected virtual void InitUtilities()
    {
        if(spaceshipData != null && spaceshipData.utilities != null)
        {
            foreach (ShipUtility utility in spaceshipData.utilities)
            {
                if (utility != null)
                {
                    Type scriptType = utility.scriptToAdd.GetClass();
                    Debug.Log("Adding class: " + scriptType);
                    Component addedScript = ship.AddComponent(scriptType);
                    IUseable useable = (IUseable)addedScript;
                    shipUtilityScripts.Add(useable);
                    useable.Init(utility, this);
                }
            }
        }
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

    public void ClearLockedMissiles()
    {
        foreach (Missile missile in lockedMissiles)
        {
            missile.ClearTarget();
        }

        lockedMissiles.Clear();
    }
}
