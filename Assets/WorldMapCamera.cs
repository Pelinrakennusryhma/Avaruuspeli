using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCamera : MonoBehaviour
{

    private Vector3 offset;

    public void Start()
    {
        offset = MotherShipOnWorldMapController.Instance.gameObject.transform.position - transform.position;
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        //transform.position = MotherShipOnWorldMapController.Instance.gameObject.transform.position - offset;
    }
}
