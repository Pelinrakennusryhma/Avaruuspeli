using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMissile : MonoBehaviour
{
    ActorSpaceship actor;
    public bool shooting = false;

    void Start()
    {
        actor = GetComponentInParent<ActorSpaceship>();
        
    }

    // Update is called once per frame
    void Update()
    {
        ScanForTarget();
    }

    void ScanForTarget()
    {
        Debug.Log(actor.faction.hostileActors.Count);
        foreach (ActorSpaceship hostileActor in actor.faction.hostileActors)
        {

        }

    }
}
