using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public UnityEvent<ActorSpaceship> EventSpaceshipDied;
    public UnityEvent<ActorSpaceship> EventSpaceshipSpawned;
    public UnityEvent EventPlayerSpaceshipDied;
    //public void CallEventPlayerSpaceshipDied()
    //{
    //    EventPlayerSpaceshipDied.Invoke();
    //}

    public void CallEventSpaceshipSpawned(ActorSpaceship ship)
    {
        EventSpaceshipSpawned.Invoke(ship);
    }
    public void CallEventSpaceshipDied(ActorSpaceship ship)
    {
        EventSpaceshipDied.Invoke(ship);
        if(ship is PlayerControls)
        {
            EventPlayerSpaceshipDied.Invoke();
        }
    }
}
