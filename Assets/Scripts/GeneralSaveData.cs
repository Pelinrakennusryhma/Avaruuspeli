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

    // Save equipped items
    public int EquippedItemInHands;
    public int EquippedSpaceSuit;

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
 
}
