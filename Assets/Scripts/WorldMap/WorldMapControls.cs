using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WorldMapControls : MonoBehaviour
{
    public void OnOptions(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnOptionsPressed();
            }
        }
    }
}
