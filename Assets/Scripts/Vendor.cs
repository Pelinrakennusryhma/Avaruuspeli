using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Vendor
{
    [System.Serializable]
    public class VendorInventoryItem
    {
        public int ItemId;
        public int ItemAmount;

        public VendorInventoryItem(int itemId, 
                                   int amount)
        {
            ItemId = itemId;
            ItemAmount = amount;
        }
    }

    [System.Serializable]
    public class ItemMultiplier
    {
        public int ItemId;
        public float Multiplier;

        public ItemMultiplier(int itemId,
                              float multiplier)
        {
            ItemId = itemId;
            Multiplier = multiplier;
        }
    }

    public List<VendorInventoryItem> Items;


    public ItemMultiplier[] BuyMultiplierss;
    public ItemMultiplier[] SellMultiplierss;

    public int GalaxyID;
    public int StarSystemID;
    public int PlanetID;

    public void InitializeVendor(int galaxyId,
                                 int starSystemId,
                                 int planetId)
    {
        GalaxyID = galaxyId;
        StarSystemID = starSystemId;
        PlanetID = planetId;

        //Debug.Log("Arvo kertoimet vendorille");

        // Get amount from somewhere. Maybe where item scriptable objects are stored?
        //BuyMultipliers = new float[GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count + 1];

        //for (int i = 0; i < BuyMultipliers.Length; i++)
        //{
        //    BuyMultipliers[i] = Random.Range(0.2f, 1.0f);
        //}

        BuyMultiplierss = new ItemMultiplier[GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count];

        for (int i = 0; i < BuyMultiplierss.Length; i++)
        {
            BuyMultiplierss[i] = new ItemMultiplier(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems[i].id,
                                                    Random.Range(0.2f, 1.0f));
        }

        SellMultiplierss = new ItemMultiplier[GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count];


        for (int i = 0; i < SellMultiplierss.Length; i++)
        {
            SellMultiplierss[i] = new ItemMultiplier(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems[i].id,
                                                     Random.Range(1.0f, 3.0f));
        }
        //Debug.LogError("Item count is " + (GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count + 1));

        //for (int i = 0; i < SellMultipliers.Length; i++)
        //{
        //    SellMultipliers[i] = Random.Range(1.0f, 3.0f);
        //}

        //Debug.LogError("Buy and sell multipliers are not currently implemented properly and not saved anywhere.");


        Items = new List<VendorInventoryItem>();

        // How much should the maximum amount of items be? Probably not all items. That would clutter the inventory!!!
        // And should there be some items that always spawn. Like fuel? That should spawn at least once per star system!!!
        int amountOfItems = Random.Range(3, 11);

        List<ItemSO> allItems = new List<ItemSO>(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems);



        // And how about resources and different item types? Drills?
        // Should the vendors have how much of what?
        CheckForFuelInStarSystem();
        int iterationsInAWhileLoop = 0;
        // NOTE: this is probably not that performant when there is a lot of items!!!
        // But to implement a weighted random system seems to be a bit difficult for me...
        for (int i = 0; i < amountOfItems; i++)
        {

            while (true) 
            {
                iterationsInAWhileLoop++;
                //Debug.Log("Iterating in a while-loop! " + iterationsInAWhileLoop);
                bool foundAnItem = false;
                ItemSO foundItem = null;
                int amount = 0;

                bool isFuelOrOxygen = false;
                
                // Decide if we want to spawn this item 

                for (int j = 0; j < allItems.Count; j++)
                {
                    bool shouldHaveMoreRandomWeight = false;

                    //if (allItems[j].id == 10
                    //    || allItems[j].id == 8)
                    //{
                    //    shouldHaveMoreRandomWeight = true;
                    //}

                    if (allItems[j].id == 15
                        || allItems[j].id == 14
                        || allItems[j].id == 13
                        || allItems[j].id == 16)
                    {
                        isFuelOrOxygen = true;
                    }


                    float random = Random.Range(0, 1.0f);

                    if (shouldHaveMoreRandomWeight)
                    {
                        random = Random.Range(0.33f, 1.0f);
                    }

                    float threshold = 1.0f / GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count;

                    if (isFuelOrOxygen)
                    {
                        random = 0;
                    }

                    //Debug.Log("random threshold is " + threshold + " random number is " + random);

                    if (random <= threshold)
                    {
                        foundItem = allItems[j];
                        foundAnItem = true;
                        break;
                    }
                }

                if (foundAnItem
                    && foundItem != null)
                {
                    if (!foundItem.isStackable)
                    {
                        amount = 1;
                    }

                    else if (isFuelOrOxygen)
                    {
                        amount = Random.Range(1, 5);
                    }

                    else
                    {
                        amount = Random.Range(1, 400);
                        //Debug.LogError("But how many items we should spawn?!?");
                    }

                    Items.Add(new VendorInventoryItem(foundItem.id, amount));
                    allItems.Remove(foundItem);
                    break;
                }


            }

            //Debug.Log("arvotaan kama " + i);
        }

        //Debug.Log("Arvo kamat vendorille. Kamojen määrä on " + amountOfItems);



        //Items.Add(new VendorInventoryItem(10, 10));
        //Items.Add(new VendorInventoryItem(6, 21));
        //Items.Add(new VendorInventoryItem(2, 1));
        //Items.Add(new VendorInventoryItem(3, 3));
        //Items.Add(new VendorInventoryItem(1, 3));
        //Items.Add(new VendorInventoryItem(4, 3));
        //Items.Add(new VendorInventoryItem(5, 3));
        //Items.Add(new VendorInventoryItem(7, 3));

        GameManager.Instance.SaverLoader.SaveVendor(this);
    }

    public float GetSellMultiplier(int itemId)
    {
        float value = 1.0f;
        //bool foundAValue = false;

        for (int i = 0; i < SellMultiplierss.Length; i++)
        {
            if (SellMultiplierss[i].ItemId == itemId)
            {
                value = SellMultiplierss[i].Multiplier;
                //foundAValue = true;
                break;
            }
        }

        //if (foundAValue)
        //{
        //    Debug.Log("Found a sell multiplier value " + value + " for item id " + itemId);
        //}

        //else
        //{
        //    Debug.LogError("Tried to fetch a sell multiplier with no success. Item id is " + itemId);
        //}


        return value;
        //if (itemId < 0
        //    || itemId >= SellMultipliers.Length)
        //{
        //    Debug.LogError("Totally invalid id. Sell multipliers do not match");
        //}

        //Debug.LogError("Replace this approach with a serializable double array!! THERE WILL BE PROBLEMS WITH ID's");

        //return SellMultipliers[itemId];
    }

    public float GetBuyMultiplier(int itemId)
    {
        float value = 1.0f;
        //bool foundAValue = false;

        for (int i = 0; i < BuyMultiplierss.Length; i++)
        {
            if (BuyMultiplierss[i].ItemId == itemId)
            {
                value = BuyMultiplierss[i].Multiplier;
                //foundAValue = true;
                break;
            }
        }

        //if (foundAValue)
        //{
        //    Debug.Log("Found a buy multiplier value " + value + " for item id " + itemId);
        //}

        //else
        //{
        //    Debug.LogError("Tried to fetch a buy multiplier with no success. Item id is " + itemId);
        //}


        return value;

        //if (itemId < 0
        //    || itemId >= BuyMultipliers.Length)
        //{
        //    Debug.LogError("Totally invalid id. Buy multipliers do not match");
        //}


        //Debug.LogError("Replace this approach with a serializable double array!! THERE WILL BE PROBLEMS WITH ID's");

        //return BuyMultipliers[itemId];
    }

    public static Vendor GetVendor(int galaxyID, 
                                   int starSystemID, 
                                   int planetID)
    {
        //Debug.LogError("Getting vendor. Implementation doesn't exist yet. galaxy is " + galaxyID + " star system is " + starSystemID + " planet is " + planetID);

        return GameManager.Instance.SaverLoader.GetVendor(galaxyID, starSystemID, planetID);
    }

    public void CheckForFuelInStarSystem()
    {
        //Debug.LogError("Missing functionality: Should maybe check if there is some fuel somewhere in the star system!!! BUT HOW, that is the question?");
    }
}
