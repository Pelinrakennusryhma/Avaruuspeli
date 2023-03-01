using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipWorldMapTrigger : MonoBehaviour
{
    public MotherShipOnWorldMapController controller;

    public void OnTriggerEnter(Collider other)
    {
        controller.OnTriggered(other);
    }
}
