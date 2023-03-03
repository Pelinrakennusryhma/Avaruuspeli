using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : ActorSpaceship
{
    [SerializeField]
    PlayerInput playerInput;
    protected override void Awake()
    {
        base.Awake();
        playerInput = GetComponent<PlayerInput>();
        GameEvents.Instance.EventPlayerLanded.AddListener(OnLand);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
    }

    void OnLand(MineableAsteroidTrigger asteroid)
    {
        Transform landPos = asteroid.ShipPosition;
        ship.transform.position = landPos.position;
        ship.transform.rotation = landPos.rotation;
        spaceshipMovement.Freeze();
        playerInput.actions.FindActionMap("ShipControls").Disable();
        playerInput.actions.FindActionMap("FirstPersonControls").Enable();
    }

    void OnLeaveAsteroid()
    {
        spaceshipMovement.UnFreeze();
        playerInput.actions.FindActionMap("FirstPersonControls").Disable();
        playerInput.actions.FindActionMap("ShipControls").Enable();

    }
    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }

    public void OnThrust(InputAction.CallbackContext context)
    {
        spaceshipMovement.thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        spaceshipMovement.strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        spaceshipMovement.upDown1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        spaceshipMovement.roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        spaceshipMovement.pitchYaw = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        spaceshipBoost.boosting = context.performed;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        spaceshipShoot.shooting = context.performed;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (Time.timeScale > 0.5f)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
        }
    }

    public void OnLand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameEvents.Instance.CallEventPlayerTriedLanding();
        }
    }
}
