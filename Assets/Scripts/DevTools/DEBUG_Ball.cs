using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DevTools
{
    public class DEBUG_Ball : MonoBehaviour
    {
        [SerializeField]
        Transform shipTransform;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            RotateWithObject();
            CheckInput();
        }

        void RotateWithObject()
        {
            Vector3 newRotation = new Vector3(shipTransform.eulerAngles.x, shipTransform.eulerAngles.y, shipTransform.eulerAngles.z);
            transform.eulerAngles = newRotation;
        }

        void CheckInput()
        {
            Vector3 movementVector = Vector3.zero;

            if (Input.GetKey(KeyCode.Keypad4))
            {
                movementVector += Vector3.left;
            }
            if (Input.GetKey(KeyCode.Keypad6))
            {
                movementVector += Vector3.right;
            }
            if (Input.GetKey(KeyCode.Keypad8))
            {
                movementVector += Vector3.forward;
            }
            if (Input.GetKey(KeyCode.Keypad2))
            {
                movementVector += Vector3.back;
            }
            if (Input.GetKey(KeyCode.KeypadPlus))
            {
                movementVector += Vector3.up;
            }
            if (Input.GetKey(KeyCode.KeypadMinus))
            {
                movementVector += Vector3.down;
            }

            transform.Translate(movementVector * Time.deltaTime * 25f);
        }
    }
}

