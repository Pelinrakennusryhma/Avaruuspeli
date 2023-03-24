using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceInventory : MonoBehaviour
{
    public static ResourceInventory Instance;

    Dictionary<Resource, int> inventory = new Dictionary<Resource, int>();

    public static int AmountOfGoldSinceLastInventoryLaunch;
    public static int AmountOfSilverSinceLastInventoryLaunch;
    public static int AmountOfCopperSinceLastInventoryLaunch;
    public static int AmountOfIronSinceLastInventoryLaunch;
    public static int AmountOfDiamondsSinceLastInventoryLaunch;

    public ShowHideInventory ShowHideInventory;

    public TextMeshProUGUI ShoppingPrompt;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        ShoppingPrompt.gameObject.SetActive(false);
    }

    public void CollectResource(Resource collectedResourceType, int amount = 1)
    {
        //Debug.Log("Collected " + collectedResourceType.ToString());

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
    public void UnloadGatheredItems(Inventory inventory)
    {
        if (AmountOfGoldSinceLastInventoryLaunch > 0)
        {
            inventory.AddItem(1, AmountOfGoldSinceLastInventoryLaunch);
        }

        if (AmountOfSilverSinceLastInventoryLaunch > 0)
        {
            inventory.AddItem(8, AmountOfSilverSinceLastInventoryLaunch);
        }

        if (AmountOfCopperSinceLastInventoryLaunch > 0)
        {
            inventory.AddItem(9, AmountOfCopperSinceLastInventoryLaunch);
        }

        if (AmountOfIronSinceLastInventoryLaunch > 0)
        {
            inventory.AddItem(0, AmountOfIronSinceLastInventoryLaunch);
        }

        if (AmountOfDiamondsSinceLastInventoryLaunch > 0)
        {
            inventory.AddItem(10, AmountOfDiamondsSinceLastInventoryLaunch);
        }

        AmountOfGoldSinceLastInventoryLaunch = 0;
        AmountOfSilverSinceLastInventoryLaunch = 0;
        AmountOfCopperSinceLastInventoryLaunch = 0;
        AmountOfIronSinceLastInventoryLaunch = 0;
        AmountOfDiamondsSinceLastInventoryLaunch = 0;



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
