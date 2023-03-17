using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModeController : MonoBehaviour
{
    [SerializeField]
    PlayerControls playerControls;
    [SerializeField]
    GameObject firstPersonControls;
    [SerializeField] private FirstPersonPlayerControllerWithCentreOfGravity fpGravity;

    PlayerInput playerInput;
    void Awake()
    {
        GameEvents.Instance.EventPlayerLanded.AddListener(OnLand);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
    }

    private void Start()
    {
        fpGravity = firstPersonControls.GetComponent<FirstPersonPlayerControllerWithCentreOfGravity>();
        playerInput = playerControls.GetComponent<PlayerInput>();
    }

    void OnLand(MineableAsteroidTrigger asteroid)
    {
        Transform landPos = asteroid.shipPosition;
        Transform ship = playerControls.ship.transform;
        asteroid.shipPosition.position = ship.position;
        //playerControls.ship.transform.position = landPos.position;
        //playerControls.ship.transform.rotation = landPos.rotation;
        playerControls.spaceshipMovement.Freeze();
        playerInput.actions.FindActionMap("ShipControls").Disable();

        fpGravity.CenterOfGravity = asteroid.CenterOfGravity;
        Vector3 posOverShip = ship.position + (ship.forward * 5f);
        firstPersonControls.transform.position = posOverShip;
        fpGravity.LookAt(asteroid.transform.position);
        //Debug.Log("rot: " + firstPersonControls.transform.rotation);
        //firstPersonControls.transform.LookAt(asteroid.transform.position, firstPersonControls.transform.up);
        //Debug.Log("rot2: " + firstPersonControls.transform.rotation);
        //firstPersonControls.transform.SetPositionAndRotation(asteroid.CharacterPosition.position, asteroid.CharacterPosition.rotation);
        firstPersonControls.SetActive(true);
        playerInput.actions.FindActionMap("FirstPersonControls").Enable();
    }

    void OnLeaveAsteroid(MineableAsteroidTrigger asteroid)
    {
        fpGravity.CenterOfGravity = null;
        firstPersonControls.SetActive(false);
        playerControls.spaceshipMovement.UnFreeze();
        playerInput.actions.FindActionMap("FirstPersonControls").Disable();
        playerInput.actions.FindActionMap("ShipControls").Enable();
    }
}
