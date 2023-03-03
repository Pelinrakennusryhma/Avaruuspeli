using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedShip : MonoBehaviour
{
    string promptText = "Press %landKey% to leave the asteroid.";
    bool playerInTriggerArea = false;

    private void Awake()
    {
        GameEvents.Instance.EventPlayerTriedLeaving.AddListener(OnTryLeaving);
    }

    private void OnTryLeaving()
    {
        if (playerInTriggerArea)
        {
            GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
            GameEvents.Instance.CallEventPlayerLeftAstroid();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("enter");
            playerInTriggerArea = true;
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(promptText);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("exit");
            playerInTriggerArea = false;
            GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
        }
    }
}
