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
    [SerializeField] private FirstPersonPlayerControllerWithCentreOfGravity fpsGravity;

    PlayerInput playerInput;
    void Awake()
    {
        GameEvents.Instance.EventPlayerLanded.AddListener(OnLand);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
    }

    private void Start()
    {
        fpsGravity = firstPersonControls.GetComponent<FirstPersonPlayerControllerWithCentreOfGravity>();
        playerInput = playerControls.GetComponent<PlayerInput>();
    }

    void OnLand(MineableAsteroidTrigger asteroid)
    {
        Transform landPos = asteroid.ShipPosition;
        playerControls.ship.transform.position = landPos.position;
        playerControls.ship.transform.rotation = landPos.rotation;
        playerControls.spaceshipMovement.Freeze();
        playerInput.actions.FindActionMap("ShipControls").Disable();

        fpsGravity.CenterOfGravity = asteroid.CenterOfGravity;
        firstPersonControls.transform.SetPositionAndRotation(asteroid.CharacterPosition.position, asteroid.CharacterPosition.rotation);
        firstPersonControls.SetActive(true);
        playerInput.actions.FindActionMap("FirstPersonControls").Enable();
    }

    void OnLeaveAsteroid(MineableAsteroidTrigger asteroid)
    {
        fpsGravity.CenterOfGravity = null;
        firstPersonControls.gameObject.SetActive(false);
        playerControls.spaceshipMovement.UnFreeze();
        playerInput.actions.FindActionMap("FirstPersonControls").Disable();
        playerInput.actions.FindActionMap("ShipControls").Enable();
    }
}
