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
        Star = 4,
        AsteroidField = 5,
        Wormhole = 6
    }

    public ClickableObjectType type;

    public delegate void ClickCallback(ClickableObjectType type);
    public ClickCallback OnObjectClicked;

    public void OnClick()
    {
        //Debug.Log("Clicked an object of type " + type);

        Vector3 mousePos = new Vector3(0, -10000, 0);

        if (GameManager.Instance.Helpers.CheckIfUIisHit())
        {
            //Debug.Log("we clicked, but hit UI at the same time. return");
            return;
        }

        if (MotherShipOnWorldMapController.Instance.IsOnCurrentClickableObject
            && MotherShipOnWorldMapController.Instance.CurrentTargetClickableObject
            == this) 
        {
            //Debug.Log("Clickity");

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

                case ClickableObjectType.Wormhole:
                    
                    //Debug.LogError("Clicked a wormhole");
                    break;

                case ClickableObjectType.Planet:
                    zoom = WorldMapMouseController.ZoomLevel.None;
                    MotherShipOnWorldMapController.Instance.SetPosOnCurrentStarSystem(transform.position);
                    //MotherShipOnWorldMapController.Instance.MoveToStarSystemPos();

                    if (MotherShipOnWorldMapController.Instance.CheckIfPlanetOrStarPositionIsWithinTolerance()) 
                    {
                        GameManager.Instance.EnterPlanet();
                        //Debug.LogError("CLICKED PLANET");
                    }

                    break;

                case ClickableObjectType.Star:
                    zoom = WorldMapMouseController.ZoomLevel.None;
                    MotherShipOnWorldMapController.Instance.SetPosOnCurrentStarSystem(transform.position);
                    //MotherShipOnWorldMapController.Instance.MoveToStarSystemPos();




                    break;

                case ClickableObjectType.AsteroidField:
                    zoom = WorldMapMouseController.ZoomLevel.None;
                    MotherShipOnWorldMapController.Instance.SetPosOnCurrentStarSystem(transform.position);
                    MotherShipOnWorldMapController.Instance.SetPosOnCurrentStarSystem(MotherShipOnWorldMapController.Instance.CurrentAsteroidFieldPos);
                    //MotherShipOnWorldMapController.Instance.MoveToStarSystemPos();

                    if (MotherShipOnWorldMapController.Instance.CheckIfAsteroidFieldPositionIsWithinTolerance()) 
                    {
                        GameManager.Instance.EnterAsteroidField();
                        //Debug.LogError("CLICKED ASTEROID FIELD");
                    }
                        //Debug.Log("WE SHOULD MOVE TO ASTEROID FIELD POS");
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

        else
        {
            if (mousePos.y >= -1000.0f)
            {
                Debug.Log("REACT TO MOUSE BEING");
            }

            bool hasEnoughFuelForTravel = MotherShipOnWorldMapController.Instance.FuelSystem.EvaluateNeededFuel(new Vector3(transform.position.x, 
                                                                                                                0, 
                                                                                                                transform.position.z));

            if (hasEnoughFuelForTravel) 
            {
                MotherShipOnWorldMapController.Instance.SetCurrentTargetClickableObject(this);
            }

            else
            {
                Debug.LogError("Not enough fuel. Don't travel to destination.");
            }
        }
    }
}