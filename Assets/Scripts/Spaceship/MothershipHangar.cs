using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MothershipHangar : MonoBehaviour
{
    [field: SerializeField]
    public bool PlayerShipInHangar { get; private set; }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            PlayerShipInHangar = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("PlayerShip"))
        {
            PlayerShipInHangar = false;
        }
    }
}
