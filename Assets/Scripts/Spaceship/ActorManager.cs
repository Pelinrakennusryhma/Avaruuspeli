using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FactionEnum
{
    PLAYER,
    ENEMY,
    FRIENDLY,
}

public class ActorManager : MonoBehaviour 
{
    public static List<ActorSpaceship> actors = new List<ActorSpaceship>();


    private void Awake()
    {
        GameEvents.instance.EventSpaceshipSpawned.AddListener(OnEventSpaceshipSpawned);
        GameEvents.instance.EventSpaceshipDied.AddListener(OnEventSpaceshipDied);
    }

    void OnEventSpaceshipSpawned(ActorSpaceship ship)
    {
        actors.Add(ship);
    }
    void OnEventSpaceshipDied(ActorSpaceship ship)
    {
        actors.Remove(ship);
    }

    public List<ActorSpaceship> GetActors(FactionEnum excludeFaction)
    {
        List<ActorSpaceship> actorsToReturn = new List<ActorSpaceship>();
        foreach (ActorSpaceship actor in actors)
        {
            if (actor.faction != excludeFaction)
            {
                actorsToReturn.Add(actor);
            }
        }

        return actorsToReturn;
    }
}
