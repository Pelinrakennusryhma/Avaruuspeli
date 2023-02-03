using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonPlayerControls : MonoBehaviour
{
    public FirstPersonPlayerControllerWithCentreOfGravity PlayerController;

    public float Horizontal;
    public float Vertical;
    public float MouseDeltaX;
    public float MouseDeltaY;

    public bool JumpDownPressed;
    public bool CrouchDown;
    public bool RunDown;
    public bool Fire1Down;
    public bool OptionsDown;

    private PlayerInput playerInput;

    public void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        //playerInput.SwitchCurrentActionMap("FirstPersonControls");
        //Debug.Log("Current action map is " + playerInput.currentActionMap.ToString());
    }

    public void OnHorizontal(InputAction.CallbackContext value)
    {
        Horizontal = value.ReadValue<float>();
        //Debug.Log("Pressed horizontal " + Horizontal);
    }

    public void OnVertical(InputAction.CallbackContext value)
    {
        Vertical = value.ReadValue<float>();
        //Debug.Log("Pressed vertical " + Vertical);
    }

    public void OnMouseMove(InputAction.CallbackContext value)
    {
        MouseDeltaX = value.ReadValue<Vector2>().x;
        MouseDeltaY = value.ReadValue<Vector2>().y;
        //MouseDeltaX = delta.x;
        //MouseDeltaY = delta.y;
    }

    public void OnJumpPressed(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            JumpDownPressed = true;
        }

        else
        {
            JumpDownPressed = false;
        }


        //Debug.Log("Pressed JUMP");
    }

    public void OnCrouchDown(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            CrouchDown = true;
        }

        else
        {
            CrouchDown = false;
        }
        //Debug.Log("Pressed CROUCH");
    }

    public void OnRunDown(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            RunDown = true;
        }

        else
        {
            RunDown = false;
        }

        //Debug.Log("Pressed RUN");
    }

    public void OnFire1Pressed(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            Fire1Down = true;
        }

        else
        {
            Fire1Down = false;
        }
        //Debug.Log("On fire 1 pressed");
    }

    public void OnOptions(InputAction.CallbackContext value)
    {
        float valueFloat = value.ReadValue<float>();

        if (valueFloat > 0)
        {
            OptionsDown = true;
        }

        else
        {
            OptionsDown = false;
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnPausePressed();
        }

        //Debug.Log("On options pressed");
    }

    public void LateUpdate()
    {
        ClearControls();
    }

    public void ClearControls()
    {
        //RunDown = false;
        //Fire1Down = false;
        OptionsDown = false;
        //CrouchDown = false;
        JumpDownPressed = false;
    }
}
