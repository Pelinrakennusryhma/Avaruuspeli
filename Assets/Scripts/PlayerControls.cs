using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    public SpaceshipMovement spaceshipMovement;
    public SpaceshipBoost spaceshipBoost;

    void Start()
    {
        spaceshipMovement = GetComponentInChildren<SpaceshipMovement>();
        spaceshipBoost = GetComponentInChildren<SpaceshipBoost>();
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
}
