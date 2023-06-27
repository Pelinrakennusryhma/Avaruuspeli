using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaverLoader : MonoBehaviour
{
    public UniverseSaveData UniverseSaveData;
    public VendorSaveData VendorSaveData;
    public GeneralSaveData GeneralSaveData;


    public const string UniverseSaveDataPath = "/UniverseData.json";
    public const string VendorSaveDataPath = "/VendorData.json";
    public const string GeneralSaveDataPath = "/GeneralSaveData.json";


    public void OnInitialStartUp()
    {
        if (GameManager.LaunchType == GameManager.TypeOfLaunch.LoadedGame) 
        {
            bool successfulRead = ReadFromGeneralSaveDataFile();

            if (successfulRead)
            {
                //Debug.LogWarning("We fetched general save data successfully");
            }

            else
            {
                //Debug.LogError("We failed to read general save data");
            }

            //Debug.LogWarning("Saver loader initial startup is called. Loading initial data about general saved things. Universe and vendors are not loaded yet at this point.");
        }

        else
        {        
            GeneralSaveData = new GeneralSaveData(true);

           // Debug.Log("Created a new general save data object. equipped item in hands is " + GeneralSaveData.EquippedItemInHands);
        }

    }

    public bool LoadUniverse()
    {
        bool success = ReadFromUniverseFile();

        ReadFromVendorFile();
        //Debug.Log("Trying to LOAD universe");

        return success;
    }

    public List<GalaxyData> GetSavedGalaxyDatas()
    {
        return UniverseSaveData.Galaxies;
    }

    public void SaveUniverse(List<GalaxyData> galaxies)
    {
        UniverseSaveData.Galaxies = galaxies;


        WriteToUniverseFile();
        //Debug.Log("Trying to SAVE universe");
    }

    public bool LoadGalaxy(int galaxyId,
                           out GalaxyData data)
    {
        bool success = false;
        data = null;
        //Debug.Log("Trying to LOAD galaxy");

        for (int i = 0; i < UniverseSaveData.Galaxies.Count; i++)
        {
            if (UniverseSaveData.Galaxies[i].ID == galaxyId)
            {
                data = UniverseSaveData.Galaxies[i];
                success = true;
                break;
            }
        }

        return success;
    }

    public void SaveGalaxy(int galaxyId,
                           List<StarSystemData> starSystems)
    {

        bool found = false;

        for (int i = 0; i < UniverseSaveData.Galaxies.Count; i++)
        {
            if (UniverseSaveData.Galaxies[i].ID == galaxyId)
            {
                found = true;
                UniverseSaveData.Galaxies[i].StarSystems = starSystems;
                break;
            }
        }

        if (found) 
        {
            //Debug.Log("Found a galaxy");
        }

        else
        {
            //Debug.Log("Didn't find a galaxy");
        }
        //Debug.Log("Trying to SAVE galaxy");

        WriteToUniverseFile();
    }

    public bool LoadStarSystem(int galaxyId,
                               int starSystemId,
                               out StarSystemData data)
    {
        bool success = false;
        data = null;

        //Debug.Log("Trying to LOAD star system");

        for (int i = 0; i < UniverseSaveData.Galaxies.Count; i++)
        {
            if (UniverseSaveData.Galaxies[i].ID == galaxyId)
            {
                for (int j = 0; j < UniverseSaveData.Galaxies[i].StarSystems.Count; j++)
                {
                    if (UniverseSaveData.Galaxies[i].StarSystems[j].ID == starSystemId)
                    {
                        data = UniverseSaveData.Galaxies[i].StarSystems[j];
                        success = true;
                        //Debug.LogWarning("We found the correct star system in iteration");
                        break;
                    }
                }

                //Debug.LogWarning("WE found the correct galaxy in iteration");
                break;
            }
        }

        return success;
    }

    public void SaveStarSystem(int galaxyId,
                               int starsystemId,
                               CenterStarData centerStar,
                               List<PlanetData> planets,
                               List<AsteroidFieldData> asteroidFields,
                               Vector3 pos,
                               Quaternion rot,
                               Vector3 localScale,
                               StarSystemData data)
    {

        //Debug.LogWarning("Caliing save star system with star system id " + starsystemId + " planets lenght is " + planets.Count);
        bool found = false;

        for (int i = 0; i < UniverseSaveData.Galaxies.Count; i++)
        {
            if (UniverseSaveData.Galaxies[i].ID == galaxyId)
            {
               // Debug.Log("Iterating through the found galaxy");

                int foundJ = -1;

                for (int j = 0; j < UniverseSaveData.Galaxies[i].StarSystems.Count; j++)
                {
                    //Debug.Log("Iterating star system. Now at id " + SaveData.Galaxies[i].StarSystems[j].ID + " searching for " + starsystemId + " star systems lenght is " + SaveData.Galaxies[i].StarSystems.Count);

                    if (UniverseSaveData.Galaxies[i].StarSystems[j].ID == starsystemId)
                    {
                        found = true;
                        foundJ = j;

                        if (found)
                        {
                            //Debug.LogError("Found the j with star system id " + starsystemId);

                        }
                        //break;
                    }
                }


                StarSystemData starSystemData = new StarSystemData();                
                
                if (data != null) 
                {
                    starSystemData = data;
                }

                else
                {
                    starSystemData.ID = starsystemId;
                    starSystemData.CenterStar = centerStar;
                    starSystemData.Planets = planets;
                    starSystemData.Asteroids = asteroidFields;
                    starSystemData.SetPosRotAndScale(pos, rot, localScale);
                }




                if (found
                    && foundJ >= 0)
                {                    
                    UniverseSaveData.Galaxies[i].StarSystems[foundJ] = starSystemData;
                    //Debug.LogError("Putting star system to previously found pos");

                }

                else
                {
                    UniverseSaveData.Galaxies[i].StarSystems.Add(starSystemData);
                    //Debug.LogError("Putting star system to new pos");
                }
            }

            if (found)
            {
                break;
            }
        }

        if (found)
        {
            //Debug.Log("Found a star system");
        }

        else
        {
            //Debug.Log("Didn't find a star system");
        }

        WriteToUniverseFile();

        //Debug.Log("Trying to SAVE star system");
    }

    public void SavePlanet(int galaxyId,
                           int starSystemId,
                           PlanetData data)
    {
        for (int i = 0; i < UniverseSaveData.Galaxies.Count; i++)
        {
            if (UniverseSaveData.Galaxies[i].ID == galaxyId)
            {
                for (int j = 0; j < UniverseSaveData.Galaxies[i].StarSystems.Count; j++)
                {
                    if (UniverseSaveData.Galaxies[i].StarSystems[j].ID == starSystemId)
                    {
                        bool foundAPlanetToReplace = false;
                        int foundK = -1;

                        for (int k = 0; k < UniverseSaveData.Galaxies[i].StarSystems[j].Planets.Count; k++)
                        {
                            if (UniverseSaveData.Galaxies[i].StarSystems[j].Planets[k].ID == data.ID)
                            {
                                foundAPlanetToReplace = true;
                                foundK = k;
                                //Debug.Log("Found a planet to replace");
                                break;
                            }
                        }

                        if (foundAPlanetToReplace
                            && foundK >= 0)
                        {
                            UniverseSaveData.Galaxies[i].StarSystems[j].Planets[foundK] = data;
                        }

                        else
                        {
                            UniverseSaveData.Galaxies[i].StarSystems[j].Planets.Add(data);
                        }

                        break;
                    }
                }
                break;
            }
        }
    }

    // This maybe is called too much, if every time a star system is generated we write to file????
    public void WriteToUniverseFile()
    {
        string universeDataString = JsonUtility.ToJson(UniverseSaveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + UniverseSaveDataPath, universeDataString);
        //Debug.LogWarning("WRITING TO SAVE FILE");
    }

    public bool ReadFromUniverseFile()
    {
        //Debug.LogWarning("READING FROM SAVE FILE");
        bool succesfullRead = false;

        string saveFile = Application.persistentDataPath + UniverseSaveDataPath;

        if (System.IO.File.Exists(saveFile))
        {
            //Debug.Log("WE have a save file that exists");

            string savedUniverse = System.IO.File.ReadAllText(saveFile);
            UniverseSaveData = JsonUtility.FromJson<UniverseSaveData>(savedUniverse);

            if (UniverseSaveData != null)
            {
                succesfullRead = true;
                //Debug.Log("We had a valid save data object");
            }

            else
            {
                succesfullRead = false;
                UniverseSaveData = new UniverseSaveData();
            }
        }

        return succesfullRead;
    }

    public void WriteToVendorSaveFile()
    {
        string vendorDataString = JsonUtility.ToJson(VendorSaveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + VendorSaveDataPath, vendorDataString);
        //Debug.LogWarning("Writing to vendor save data file");
    }

    public bool ReadFromVendorFile()
    {
        //Debug.LogWarning("READING FROM vendor SAVE FILE");
        bool succesfullRead = false;

        string saveFile = Application.persistentDataPath + VendorSaveDataPath;

        if (System.IO.File.Exists(saveFile))
        {
            //Debug.Log("WE have a vendor save file that exists");

            string savedVendors = System.IO.File.ReadAllText(saveFile);
            VendorSaveData = JsonUtility.FromJson<VendorSaveData>(savedVendors);

            if (VendorSaveData != null)
            {
                succesfullRead = true;
                //Debug.Log("We had a valid VENDOR save data object");
            }

            else
            {
                succesfullRead = false;
                VendorSaveData = new VendorSaveData();
            }
        }

        return succesfullRead;
    }

    public Vendor GetVendor(int galaxyId,
                            int starSystemId,
                            int planetId)
    {
        Vendor vendor = null;

        if (VendorSaveData == null)
        {
            VendorSaveData = new VendorSaveData();
            Debug.LogError("Null vendor save data");
        }

        if (VendorSaveData.Vendors == null)
        {
            VendorSaveData.Vendors = new List<Vendor>();
            Debug.LogError("Null vendors list");
        }

        for (int i = 0; i < VendorSaveData.Vendors.Count; i++)
        {
            if (VendorSaveData.Vendors[i].GalaxyID == galaxyId
                && VendorSaveData.Vendors[i].StarSystemID == starSystemId
                && VendorSaveData.Vendors[i].PlanetID == planetId)
            {
                vendor = VendorSaveData.Vendors[i];
                //Debug.Log("Found vendor from galaxy " + galaxyId + " star system " + starSystemId + " from planet " + planetId);
                break;
            }
        }

        return vendor;
    }

    public void SaveVendor(Vendor vendor)
    {
        if (VendorSaveData == null)
        {
            ReadFromVendorFile();
            Debug.LogError("Had to read from vendor file");
        }

        if (VendorSaveData == null)
        {
            VendorSaveData = new VendorSaveData();
            Debug.LogError("Vendor save data is null");
        }

        if (VendorSaveData.Vendors == null)
        {
            VendorSaveData.Vendors = new List<Vendor>();
            Debug.LogError("A null list of vendors");
        }

        //Debug.LogWarning("Saving vendor with galaxy id " + vendor.GalaxyID 
        //                 + " star system id " + vendor.StarSystemID + " at planed id " + vendor.PlanetID);

        bool alreadyInList = false;

        // God, I wish there was better solution for this
        // But don't know yet, how to implement this otherwise and save to json
        for (int i = 0; i < VendorSaveData.Vendors.Count; i++)
        {
            if (VendorSaveData.Vendors[i].GalaxyID == vendor.GalaxyID
                && VendorSaveData.Vendors[i].StarSystemID == vendor.StarSystemID
                && VendorSaveData.Vendors[i].PlanetID == vendor.PlanetID) 
            {
                alreadyInList = true;
                VendorSaveData.Vendors[i] = vendor;
                break;
            }
        }

        if (!alreadyInList) 
        {
            VendorSaveData.Vendors.Add(vendor);
        }

        //for (int i = 0; i < VendorSaveData.Vendors.Count; i++)
        //{
        //    if (VendorSaveData.Vendors[i].Items.Count > 0) 
        //    {
        //        for (int j = 0; j < VendorSaveData.Vendors[i].Items.Count; j++)
        //        {
        //            Debug.Log("Vendor " + VendorSaveData.Vendors[i].GalaxyID + " "
        //                      + VendorSaveData.Vendors[i].StarSystemID
        //                      + " " + VendorSaveData.Vendors[i].PlanetID + "  has item "
        //                      + VendorSaveData.Vendors[i].Items[j].ItemId + " of amount "
        //                      + VendorSaveData.Vendors[i].Items[j].ItemAmount);
        //        }
        //    }

        //    else
        //    {
        //        Debug.Log("Vendor " + VendorSaveData.Vendors[i].GalaxyID + " "
        //                      + VendorSaveData.Vendors[i].StarSystemID
        //                      + " " + VendorSaveData.Vendors[i].PlanetID + " has zero items");
        //    }
        //}

        WriteToVendorSaveFile();
        //Debug.LogWarning("Save vendor");
    }

    public void WriteToGeneralSaveDataFile()
    {
        string generalSaveDataString = JsonUtility.ToJson(GeneralSaveData);
        System.IO.File.WriteAllText(Application.persistentDataPath + GeneralSaveDataPath, generalSaveDataString);
        //Debug.LogWarning("Writing to general save data file");
    }

    public bool ReadFromGeneralSaveDataFile()
    {
        //Debug.LogWarning("READING FROM general SAVE FILE");
        bool succesfullRead = false;

        string saveFile = Application.persistentDataPath + GeneralSaveDataPath;

        if (System.IO.File.Exists(saveFile))
        {
            //Debug.Log("WE have a general save file that exists");

            string savedGeneralData = System.IO.File.ReadAllText(saveFile);
            GeneralSaveData = JsonUtility.FromJson<GeneralSaveData>(savedGeneralData);

            if (GeneralSaveData != null)
            {
                succesfullRead = true;
                //Debug.Log("We had a valid GENERAL save data object");
            }

            else
            {
                succesfullRead = false;
                GeneralSaveData = new GeneralSaveData(true);
            }
        }

        return succesfullRead;
    }

    // This won't work on WebGL. So, we shouldn't rely on it if we decide a browser version will be supported.
    //public void OnApplicationQuit()
    //{
    //    Debug.Log("Quitting, so we start fetching things to save");
    //    WriteToGeneralSaveDataFile();
    //}

    #region MiscellaneousSaving


    public void SaveCurrentUniversePos(Vector3 pos)
    {
        GeneralSaveData.CurrentUniversePos = pos;
        WriteToGeneralSaveDataFile();
        //Debug.LogError("Saving universe pos. x " + pos.x + " z " + pos.z);
    }


    public void SaveCurrentGalaxyPos(Vector3 pos)
    {
        GeneralSaveData.CurrentGalaxyPos = pos;
        WriteToGeneralSaveDataFile();
        //Debug.LogError("Saving galaxy pos. x " + pos.x + " z " + pos.z);
    }


    public void SaveCurrentStarSystemPos(Vector3 pos)
    {
        GeneralSaveData.CurrentStarSystemPos = pos;
        WriteToGeneralSaveDataFile();
        //Debug.LogError("Saving star system pos. x " + pos.x + " z " + pos.z);
    }

    public void SaveGalaxyID(int id)
    {
        GeneralSaveData.CurrentGalaxyID = id;
        WriteToGeneralSaveDataFile();
        //Debug.LogWarning("Save galaxy id " + id);
    }

    public void SaveStarSystemID(int id)
    {
        GeneralSaveData.CurrentStarSystemID = id;
        WriteToGeneralSaveDataFile();
        //Debug.LogWarning("Save star system id " + id);
    }


    public void SaveWorldMapZoomLevel(WorldMapMouseController.ZoomLevel zoomLevel)
    {
        GeneralSaveData.ZoomLevel = zoomLevel;
        WriteToGeneralSaveDataFile();
    }

    // Save equipped items

    public void SaveEquippedItemInHands(int itemID)
    {
        GeneralSaveData.EquippedItemInHands = itemID;
        WriteToGeneralSaveDataFile();
    }


    public void SaveEquippedSpaceSuit(int itemID)
    {
        GeneralSaveData.EquippedSpaceSuit = itemID;
        Debug.Log("Saving equipped spacesuit. Item id is " + itemID);
        WriteToGeneralSaveDataFile();
    }

    public void SaveEquippedShipItem(int itemID, int slot)
    {
        GeneralSaveData.EquippedShipItems[slot] = itemID;
        //Debug.Log($"Saving ship item. ID: {itemID}, SLOT: {slot}");
        WriteToGeneralSaveDataFile();
    }

    // Hydroponics Bay

    public void SaveHydroponicsBayIsRunningStatus(bool isRunning)
    {
        GeneralSaveData.HydroponicsBayIsRunning = isRunning;
        WriteToGeneralSaveDataFile();
    }


    public void SaveHydroponicsBayIsProducingOxygenStatus(bool isProducingOxygen)
    {
        GeneralSaveData.HydroponicsBayIsProducingOxygen = isProducingOxygen;
        WriteToGeneralSaveDataFile();
    }


    public void SaveAmountOfCarbonInLastUnit(float amountOfCarbonInLastUnit)
    {
        GeneralSaveData.AmountOfCarbonInLastUnit = amountOfCarbonInLastUnit;
        WriteToGeneralSaveDataFile();
    }


    public void SaveAmountOfWaterInLastBottle(float amountOfWaterInLastBottle)
    {
        GeneralSaveData.AmountOfWaterInLastBottle = amountOfWaterInLastBottle;
        WriteToGeneralSaveDataFile();
    }


    public void SaveTimeInMinutesForHydroponicsBay(float time)
    {
        GeneralSaveData.TimeInMinutesHydroponicsBayHasBeenOutOfOxygenProduction = time;
        WriteToGeneralSaveDataFile();
    }

    // Fuel system

    public void SaveAmountOfRocketFuelInLastTank(float amountOfFuel)
    {
        GeneralSaveData.AmountOfRocketFuelInLastTank = amountOfFuel;
        WriteToGeneralSaveDataFile();
    }


    public void SaveAmountOfWarpDriveFuelInLastTank(float amountOfFuel)
    {
        GeneralSaveData.AmountOfWarpDriveFuelInLastTank = amountOfFuel;
        WriteToGeneralSaveDataFile();
    }

    // Life support systems

    public void SaveAmountOfOxygenInLastBottle(float amount)
    {
        GeneralSaveData.AmountOfOxygenInLastBottle = amount;
        WriteToGeneralSaveDataFile();
    }

  
    public void SaveAmountOfOxygenInLastStorage(float amount)
    {
        GeneralSaveData.AmountOfOxygenInLastStorage = amount;
        WriteToGeneralSaveDataFile();
        Debug.Log("Saved amount of oxygen in last storage " + amount);
    }


    public void SaveMoney(float money)
    {
        GeneralSaveData.AmountOfMoney = money;
        WriteToGeneralSaveDataFile();
    }


    // Inventory
    public void SaveInventory(List<ItemScript> inventoryItems)
    {
        // Just overwrite anything already existing
        GeneralSaveData.InventoryItems = new List<GeneralSaveData.InventoryItem>();

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            GeneralSaveData.InventoryItem toAdd = new GeneralSaveData.InventoryItem();
            toAdd.ID = inventoryItems[i].itemToAdd.id;
            toAdd.Amount = inventoryItems[i].currentItemAmount;
            GeneralSaveData.InventoryItems.Add(toAdd);
        }

        WriteToGeneralSaveDataFile();
    }

    // Combo saving. We probably want to call write to file only when necessary? Not at every saved item?
    public void SaveFuelDataAndInventory(float rocketAmount,
                                         float warpAmount,
                                         List<ItemScript> inventoryItems)
    {
        GeneralSaveData.AmountOfRocketFuelInLastTank = rocketAmount;
        GeneralSaveData.AmountOfWarpDriveFuelInLastTank = warpAmount;
        SaveInventory(inventoryItems); // Note write to file is called at the end of this method!
    }

    public void SaveLifeSupportData(float amountOfOxygenInLastStorage,
                                    float timeThatHydroponicsHasBeenOutOfResources,
                                    float amountOfCarbonInLastUnit,
                                    float amountOfWaterInLastBottle)
    {
        GeneralSaveData.AmountOfOxygenInLastStorage = amountOfOxygenInLastStorage;
        GeneralSaveData.AmountOfCarbonInLastUnit = amountOfCarbonInLastUnit;
        GeneralSaveData.AmountOfWaterInLastBottle = amountOfWaterInLastBottle;
        GeneralSaveData.TimeInMinutesHydroponicsBayHasBeenOutOfOxygenProduction = timeThatHydroponicsHasBeenOutOfResources;
        WriteToGeneralSaveDataFile();
    }

    #endregion

    #region MiscellaneousLoading

    public Vector3 LoadCurrentUniversePos()
    {
        return GeneralSaveData.CurrentUniversePos;
    }

    public Vector3 LoadCurrentGalaxyPos()
    {
        return GeneralSaveData.CurrentGalaxyPos;
    }

    public Vector3 LoadCurrentStarSystemPos()
    {
        return GeneralSaveData.CurrentStarSystemPos;
    }

    public WorldMapMouseController.ZoomLevel LoadWorldMapZoomLevel()
    {
        return GeneralSaveData.ZoomLevel;
    }

    public int LoadGalaxyID()
    {
        return GeneralSaveData.CurrentGalaxyID;
    }

    public int LoadStarSystemID()
    {
        return GeneralSaveData.CurrentStarSystemID;

    }

    // Load equipped items
    //DONE
    public int LoadEquippedItemInHands()
    {
        return GeneralSaveData.EquippedItemInHands;
    }

    // DONE
    public int LoadEquippedSpaceSuit()
    {
        return GeneralSaveData.EquippedSpaceSuit;
    }

    public int[] LoadEquippedShipItems()
    {
        return GeneralSaveData.EquippedShipItems;
    }

    // Hydroponics Bay
    // DONE
    public bool LoadHydroponicsBayIsRunningStatus()
    {
        return GeneralSaveData.HydroponicsBayIsRunning;
    }

    // DONE
    public bool LoadHydroponicsBayIsProducingOxygenStatus()
    {
        return GeneralSaveData.HydroponicsBayIsProducingOxygen;
    }

    // Ship life support system
    // Not done, buggy
    public float LoadAmountOfCarbonInLastUnit()
    {
        return GeneralSaveData.AmountOfCarbonInLastUnit;
    }

    // Not done, buggy
    public float LoadAmountOfWaterInLastBottle()
    {
        return GeneralSaveData.AmountOfWaterInLastBottle;
    }

    // DONE
    public float LoadTimeInMinutesForHydroponicsBay()
    {
        return GeneralSaveData.TimeInMinutesHydroponicsBayHasBeenOutOfOxygenProduction;
    }

    // Fuel system
    // DONE
    public float LoadAmountOfRocketFuelInLastTank()
    {
        return GeneralSaveData.AmountOfRocketFuelInLastTank;
    }

    // DONE
    public float LoadAmountOfWarpDriveFuelInLastTank()
    {
        return GeneralSaveData.AmountOfWarpDriveFuelInLastTank;
    }

    // Life support systems
    // DONE
    public float LoadAmountOfOxygenInLastBottle()
    {
        return GeneralSaveData.AmountOfOxygenInLastBottle;
    }

    // DONE
    public float LoadAmountOfOxygenInLastStorage()
    {
        return GeneralSaveData.AmountOfOxygenInLastStorage;
    }

    // DONE
    public float LoadMoney()
    {
        return GeneralSaveData.AmountOfMoney;
    }

    // Inventory
    // DONE
    public List<GeneralSaveData.InventoryItem> LoadInventory()
    {
        // Just overwrite anything already existing
        return GeneralSaveData.InventoryItems;
    }

    #endregion
}
