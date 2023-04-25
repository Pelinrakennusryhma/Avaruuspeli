using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceInventory : MonoBehaviour
{
    public static ResourceInventory Instance;

    Dictionary<Resource, int> inventory = new Dictionary<Resource, int>();

    //public static int AmountOfGoldSinceLastInventoryLaunch;
    //public static int AmountOfSilverSinceLastInventoryLaunch;
    //public static int AmountOfCopperSinceLastInventoryLaunch;
    //public static int AmountOfIronSinceLastInventoryLaunch;
    //public static int AmountOfDiamondsSinceLastInventoryLaunch;

    public ShowHideInventory ShowHideInventory;

    public TextMeshProUGUI ShoppingPrompt;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        
        if (ShoppingPrompt != null) 
        {
            ShoppingPrompt.gameObject.SetActive(false);
        }
    }

    public void Start()
    {
        SetResourceAmounts(GameManager.Instance.InventoryController.Inventory);
    }

    public void CollectResource(Resource collectedResourceType, int amount = 1)
    {
        Debug.Log("Collected " + collectedResourceType.ToString());

        int totalAmount;

        if (inventory.ContainsKey(collectedResourceType))
        {
            totalAmount = amount + inventory[collectedResourceType];
            inventory[collectedResourceType] = totalAmount;
        } else
        {
            inventory.Add(collectedResourceType, amount);
            totalAmount = amount;
        }

        ResourcePickUpPrompt.Instance.ShowResource(collectedResourceType, totalAmount);
    }
    public void UnloadGatheredItems(Inventory oldInventory)
    {
        //Debug.LogWarning("Time to update inventory amounts ");


        foreach (KeyValuePair<Resource, int> kvp in inventory)
        {
            oldInventory.ForceItemAmount(kvp.Key, kvp.Value);
        }




        //foreach (var kvp in this.inventory)
        //{
        //    if (kvp.Key.id == 9)
        //    {
        //        inventory.AddItem(9, kvp.Value);
        //        Debug.Log("Added copper");
        //    }

        //    else if (kvp.Key.id == 10)
        //    {
        //        inventory.AddItem(10, kvp.Value);
        //        Debug.Log("Added diamond");
        //    }
        //}

        //if (AmountOfGoldSinceLastInventoryLaunch > 0)
        //{
        //    inventory.AddItem(1, AmountOfGoldSinceLastInventoryLaunch);
        //}

        //if (AmountOfSilverSinceLastInventoryLaunch > 0)
        //{
        //    inventory.AddItem(8, AmountOfSilverSinceLastInventoryLaunch);
        //}

        //if (AmountOfCopperSinceLastInventoryLaunch > 0)
        //{
        //    inventory.AddItem(9, AmountOfCopperSinceLastInventoryLaunch);
        //}

        //if (AmountOfIronSinceLastInventoryLaunch > 0)
        //{
        //    inventory.AddItem(0, AmountOfIronSinceLastInventoryLaunch);
        //}

        //if (AmountOfDiamondsSinceLastInventoryLaunch > 0)
        //{
        //    inventory.AddItem(10, AmountOfDiamondsSinceLastInventoryLaunch);
        //}

        //AmountOfGoldSinceLastInventoryLaunch = 0;
        //AmountOfSilverSinceLastInventoryLaunch = 0;
        //AmountOfCopperSinceLastInventoryLaunch = 0;
        //AmountOfIronSinceLastInventoryLaunch = 0;
        //AmountOfDiamondsSinceLastInventoryLaunch = 0;



    }

    public void SetResourceAmounts(Inventory oldInventory)
    {
        // This is probably not the way to do this, but should just get this working first
        // Also, the item script is removed if resource reaches zero, which is maybe a little bad at least in this case...

        // We have to check for zeroes/non-existent resources too :(


        List<KeyValuePair<Resource, int>> toZeroOut = new List<KeyValuePair<Resource, int>>(); 

        foreach (KeyValuePair<Resource, int> kvp in inventory)
        {
            bool hasItemAtAll = oldInventory.CheckForItem(kvp.Key.id);

            if (!hasItemAtAll)
            {
                toZeroOut.Add(kvp);
                //inventory[kvp.Key] = 0; // invalid operation!!! Don't do this
                //Debug.Log("Zeroed out an item " + kvp.Key.itemName);
            }
        }

        for (int i = 0; i < toZeroOut.Count; i++)
        {
            inventory[toZeroOut[i].Key] = 0;
        }

        // Maybe we could just check for each resource, if the inventory contains it?

        for (int i = 0; i < oldInventory.InventoryItemScripts.Count; i++)
        {
            if (oldInventory.InventoryItemScripts[i].itemToAdd.itemType == ItemSO.ItemType.Resource)
            {
                Resource resourceSO = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetResourceItem(oldInventory.InventoryItemScripts[i].itemToAdd.id);

                if (resourceSO != null)
                {
                    int totalAmount = oldInventory.InventoryItemScripts[i].currentItemAmount;
                    
                    //Debug.LogWarning("Updating resource inventory amount. Resource is " + resourceSO.itemName + " amount is " + totalAmount);

                    if (inventory.ContainsKey(resourceSO))
                    {
                        //totalAmount = amount + inventory[collectedResourceType];
                        inventory[resourceSO] = totalAmount;

                    }
                    else
                    {
                        inventory.Add(resourceSO, totalAmount);
                        //totalAmount = amount;
                    }                        
                    
                    //Debug.Log("Set succesfully resource amount to " + totalAmount + " of resource " + resourceSO.itemName);
                }

                else
                {
                    //Debug.LogError("Invalid ResourceSO object. Don't set amounts to resource inventory");
                }
            }
        }


        //Debug.LogWarning("Should set resource amounts ");
        //Debug.Break();
    }

    public void OnEnterShoppingArea()
    {
        ShoppingPrompt.gameObject.SetActive(true);
        ShoppingPrompt.text = "PRESS E TO SHOP";
    }

    public void OnStartShopping()
    {
        ShoppingPrompt.text = "PRESS E TO EXIT SHOP";
    }

    public void OnExitShoppingArea()
    {
        ShoppingPrompt.gameObject.SetActive(false);
    }
}
