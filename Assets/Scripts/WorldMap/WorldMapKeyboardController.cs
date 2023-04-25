using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldMapKeyboardController : MonoBehaviour
{
    public void OnInventoryPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.OnInventoryPressed();
        }
    }

    public void OnOptionsPressed(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.OnOptionsPressed();
        }
    }
}
