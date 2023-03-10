using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        } else
        {
            Instance = this;
        }
    }

    public UnityEvent<ActorSpaceship> EventSpaceshipDied;
    public UnityEvent<ActorSpaceship> EventSpaceshipSpawned;
    public UnityEvent EventPlayerSpaceshipDied;
    public UnityEvent<string> EventPlayerEnteredPromptTrigger;
    public UnityEvent EventPlayerExitedPromptTrigger;
    public UnityEvent EventEnemiesKilled;
    public UnityEvent EventPlayerTriedLanding;
    public UnityEvent<MineableAsteroidTrigger> EventPlayerLanded;
    public UnityEvent EventPlayerTriedLeaving;
    public UnityEvent<MineableAsteroidTrigger> EventPlayerLeftAsteroid;
    public UnityEvent<bool> EventToggleIndicators;
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

    public void CallEventPlayerLanded(MineableAsteroidTrigger asteroid)
    {
        EventPlayerLanded.Invoke(asteroid);
    }
    
    public void CallEventPlayerTriedLeaving()
    {
        EventPlayerTriedLeaving.Invoke();
    }

    public void CallEventPlayerLeftAstroid(MineableAsteroidTrigger asteroid)
    {
        EventPlayerLeftAsteroid.Invoke(asteroid);
    }

    public void CallEventToggleIndicators(bool showIndicators)
    {
        EventToggleIndicators.Invoke(showIndicators);
    }
}
