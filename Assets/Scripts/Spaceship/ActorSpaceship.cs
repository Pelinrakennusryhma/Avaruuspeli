using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public SpaceshipHealth spaceshipHealth;
    public bool InDanger { get { return lockedMissiles.Count > 0; } }
    public List<Useable> ActiveUtils { get; private set; }

    protected List<Missile> lockedMissiles = new List<Missile>();

    protected List<Useable> shipUtilityScripts = new List<Useable>();

    virtual protected void Start()
    {
        ship = InitShipModel();
        spaceshipMovement = ship.GetComponent<SpaceshipMovement>();
        spaceshipBoost = ship.GetComponent<SpaceshipBoost>();
        spaceshipShoot = ship.GetComponent<SpaceshipShoot>();
        spaceshipEvents = ship.GetComponent<SpaceshipEvents>();
        spaceshipMissile = ship.GetComponent<SpaceshipMissile>();
        targetProjection = ship.GetComponent<TargetProjection>();
        spaceshipHealth = ship.GetComponent<SpaceshipHealth>();

        spaceshipEvents.EventSpaceshipDied.AddListener(OnDeath);
        ActiveUtils = new List<Useable>();
        InitShip();
        GameEvents.Instance.CallEventSpaceshipSpawned(this);
    }

    private GameObject InitShipModel() 
    {
        GameObject shipModel;
        if(spaceshipData != null && spaceshipData.shipModel != null)
        {
            // delete current ship if it exists
            if(transform.childCount > 0)
            {
                Destroy(transform.GetChild(0).gameObject);            
            }

            shipModel = Instantiate(spaceshipData.shipModel.itemPrefab, transform);

        } else
        {
            shipModel = transform.GetChild(0).gameObject;
        }

        return shipModel;
    }

    protected virtual void InitShip()
    {
        if(spaceshipData != null)
        {
            InitShipHealth();
            InitShipWeapons();
            InitUtilities();
        }
    }

    void InitShipHealth()
    {
        if(spaceshipHealth != null && spaceshipData.hull != null)
        {
            spaceshipHealth.InitHull(spaceshipData.hull);
        } else
        {
            throw new Exception("Unable to initialize ship health.");
        }
    }

    void InitShipWeapons()
    {
        if(spaceshipData.primaryWeapon != null && spaceshipShoot != null)
        {
            if(spaceshipData.primaryWeapon is ShipLaser)
            {
                ShipLaser shipLaser = (ShipLaser)spaceshipData.primaryWeapon;
                float damage = shipLaser.damage;
                float velocity = shipLaser.velocity;
                float rateOfFire = shipLaser.rateOfFire;
                spaceshipShoot.Init(damage, velocity, rateOfFire);
            }
        }

        if(spaceshipData.secondaryWeapon != null)
        {
            if(spaceshipData.secondaryWeapon is ShipMissileBattery && spaceshipMissile != null)
            {
                ShipMissileBattery missileBattery = (ShipMissileBattery)spaceshipData.secondaryWeapon;
                float damage = missileBattery.explosionDamage;
                float speed = missileBattery.missileSpeed;
                float cooldown = missileBattery.cooldown;
                float radius = missileBattery.explosionRadius;
                int amount = missileBattery.missileCapacity;
                spaceshipMissile.Init(damage, radius, speed, cooldown, amount);
            }
        }
    }

    protected virtual void InitUtilities()
    {
        if(spaceshipData.utilities != null)
        {
            foreach (ShipUtility utility in spaceshipData.utilities)
            {
                if (utility != null)
                {
                    Type scriptType = Type.GetType(utility.scriptToAdd);
                    //Debug.Log("Adding class: " + scriptType);
                    Component addedScript = ship.AddComponent(scriptType);
                    Useable useable = (Useable)addedScript;
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
        List<Missile> deepCopy = new List<Missile>();
        foreach (Missile missile in lockedMissiles)
        {
            deepCopy.Add(missile);
            missile.ClearTarget();
        }

        foreach (Missile missile in deepCopy)
        {
            UnlockMissile(missile);
        }
    }

    public void ActivateUtil(Useable util)
    {
        Debug.Log("Activating: " + util.Data.itemName);
        ActiveUtils.Add(util);
    }

    public void DeactivateUtil(Useable util)
    {
        ActiveUtils.Remove(util);
    }
}
