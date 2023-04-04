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

    public List<VendorInventoryItem> Items;
    public float[] BuyMultipliers;
    public float[] SellMultipliers;

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

        Debug.Log("Arvo kertoimet vendorille");

        // Get amount from somewhere. Maybe where item scriptable objects are stored?
        BuyMultipliers = new float[GameManager.Instance.InventoryController.Inventory.itemDatabase.items.Count];

        for (int i = 0; i < BuyMultipliers.Length; i++)
        {
            BuyMultipliers[i] = Random.Range(0.2f, 1.0f);
        }

        SellMultipliers = new float[GameManager.Instance.InventoryController.Inventory.itemDatabase.items.Count];

        Debug.LogError("Item count is " + GameManager.Instance.InventoryController.Inventory.itemDatabase.items.Count);

        for (int i = 0; i < SellMultipliers.Length; i++)
        {
            SellMultipliers[i] = Random.Range(1.0f, 3.0f);
        }

        Debug.LogError("Buy and sell multipliers are not currently implemented properly and not saved anywhere.");


        Items = new List<VendorInventoryItem>();

        // How much should the maximum amount of items be? Probably not all items. That would clutter the inventory!!!
        // And should there be some items that always spawn. Like fuel? That should spawn at least once per star system!!!
        int amountOfItems = Random.Range(3, GameManager.Instance.InventoryController.Inventory.itemDatabase.items.Count + 1);

        List<Item> allItems = new List<Item>(GameManager.Instance.InventoryController.Inventory.itemDatabase.items);



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
                Debug.Log("Iterating in a while-loop! " + iterationsInAWhileLoop);
                bool foundAnItem = false;
                Item foundItem = null;
                int amount = 0;
                
                // Decide if we want to spawn this item 

                for (int j = 0; j < allItems.Count; j++)
                {
                    bool shouldHaveMoreRandomWeight = false;

                    //if (allItems[j].id == 10
                    //    || allItems[j].id == 8)
                    //{
                    //    shouldHaveMoreRandomWeight = true;
                    //}


                    float random = Random.Range(0, 1.0f);

                    if (shouldHaveMoreRandomWeight)
                    {
                        random = Random.Range(0.33f, 1.0f);
                    }

                    float threshold = 1.0f / GameManager.Instance.InventoryController.Inventory.itemDatabase.items.Count;

                    Debug.Log("random threshold is " + threshold + " random number is " + random);

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
                    if (!foundItem.stackable)
                    {
                        amount = 1;
                    }

                    else
                    {
                        amount = Random.Range(0, 400);
                        Debug.LogError("But how many items we should spawn?!?");
                    }

                    Items.Add(new VendorInventoryItem(foundItem.id, amount));
                    allItems.Remove(foundItem);
                    break;
                }


            }

            Debug.Log("arvotaan kama " + i);
        }

        Debug.Log("Arvo kamat vendorille. Kamojen määrä on " + amountOfItems);



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
        return SellMultipliers[itemId];
    }

    public float GetBuyMultiplier(int itemId)
    {
        return BuyMultipliers[itemId];
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
        Debug.LogError("Missing functionality: Should check if there is some fuel somewhere in the star system!!!");
    }
}
