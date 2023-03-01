using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MotherShipFuelSystem : MonoBehaviour
{
    public TextMeshProUGUI UniverseFuelPlaceHolderText;
    public TextMeshProUGUI GalaxyFuelPlaceHolderText;

    public float UniverseFuel = 100000.0f;
    public float GalaxyFuel = 100000.0f;

    public MotherShipOnWorldMapController MotherShipController;

    // Update is called once per frame

    public Vector3 previousUniversePos;
    public Vector3 previousGalaxyPos;

    public WorldMapMouseController.ZoomLevel previousKnownZoomLevel;

    private void Awake()
    {
        UniverseFuelPlaceHolderText.text = "UNIVERSE FUEL: " + ((int)UniverseFuel).ToString();
        GalaxyFuelPlaceHolderText.text = "GALAXY FUEL: " + ((int)GalaxyFuel).ToString();
    }

    public void LateUpdate()
    {
        //EvaluateNeededFuel(Vector3.zero);

        float distanceTravelled = 0;

        if (WorldMapMouseController.Instance.CurrentZoomLevel 
            != previousKnownZoomLevel)
        {
            if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.Universe)
            {
                previousUniversePos = transform.position;
            }

            else if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
            {
                previousGalaxyPos = transform.position;
            }
        }

        if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.Universe)
        {
            distanceTravelled = (transform.position - previousUniversePos).magnitude;

            if (distanceTravelled <= 0.2f)
            {
                distanceTravelled = 0;
            }

            UniverseFuel -= distanceTravelled * 10.0f;
            UniverseFuelPlaceHolderText.text = "UNIVERSE FUEL " + ((int)UniverseFuel).ToString();

            previousUniversePos = transform.position;
            previousKnownZoomLevel = WorldMapMouseController.ZoomLevel.Universe;
        }

        else if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
        {
            distanceTravelled = (transform.position - previousGalaxyPos).magnitude;

            if (distanceTravelled <= 0.01f)
            {
                distanceTravelled = 0;
            }

            
            GalaxyFuel -= distanceTravelled * 100.0f;
            GalaxyFuelPlaceHolderText.text = "GALAXY FUEL " + ((int)GalaxyFuel).ToString();

            previousGalaxyPos = transform.position;
            previousKnownZoomLevel = WorldMapMouseController.ZoomLevel.Galaxy;
        }

        //Debug.Log("Distance travelled during frame is " + distanceTravelled);
    }

    public void EvaluateNeededFuel(Vector3 targetPos)
    {
        float neededFuelIs = 0;
        Vector3 yZeroedPos = new Vector3(transform.position.x,
                                         0,
                                         transform.position.z);
        float distanceToTravel = 0;
        bool weHaveEnoughFuel = false;

        if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.Universe)
        {
            distanceToTravel = (yZeroedPos - targetPos).magnitude;
            neededFuelIs = distanceToTravel * 10.0f;

            if (UniverseFuel - neededFuelIs >= 0.0f)
            {
                weHaveEnoughFuel = true;
            }
        }

        else if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
        {
            distanceToTravel = (yZeroedPos - targetPos).magnitude;
            neededFuelIs = distanceToTravel * 10.0f;

            if (GalaxyFuel - neededFuelIs >= 0.0f)
            {
                weHaveEnoughFuel = true;
            }
        }

        Debug.LogWarning("we are at a point where we should evaluate needed fuel to target position. Fuel needed for travel is " + neededFuelIs + " we have enough fuel is " + weHaveEnoughFuel);
    }

    public void DoRaycastingToDetectTargetObjectsAndNeededFuel()
    {
        // We probably need to detect distances with raycasts from camera
    }

    public void AddGalaxyFuel(float amount)
    {
        GalaxyFuel += amount;
    }

    public void AddUniverseFuel(float amount)
    {
        UniverseFuel += amount;
    }
}
