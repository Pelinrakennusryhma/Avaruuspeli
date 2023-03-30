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


    public void InitializeVendor()
    {
        // Get amount from somewhere. Maybe where item scriptable objects are stored?
        BuyMultipliers = new float[11];

        for (int i = 0; i < BuyMultipliers.Length; i++)
        {
            BuyMultipliers[i] = Random.Range(0.2f, 1.0f);
        }

        SellMultipliers = new float[11];

        for (int i = 0; i < SellMultipliers.Length; i++)
        {
            SellMultipliers[i] = Random.Range(1.0f, 3.0f);
        }

        Debug.LogError("Buy and sell multipliers are not currently implemented properly and not saved anywhere.");

        Items = new List<VendorInventoryItem>();
        Items.Add(new VendorInventoryItem(10, 10));
        Items.Add(new VendorInventoryItem(6, 21));
        Items.Add(new VendorInventoryItem(2, 1));
        Items.Add(new VendorInventoryItem(3, 3));
        Items.Add(new VendorInventoryItem(1, 3));
        Items.Add(new VendorInventoryItem(4, 3));
        Items.Add(new VendorInventoryItem(5, 3));
        Items.Add(new VendorInventoryItem(7, 3));
    }

    public float GetSellMultiplier(int itemId)
    {
        return SellMultipliers[itemId];
    }

    public float GetBuyMultiplier(int itemId)
    {
        return BuyMultipliers[itemId];
    }
}
