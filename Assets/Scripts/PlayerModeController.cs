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
    PlayerInput playerInput;
    void Awake()
    {
        GameEvents.Instance.EventPlayerLanded.AddListener(OnLand);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
    }

    private void Start()
    {
        playerInput = playerControls.GetComponent<PlayerInput>();
    }

    void OnLand(MineableAsteroidTrigger asteroid)
    {
        Transform landPos = asteroid.ShipPosition;
        playerControls.ship.transform.position = landPos.position;
        playerControls.ship.transform.rotation = landPos.rotation;
        playerControls.spaceshipMovement.Freeze();
        playerInput.actions.FindActionMap("ShipControls").Disable();

        firstPersonControls.transform.position = asteroid.CharacterPosition.position;
        firstPersonControls.transform.rotation = asteroid.CharacterPosition.rotation;
        firstPersonControls.gameObject.SetActive(true);
        playerInput.actions.FindActionMap("FirstPersonControls").Enable();
    }

    void OnLeaveAsteroid()
    {
        firstPersonControls.gameObject.SetActive(false);
        playerControls.spaceshipMovement.UnFreeze();
        playerInput.actions.FindActionMap("FirstPersonControls").Disable();
        playerInput.actions.FindActionMap("ShipControls").Enable();
    }
}
