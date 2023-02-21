using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum FactionEnum
{
    PLAYER,
    ENEMY,
    FRIENDLY,
}

public class ActorManager : MonoBehaviour 
{
    [SerializeField]
    List<Faction> factions;
    //public static Dictionary<Faction, List<ActorSpaceship>> actors = new Dictionary<Faction, List<ActorSpaceship>>();


    private void Awake()
    {
        GameEvents.instance.EventSpaceshipSpawned.AddListener(OnEventSpaceshipSpawned);
        GameEvents.instance.EventSpaceshipDied.AddListener(OnEventSpaceshipDied);
        InitFactions();  
    }

    void InitFactions()
    {
        foreach (Faction faction in factions)
        {
            //actors.Add(faction, new List<ActorSpaceship>());
            faction.hostileActors.Clear();
        }
    }

    void OnEventSpaceshipSpawned(ActorSpaceship ship)
    {
        foreach (Faction faction in factions)
        {
            if (faction.hostileFactions.Contains(ship.faction))
            {
                Debug.Log("added: " + ship.faction);
                faction.hostileActors.Add(ship);
            }
        }
    }

    void OnEventSpaceshipDied(ActorSpaceship ship)
    {
        foreach (Faction faction in factions)
        {
            if (faction.hostileFactions.Contains(ship.faction))
            {
                faction.hostileActors.Remove(ship);
            }
        }
    }

    //public static List<ActorSpaceship> GetFactionEnemies(Faction faction)
    //{
    //    List<ActorSpaceship> result = new List<ActorSpaceship>();
    //    foreach (Faction enemyFaction in faction.hostileFactions)
    //    {
    //        if (actors.ContainsKey(enemyFaction))
    //        {
    //            result.Concat(actors[enemyFaction]);
    //        }
    //    }

    //    return result;
    //}
}
