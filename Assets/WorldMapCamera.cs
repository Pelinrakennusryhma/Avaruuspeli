using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapCamera : MonoBehaviour
{

    private Vector3 offset;

    public static WorldMapCamera Instance;

    public void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        offset = MotherShipOnWorldMapController.Instance.gameObject.transform.position - transform.position;
    }

    // Update is called once per frame
    public void LateUpdate()
    {
        //transform.position = MotherShipOnWorldMapController.Instance.gameObject.transform.position - offset;
    }

    public void SetToUniverseOffset(Vector3 originPos)
    {
        Vector3 originPos2D = new Vector3(originPos.x, 0, originPos.z);
        transform.position = originPos2D + new Vector3(0, transform.position.y, -333.0f);
    }

    public void SetToGalaxyOffset(Vector3 originPos)
    {
        Vector3 originPos2D = new Vector3(originPos.x, 0, originPos.z);
        transform.position = originPos2D + new Vector3(0, transform.position.y, -6.66f);
    }

    public void SetToStarSystemOffset(Vector3 originPos)
    {
        Vector3 originPos2D = new Vector3(originPos.x, 0, originPos.z);
        transform.position = originPos2D + new Vector3(0, transform.position.y, -1.5f);
    }
}
