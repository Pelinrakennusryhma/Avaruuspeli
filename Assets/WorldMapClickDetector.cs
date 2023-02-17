using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapClickDetector : MonoBehaviour
{
    public enum ClickableObjectType
    {
        None = 0,
        Galaxy = 1,
        StarSystem = 2,
        Planet = 3,
        Star = 4
    }

    public ClickableObjectType type;

    public delegate void ClickCallback(ClickableObjectType type);
    public ClickCallback OnObjectClicked;

    public void OnClick()
    {
        Debug.Log("Clicked an object of type " + type);

        WorldMapMouseController.ZoomLevel zoom = WorldMapMouseController.ZoomLevel.None;

        switch (type)
        {
            case ClickableObjectType.None:
                zoom = WorldMapMouseController.ZoomLevel.None;
                break;

            case ClickableObjectType.Galaxy:
                zoom = WorldMapMouseController.ZoomLevel.Galaxy;
                MotherShipOnWorldMapController.Instance.SetPosOnUniverse(transform.position);
                break;

            case ClickableObjectType.StarSystem:
                zoom = WorldMapMouseController.ZoomLevel.StarSystem;
                MotherShipOnWorldMapController.Instance.SetPosOnCurrentGalaxy(transform.position);
                break;

            case ClickableObjectType.Planet:
                zoom = WorldMapMouseController.ZoomLevel.None;
                MotherShipOnWorldMapController.Instance.SetPosOnCurrentStarSystem(transform.position);
                MotherShipOnWorldMapController.Instance.MoveToStarSystemPos();
                break;

            case ClickableObjectType.Star:
                zoom = WorldMapMouseController.ZoomLevel.None;
                MotherShipOnWorldMapController.Instance.SetPosOnCurrentStarSystem(transform.position);
                MotherShipOnWorldMapController.Instance.MoveToStarSystemPos();
                break;

            default:
                break;
        }



        WorldMapMouseController.Instance.ZoomIn(transform.position, 
                                                zoom,
                                                WorldMapMouseController.Instance.CurrentGalaxy,
                                                WorldMapMouseController.Instance.CurrentStarSystem);

        if (OnObjectClicked != null)
        {
            OnObjectClicked(type);
        }
    }
}
