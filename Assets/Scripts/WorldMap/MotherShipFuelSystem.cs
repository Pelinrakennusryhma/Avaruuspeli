using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MotherShipFuelSystem : MonoBehaviour
{
    public const string WarpdriveFuelText = "WARPDRIVE FUEL: ";
    public const string RocketFuelText = "ROCKET FUEL: ";

    public TextMeshProUGUI WarpdriveFuelPlaceHolderText;
    public TextMeshProUGUI RocketFuelPlaceHolderText;

    public GameObject WarpdriveFuelBackPlate;
    public GameObject RocketFuelBackPlate;

    public TextMeshProUGUI NeededFuelPlaceHolderText;
    public GameObject NeededFuelUIBackPlate;

    private float WarpdriveFuel = 100000.0f;
    private float RocketFuel = 100000.0f;

    private float universeConsumptionRate = 10.0f;
    private float galaxyConsumptionRate = 500.0f;

    public MotherShipOnWorldMapController MotherShipController;

    public int AmountOfWarpdriveFuelTanks;
    public int AmountOfRocketFuelTanks;

    public float AmountOfWarpdriveFuelInLastTank;
    public float AmountOfRocketFuelInLastTank;

    // Update is called once per frame

    public Vector3 previousUniversePos;
    public Vector3 previousGalaxyPos;

    public WorldMapMouseController.ZoomLevel previousKnownZoomLevel;


    private RaycastHit[] RaycastHits;

    public bool HasTeleportedLastFrame;

    private void Awake()
    {
        AmountOfWarpdriveFuelInLastTank = 1.0f;
        AmountOfRocketFuelInLastTank = 1.0f;
        WarpdriveFuelPlaceHolderText.text = "WARPDRIVE FUEL: " + ((int)WarpdriveFuel).ToString();
        RocketFuelPlaceHolderText.text = "ROCKET FUEL: " + ((int)RocketFuel).ToString();

        if (GameManager.Instance != null) 
        {
            GameManager.Instance.OnEnterWorldMap -= UpdateAllFuelAmounts;
            GameManager.Instance.OnEnterWorldMap += UpdateAllFuelAmounts;
        }


    }

    private void Start()
    {
        if (GameManager.LaunchType == GameManager.TypeOfLaunch.LoadedGame)
        {
            AmountOfWarpdriveFuelInLastTank = GameManager.Instance.SaverLoader.LoadAmountOfWarpDriveFuelInLastTank();
            AmountOfRocketFuelInLastTank = GameManager.Instance.SaverLoader.LoadAmountOfRocketFuelInLastTank();

            if (AmountOfRocketFuelInLastTank < 0)
            {
                AmountOfRocketFuelInLastTank = 0;
            }

            if (AmountOfWarpdriveFuelInLastTank < 0)
            {
                AmountOfWarpdriveFuelInLastTank = 0;
            }
        }        
        
        UpdateAllFuelAmounts();
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

            WarpdriveFuel = (AmountOfWarpdriveFuelTanks - 1 + AmountOfWarpdriveFuelInLastTank) * 1000;

            //WarpdriveFuel -= distanceTravelled * universeConsumptionRate;
            AmountOfWarpdriveFuelInLastTank -= (distanceTravelled * universeConsumptionRate) / 1000;

            if (AmountOfWarpdriveFuelInLastTank <= 0)
            {
                if (AmountOfWarpdriveFuelTanks > 1) 
                {
                    AmountOfWarpdriveFuelTanks--;
                    GameManager.Instance.InventoryController.Inventory.RemoveItem(14, 1);

                    AmountOfWarpdriveFuelInLastTank = 1.0f + AmountOfWarpdriveFuelInLastTank;

                    if (AmountOfWarpdriveFuelInLastTank < 0.0f)
                    {
                        Debug.LogError("We went through two tanks in one update. Not cool. Fix this. Amount of warpdrive fuel in last tank is " + AmountOfWarpdriveFuelInLastTank);
                    }
                }

                else
                {
                    AmountOfWarpdriveFuelInLastTank = 0.1f;

                    if (AmountOfWarpdriveFuelTanks > 0) 
                    {
                        GameManager.Instance.InventoryController.Inventory.RemoveItem(14, 999999);
                    }

                    AmountOfWarpdriveFuelTanks = 0;


                }
            }

            int amountOfTanks = AmountOfWarpdriveFuelTanks;
            //Debug.Log("Amount of tanks is " + amountOfTanks + " universe fuel is " + WarpdriveFuel + " amount of warpdrive fuel in last tank is " + AmountOfWarpdriveFuelInLastTank);

            // Divide and conquer?

            //if (AmountOfWarpdriveFuelInLastTank >= 0)
            //{
            //    AmountOfWarpdriveFuelTanks = amountOfFullTanks + 1; 
            //}

            //else
            //{
            //    AmountOfWarpdriveFuelTanks = 0;
            //}


            if (WarpdriveFuel <= 0)
            {
                WarpdriveFuel = 0.1f;
            }

            WarpdriveFuelPlaceHolderText.text = WarpdriveFuelText + ((int)WarpdriveFuel).ToString();

            //Debug.Log("Universe fuel is " + UniverseFuel);

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

            //RocketFuel = ((AmountOfRocketFuelTanks - 1) + AmountOfRocketFuelInLastTank) * 1000;
            //RocketFuel -= distanceTravelled * galaxyConsumptionRate;

            RocketFuel = (AmountOfRocketFuelTanks - 1 + AmountOfRocketFuelInLastTank) * 1000;

            //WarpdriveFuel -= distanceTravelled * universeConsumptionRate;
            AmountOfRocketFuelInLastTank -= (distanceTravelled * galaxyConsumptionRate) / 1000;

            if (AmountOfRocketFuelInLastTank <= 0)
            {
                if (AmountOfRocketFuelTanks > 1)
                {
                    AmountOfRocketFuelTanks--;
                    GameManager.Instance.InventoryController.Inventory.RemoveItem(15, 1);

                    AmountOfRocketFuelInLastTank = 1.0f + AmountOfRocketFuelInLastTank;

                    if (AmountOfRocketFuelInLastTank < 0.0f)
                    {
                        Debug.LogError("We went through two tanks in one update. Not cool. Fix this");
                    }
                }

                else
                {
                    AmountOfRocketFuelInLastTank = 0.1f;
                    
                    if (AmountOfRocketFuelTanks > 0) 
                    {
                        GameManager.Instance.InventoryController.Inventory.RemoveItem(15, 999999);
                    }
                    AmountOfRocketFuelTanks = 0;
 

                }
            }

            int amountOfTanks = AmountOfRocketFuelTanks;
            //Debug.Log("Amount of tanks is " + amountOfTanks + " rocket fuel is " + RocketFuel + " amount of rocket fuel in last tank is " + AmountOfRocketFuelInLastTank);



            //Debug.Log("Galaxy fuel is " + RocketFuel);

            if (RocketFuel <= 0)
            {
                RocketFuel = 0.1f;
            }

            RocketFuelPlaceHolderText.text = RocketFuelText + ((int)RocketFuel).ToString();

            previousGalaxyPos = transform.position;
            previousKnownZoomLevel = WorldMapMouseController.ZoomLevel.Galaxy;
        }

        if (RocketFuel <= 0.0f)
        {
            RocketFuel = 0.1f;

            //UniverseFuelPlaceHolderText.text = "RUNNING ON FUMES";
        }

        if (WarpdriveFuel <= 0.0f)
        {
            WarpdriveFuel = 0.1f;
            //GalaxyFuelPlaceHolderText.text = "RUNNING ON FUMES";
        }

        previousKnownZoomLevel = WorldMapMouseController.Instance.CurrentZoomLevel;
        //Debug.Log("Distance travelled during frame is " + distanceTravelled);
    }

    public void SetPreviousKnownZoomLevel(WorldMapMouseController.ZoomLevel zoom)
    {
        previousKnownZoomLevel = zoom;
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

            if (WarpdriveFuel - neededFuelIs >= 0.0f
                || distanceToTravel <= 1.0f)
            {
                weHaveEnoughFuel = true;
            }
        }

        else if (previousKnownZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
        {
            //Debug.LogWarning("We evaluated galaxy fuel");
            distanceToTravel = (yZeroedPos - targetPos).magnitude;
            neededFuelIs = distanceToTravel * galaxyConsumptionRate;

            if (RocketFuel - neededFuelIs >= 0.0f
                || distanceToTravel <= 0.1f)
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

        if (GameManager.Instance == null)
        {
            return;
        }

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
        RocketFuel += amount;
    }

    public void AddUniverseFuel(float amount)
    {
        WarpdriveFuel += amount;
    }

    public void OnTeleportStarted()
    {
        HasTeleportedLastFrame = true;
        previousGalaxyPos = transform.position;
        previousUniversePos = transform.position;
    }

    public void UpdateWarpdriveFuelTankAmount(int newAmount)
    {
        if (AmountOfWarpdriveFuelTanks <= 0
            && newAmount > 0)
        {
            AmountOfWarpdriveFuelInLastTank = 1.0f;
        }

        AmountOfWarpdriveFuelTanks = newAmount;

        if (newAmount <= 0)
        {
            AmountOfWarpdriveFuelInLastTank = 0;
        }

        WarpdriveFuel = ((AmountOfWarpdriveFuelTanks - 1) + AmountOfWarpdriveFuelInLastTank) * 1000.0f;

        if (WarpdriveFuel <= 0.0f)
        {
            WarpdriveFuel = 0.1f;
        }

        //Debug.Log("Amount of warpdrive fuel is " + WarpdriveFuel + " amount of fuel in last tank is " + AmountOfWarpdriveFuelInLastTank);
        WarpdriveFuelPlaceHolderText.text = "WARPDRIVE FUEL: " + ((int)WarpdriveFuel).ToString();
    }

    public void UpdateRocketFuelTankAmount(int newAmount)
    {
        if (AmountOfRocketFuelTanks <= 0
            && newAmount > 0)
        {
            AmountOfRocketFuelInLastTank = 1.0f;
        }

        AmountOfRocketFuelTanks = newAmount;

        if(newAmount <= 0)
        {
            AmountOfRocketFuelInLastTank = 0;
        }

        RocketFuel = ((AmountOfRocketFuelTanks - 1) + AmountOfRocketFuelInLastTank) * 1000.0f;

        if (RocketFuel <= 0.0f)
        {
            RocketFuel = 0.1f;
        }

        //Debug.Log("Amount of rocket fuel is " + RocketFuel + " amount of fuel in last tank is " + AmountOfRocketFuelInLastTank);

        RocketFuelPlaceHolderText.text = "ROCKET FUEL: " + ((int)RocketFuel).ToString();
    }

    public void UpdateAllFuelAmounts()
    {
        int rocketAmount = 0;
        int warpAmount = 0;

        ItemScript rockets = GameManager.Instance.InventoryController.Inventory.GetItemScript(15);
        ItemScript warps = GameManager.Instance.InventoryController.Inventory.GetItemScript(14);

        if (rockets != null)
        {
            rocketAmount = rockets.currentItemAmount;
        }

        if (warps != null)
        {
            warpAmount = warps.currentItemAmount;
        }

        UpdateWarpdriveFuelTankAmount(warpAmount);
        UpdateRocketFuelTankAmount(rocketAmount);

        //Debug.Log("Update fuels. We entered world map.");
    }

    public void OnZoom(WorldMapMouseController.ZoomLevel zoom)
    {
        switch (zoom)
        {
            case WorldMapMouseController.ZoomLevel.None:
                break;
            case WorldMapMouseController.ZoomLevel.Universe:

                WarpdriveFuelPlaceHolderText.gameObject.SetActive(true);
                RocketFuelPlaceHolderText.gameObject.SetActive(false);

                WarpdriveFuelBackPlate.gameObject.SetActive(true);
                RocketFuelBackPlate.gameObject.SetActive(false);
                break;

            case WorldMapMouseController.ZoomLevel.Galaxy:

                WarpdriveFuelPlaceHolderText.gameObject.SetActive(false);
                RocketFuelPlaceHolderText.gameObject.SetActive(true);

                WarpdriveFuelBackPlate.gameObject.SetActive(false);
                RocketFuelBackPlate.gameObject.SetActive(true);
                break;

            case WorldMapMouseController.ZoomLevel.StarSystem:

                WarpdriveFuelPlaceHolderText.gameObject.SetActive(false);
                RocketFuelPlaceHolderText.gameObject.SetActive(false);

                WarpdriveFuelBackPlate.gameObject.SetActive(false);
                RocketFuelBackPlate.gameObject.SetActive(false);
                break;

            default:
                break;
        }
    }

    public void SaveData()
    {
        //GameManager.Instance.SaverLoader.SaveAmountOfRocketFuelInLastTank(AmountOfRocketFuelInLastTank);
        //GameManager.Instance.SaverLoader.SaveAmountOfWarpDriveFuelInLastTank(AmountOfWarpdriveFuelInLastTank);

        //// We have to save inventory too, so we keep track of consumed fuel
        //GameManager.Instance.InventoryController.SaveInventory();

        GameManager.Instance.SaverLoader.SaveFuelDataAndInventory(AmountOfRocketFuelInLastTank,
                                                                  AmountOfWarpdriveFuelInLastTank,
                                                                  GameManager.Instance.InventoryController.Inventory.InventoryItemScripts);
    }
}
