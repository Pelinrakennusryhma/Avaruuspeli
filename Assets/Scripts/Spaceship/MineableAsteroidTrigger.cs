using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AsteroidLauncher;

public class MineableAsteroidTrigger : MonoBehaviour
{
    [SerializeField]
    GameObject firstPersonControls;
    [SerializeField]
    GameObject shipOnAsteroid;
    [SerializeField]
    GameObject shipControls;
    [SerializeField]
    ActorManager actorManager;
    string successText = "Press %landKey% to land on the asteroid";
    string failureText = "Clear the area of hostiles before landing on the asteroid.";
    string currentText;
    bool playerInTriggerArea = false;

    private void Awake()
    {
        GameEvents.instance.EventEnemiesKilled.AddListener(OnEnemiesKilled);
        GameEvents.instance.EventPlayerTriedLanding.AddListener(OnLandingAttempt);
        GameEvents.instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
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
        GameEvents.instance.CallEventPlayerLanded();
        shipControls.SetActive(false);
        firstPersonControls.SetActive(true);
        shipOnAsteroid.SetActive(true);
    }

    void OnLeaveAsteroid()
    {
        shipOnAsteroid.SetActive(false);
    }

    void OnEnemiesKilled()
    {
        currentText = successText;
        if (playerInTriggerArea)
        {
            GameEvents.instance.CallEventPlayerEnteredPromptTrigger(currentText);
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
            GameEvents.instance.CallEventPlayerEnteredPromptTrigger(currentText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            playerInTriggerArea = false;
            GameEvents.instance.CallEventPlayerExitedPromptTrigger();
        }
    }
}
