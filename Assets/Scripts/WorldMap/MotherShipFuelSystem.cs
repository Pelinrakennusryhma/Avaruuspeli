using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MotherShipFuelSystem : MonoBehaviour
{
    public TextMeshProUGUI UniverseFuelPlaceHolderText;
    public TextMeshProUGUI GalaxyFuelPlaceHolderText;

    public TextMeshProUGUI NeededFuelPlaceHolderText;
    public GameObject NeededFuelUIBackPlate;

    private float UniverseFuel = 100000.0f;
    private float GalaxyFuel = 100000.0f;

    private float universeConsumptionRate = 10.0f;
    private float galaxyConsumptionRate = 500.0f;

    public MotherShipOnWorldMapController MotherShipController;

    // Update is called once per frame

    public Vector3 previousUniversePos;
    public Vector3 previousGalaxyPos;

    public WorldMapMouseController.ZoomLevel previousKnownZoomLevel;


    private RaycastHit[] RaycastHits;

    public bool HasTeleportedLastFrame;

    private void Awake()
    {
        UniverseFuelPlaceHolderText.text = "UNIVERSE FUEL: " + ((int)UniverseFuel).ToString();
        GalaxyFuelPlaceHolderText.text = "GALAXY FUEL: " + ((int)GalaxyFuel).ToString();
    }

    public void LateUpdate()
    {
        //EvaluateNeededFuel(Vector3.zero);

        if (HasTeleportedLastFrame)
        {
            HasTeleportedLastFrame = false;
            return;
        }

        DoRaycastingToDetectTargetObjectsAndNeededFuel();

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

            UniverseFuel -= distanceTravelled * universeConsumptionRate;
            UniverseFuelPlaceHolderText.text = "UNIVERSE FUEL: " + ((int)UniverseFuel).ToString();

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

            
            GalaxyFuel -= distanceTravelled * galaxyConsumptionRate;
            GalaxyFuelPlaceHolderText.text = "GALAXY FUEL: " + ((int)GalaxyFuel).ToString();

            previousGalaxyPos = transform.position;
            previousKnownZoomLevel = WorldMapMouseController.ZoomLevel.Galaxy;
        }

        if (GalaxyFuel <= 0.0f)
        {
            GalaxyFuel = 0.1f;

            //UniverseFuelPlaceHolderText.text = "RUNNING ON FUMES";
        }

        if (UniverseFuel <= 0.0f)
        {
            UniverseFuel = 0.1f;
            //GalaxyFuelPlaceHolderText.text = "RUNNING ON FUMES";
        }

        previousKnownZoomLevel = WorldMapMouseController.Instance.CurrentZoomLevel;
        //Debug.Log("Distance travelled during frame is " + distanceTravelled);
    }

    public bool EvaluateNeededFuel(Vector3 targetPos)
    {
        if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.None
            || previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.StarSystem)
        {
            return true;
        }

        float neededFuelIs = 0;
        Vector3 yZeroedPos = new Vector3(transform.position.x,
                                         0,
                                         transform.position.z);
        float distanceToTravel = 0;
        bool weHaveEnoughFuel = false;

        if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.Universe)
        {
            distanceToTravel = (yZeroedPos - targetPos).magnitude;
            neededFuelIs = distanceToTravel * universeConsumptionRate;

            if (UniverseFuel - neededFuelIs >= 0.0f)
            {
                weHaveEnoughFuel = true;
            }
        }

        else if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
        {
            //Debug.LogWarning("We evaluated galaxy fuel");
            distanceToTravel = (yZeroedPos - targetPos).magnitude;
            neededFuelIs = distanceToTravel * galaxyConsumptionRate;

            if (GalaxyFuel - neededFuelIs >= 0.0f)
            {
                weHaveEnoughFuel = true;
            }
        }

        //Debug.LogWarning("we are at a point where we should evaluate needed fuel to target position. Fuel needed for travel is " + neededFuelIs + " we have enough fuel is " + weHaveEnoughFuel);
        return weHaveEnoughFuel;
    }

    public void DoRaycastingToDetectTargetObjectsAndNeededFuel()
    {
        // We probably need to detect distances with raycasts from camera


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);

        //RaycastHits = Physics.RaycastAll(ray, 
        //                                 10000.0f);

        RaycastHit hitInfo;

        bool hit = Physics.Raycast(ray,
                                   out hitInfo,
                                   10000.0f);

        bool hitUI = GameManager.Instance.Helpers.CheckIfUIisHit();

        float distance = 0;

        if (hit)
        {
            Vector3 yZeroedPos = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 yZeroedTargetPos = hitInfo.collider.gameObject.transform.position;
            yZeroedTargetPos = new Vector3(yZeroedTargetPos.x, 0, yZeroedTargetPos.z);
            distance = (yZeroedTargetPos - yZeroedPos).magnitude;
        }

        float neededFuel = 0;

        if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.Universe)
        {
            neededFuel = distance * universeConsumptionRate;
        }

        else if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
        {
            neededFuel = distance * galaxyConsumptionRate;
        }

        if (!hitUI
            && hit
            && neededFuel >= 1.0f)
        {
            NeededFuelPlaceHolderText.gameObject.SetActive(true);
            NeededFuelUIBackPlate.gameObject.SetActive(true);

            NeededFuelPlaceHolderText.text = "FUEL NEEDED TO TARGET: " + ((int)neededFuel).ToString();
           //Debug.Log("we hit with raycast");
        }

        else
        {
            NeededFuelPlaceHolderText.gameObject.SetActive(false);
            NeededFuelUIBackPlate.gameObject.SetActive(false);
            //Debug.Log("No hit with raycast");
        }
    }

    public void AddGalaxyFuel(float amount)
    {
        GalaxyFuel += amount;
    }

    public void AddUniverseFuel(float amount)
    {
        UniverseFuel += amount;
    }

    public void OnTeleportStarted()
    {
        HasTeleportedLastFrame = true;
        previousGalaxyPos = transform.position;
        previousUniversePos = transform.position;
    }
}
