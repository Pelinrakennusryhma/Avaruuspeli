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

    virtual protected void Awake()
    {
        ship = transform.GetChild(0).gameObject;
        spaceshipMovement = ship.GetComponent<SpaceshipMovement>();
        spaceshipBoost = ship.GetComponent<SpaceshipBoost>();
        spaceshipShoot = ship.GetComponent<SpaceshipShoot>();
        spaceshipEvents = ship.GetComponent<SpaceshipEvents>();

        spaceshipEvents.EventSpaceshipDied.AddListener(OnDeath);
    }

    virtual protected void Start()
    {
        GameEvents.instance.CallEventSpaceshipSpawned(this);
    }

    virtual protected void OnDeath()
    {
        Debug.Log("spaceship died: " + transform.name + " faction: " + faction.factionName);
        GameEvents.instance.CallEventSpaceshipDied(this);
    }
}
