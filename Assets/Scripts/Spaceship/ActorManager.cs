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

                // shuffle the target list for randomness
                for (int i = 0; i < faction.hostileActors.Count; i++)
                {
                    ActorSpaceship temp = faction.hostileActors[i];
                    int randomIndex = Random.Range(i, faction.hostileActors.Count);
                    faction.hostileActors[i] = faction.hostileActors[randomIndex];
                    faction.hostileActors[randomIndex] = temp;
                }
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
}
