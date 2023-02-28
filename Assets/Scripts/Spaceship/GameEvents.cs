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
    public UnityEvent<string> EventPlayerEnteredPromptTrigger;
    public UnityEvent EventPlayerExitedPromptTrigger;
    public UnityEvent EventEnemiesKilled;
    public UnityEvent EventPlayerTriedLanding;
    public UnityEvent EventPlayerLanded;
    public UnityEvent EventPlayerLeftAsteroid;
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

    public void CallEventPlayerEnteredPromptTrigger(string promptText)
    {
        EventPlayerEnteredPromptTrigger.Invoke(promptText);
    }

    public void CallEventPlayerExitedPromptTrigger()
    {
        EventPlayerExitedPromptTrigger.Invoke();
    }

    public void CallEventEnemiesKilled()
    {
        Debug.Log("enemies killed!");
        EventEnemiesKilled.Invoke();
    }

    public void CallEventPlayerTriedLanding()
    {
        EventPlayerTriedLanding.Invoke();
    }

    public void CallEventPlayerLanded()
    {
        EventPlayerLanded.Invoke();
    }

    public void CallEventPlayerLeftAstroid()
    {
        EventPlayerLeftAsteroid.Invoke();
    }
}
