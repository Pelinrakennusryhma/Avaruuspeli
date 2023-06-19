using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandedShip : MonoBehaviour
{
    [SerializeField]
    MineableAsteroidTrigger asteroid;
    string promptText = "Press %landKey% to leave the asteroid.";
    bool playerInTriggerArea = false;
    bool playerFacingShip = false;
    FirstPersonPlayerControllerWithCentreOfGravity fpControls;

    private void Awake()
    {
        GameEvents.Instance.EventPlayerTriedLeaving.AddListener(OnTryLeaving);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeave);
        GameEvents.Instance.EventPlayerRanOutOfOxygen.AddListener(OnRanOutOfOxygen);
        GameEvents.Instance.EventPlayerWasTooHungryToContinue.AddListener(PlayerWasTooHungryToContinue);
        //Debug.Log("Added a listener to player ran out of oxygen");
        //Debug.Log("Listener added to on leftasteroid" + Time.time + " gameobject is " + gameObject.name);
    }

    private void OnTryLeaving()
    {
        if (playerInTriggerArea && playerFacingShip)
        {
            GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
            GameEvents.Instance.CallEventPlayerLeftAstroid(asteroid);
        }
    }

    private void OnLeave(MineableAsteroidTrigger asteroidLeft)
    {
        if(asteroidLeft == asteroid)
        {
            playerInTriggerArea = false;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(fpControls == null)
            {
                // THIS GIVES NULL REFS. Probably the hierarchy has changed or something
                //fpControls = other.transform.parent.parent.GetComponent<FirstPersonPlayerControllerWithCentreOfGravity>();

                fpControls = FindObjectOfType<FirstPersonPlayerControllerWithCentreOfGravity>();
            }

            Debug.Log("enter");
            playerInTriggerArea = true;         
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

    private void Update()
    {
        if (playerInTriggerArea)
        {
            // this should never happen but just in case hierarchy changes or something
            if(fpControls == null)
            {
                fpControls = FindObjectOfType<FirstPersonPlayerControllerWithCentreOfGravity>();
            }
            Vector3 fpShipDir = (transform.position - fpControls.Camera.transform.position).normalized;
            float dotProduct = Vector3.Dot(fpShipDir, fpControls.Camera.transform.forward);

            if(dotProduct > 0.85f)
            {
                playerFacingShip = true;
                GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(promptText);
            } else
            {
                playerFacingShip = false;
                GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
            }
        }
    }

    private void OnRanOutOfOxygen()
    {
        GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
        GameEvents.Instance.CallEventPlayerLeftAstroid(asteroid);
    }

    private void PlayerWasTooHungryToContinue()
    {
        GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
        GameEvents.Instance.CallEventPlayerLeftAstroid(asteroid);

        Debug.Log("We should now leave the asteroid neatly and cleanly");
    }

}
