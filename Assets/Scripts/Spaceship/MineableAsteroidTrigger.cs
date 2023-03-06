using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AsteroidLauncher;

public class MineableAsteroidTrigger : MonoBehaviour
{
    [field: SerializeField]
    public Transform ShipPosition { get; private set; }
    [field: SerializeField]
    public Transform CharacterPosition { get; private set; }
    [SerializeField]
    ActorManager actorManager;
    string successText = "Press %landKey% to land on the asteroid";
    string failureText = "Clear the area of hostiles before landing on the asteroid.";
    string currentText;
    bool playerInTriggerArea = false;

    private void Awake()
    {
        GameEvents.Instance.EventEnemiesKilled.AddListener(OnEnemiesKilled);
        GameEvents.Instance.EventPlayerTriedLanding.AddListener(OnLandingAttempt);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
    }

    void OnLandingAttempt()
    {
        if (playerInTriggerArea && actorManager.SceneCleared)
        {
            //LaunchAsteroidScene(AsteroidType.Asteroid01a, ResourceInventory.ResourceType.Iron, MineralDensity.Medium, true);
            Land();
        }
    }

    void Land()
    {
        GameEvents.Instance.CallEventPlayerLanded(this);
    }

    void OnLeaveAsteroid()
    {
        GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
    }

    void OnEnemiesKilled()
    {
        currentText = successText;
        if (playerInTriggerArea)
        {
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            playerInTriggerArea = true;
            if (actorManager.SceneCleared)
            {
                currentText = successText;
            } else
            {
                currentText = failureText;
            }
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(currentText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            playerInTriggerArea = false;
            GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
        }
    }
}
