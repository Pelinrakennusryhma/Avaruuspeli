using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneralSaveData
{
    // Save inventory, but when and how

    [System.Serializable]
    public class InventoryItem
    {
        public int ID;
        public int Amount;
    }

    public List<InventoryItem> InventoryItems;


    // Universe positions and zoom level

    public Vector3 CurrentUniversePos;
    public Vector3 CurrentGalaxyPos;
    public Vector3 CurrentStarSystemPos;
    public WorldMapMouseController.ZoomLevel ZoomLevel = WorldMapMouseController.ZoomLevel.None;

    // Universe, galaxy and star system IDs

    public int CurrentGalaxyID;
    public int CurrentStarSystemID;
    public int CurrentPlanetID;
    public int CurrentAsteroidFieldID;
    public int CurrentPOIID; // But these are generated on the fly, so we can't really use them. Maybe just return to position on star system?

    // Save equipped items
    public int EquippedItemInHands;
    public int EquippedSpaceSuit;
    public int[] EquippedShipItems;

    // Hydroponics Bay
    public bool HydroponicsBayIsRunning;
    public bool HydroponicsBayIsProducingOxygen;
    public float AmountOfCarbonInLastUnit;
    public float AmountOfWaterInLastBottle;

    public float TimeInMinutesHydroponicsBayHasBeenOutOfOxygenProduction;

    // Fuel system
    public float AmountOfRocketFuelInLastTank;
    public float AmountOfWarpDriveFuelInLastTank;

    // Life support systems
    public float AmountOfOxygenInLastBottle;
    public float AmountOfOxygenInLastStorage;

    // Money
    public float AmountOfMoney;

    public GeneralSaveData(bool justABool)
    {
        InventoryItems = new List<InventoryItem>();
        CurrentUniversePos = Vector3.zero;
        CurrentGalaxyPos = Vector3.zero;
        CurrentStarSystemPos = Vector3.zero;
        ZoomLevel = WorldMapMouseController.ZoomLevel.None;
        CurrentGalaxyID = -1;
        CurrentStarSystemID = -1;
        CurrentPlanetID = -1;
        CurrentAsteroidFieldID = -1;
        CurrentPOIID = -1;

        EquippedItemInHands = -1;
        EquippedSpaceSuit = -1;
        EquippedShipItems = new int[7];
        HydroponicsBayIsRunning = false;
        HydroponicsBayIsProducingOxygen = false;
        AmountOfCarbonInLastUnit = -1;
        AmountOfWaterInLastBottle = -1;
        TimeInMinutesHydroponicsBayHasBeenOutOfOxygenProduction = -1;
        AmountOfRocketFuelInLastTank = -1;
        AmountOfWarpDriveFuelInLastTank = -1;
        AmountOfOxygenInLastBottle = -1;
        AmountOfOxygenInLastStorage = -1;  
        AmountOfMoney = -1;

        //Debug.Log("Constructor called");
    }
}
