using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AsteroidLauncher;

public class MineableAsteroidTrigger : MonoBehaviour
{
    string successText = "Press %landKey% to land on the asteroid";
    string failureText = "Clear the area of hostiles before landing on the asteroid.";
    string currentText;
    bool playerInTriggerArea = false;

    private void Awake()
    {
        currentText = failureText;
        GameEvents.instance.EventEnemiesKilled.AddListener(OnEnemiesKilled);
        GameEvents.instance.EventPlayerTriedLanding.AddListener(OnLandingAttempt);
    }

    void OnLandingAttempt()
    {
        if (playerInTriggerArea)
        {
            LaunchAsteroidScene(AsteroidType.Asteroid01a, ResourceInventory.ResourceType.Iron, MineralDensity.Medium, true);
        }
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
        if (other.CompareTag("Player"))
        {
            playerInTriggerArea = true;
            GameEvents.instance.CallEventPlayerEnteredPromptTrigger(currentText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTriggerArea = false;
            GameEvents.instance.CallEventPlayerExitedPromptTrigger();
        }
    }
}
