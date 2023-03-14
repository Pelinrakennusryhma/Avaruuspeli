using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public enum ResourceType
    {
        None = 0,
        TestDice = 1,
        Gold = 2,
        Silver = 3,
        Copper = 4,
        Iron = 5,
        Diamond = 6
    }

    public static ResourceInventory Instance;

    public static int AmountOfTestDice;
    public static int AmountOfGold;
    public static int AmountOfSilver;
    public static int AmountOfCopper;
    public static int AmountOfIron;
    public static int AmountOfDiamonds;

    public static int AmountOfGoldSinceLastInventoryLaunch;
    public static int AmountOfSilverSinceLastInventoryLaunch;
    public static int AmountOfCopperSinceLastInventoryLaunch;
    public static int AmountOfIronSinceLastInventoryLaunch;
    public static int AmountOfDiamondsSinceLastInventoryLaunch;

    public ShowHideInventory ShowHideInventory;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this; 
    }

    public void CollectResource(ResourceType collectedResourceType)
    {
        //Debug.Log("Collected " + collectedResourceType.ToString());

        //ShowHideInventory.ShowInventory();

        int amount = 0;

        if (collectedResourceType == ResourceType.TestDice)
        {
            AmountOfTestDice++;
            amount = AmountOfTestDice;
            //Debug.Log("Amount of test dice " + AmountOfTestDice);
        }

        else if (collectedResourceType == ResourceType.Gold)
        {
            AmountOfGold++;
            amount = AmountOfGold;
            AmountOfGoldSinceLastInventoryLaunch++;
            //ShowHideInventory.Inventory.AddItem(1, 1);
            //Debug.Log("Amount of gold " + AmountOfGold);
        }

        else if(collectedResourceType == ResourceType.Silver)
        {
            AmountOfSilver++;
            amount = AmountOfSilver;
            AmountOfSilverSinceLastInventoryLaunch++;
            //Debug.Log("Amount of silver " + AmountOfSilver);
        }

        else if (collectedResourceType == ResourceType.Copper)
        {
            AmountOfCopper++;
            amount = AmountOfCopper;
            AmountOfCopperSinceLastInventoryLaunch++;
            //Debug.Log("Amount of copper " + AmountOfCopper);
        }
        else if (collectedResourceType == ResourceType.Iron)
        {
            AmountOfIron++;
            amount = AmountOfIron;
            AmountOfIronSinceLastInventoryLaunch++;
            //Debug.Log("Amount of iron " + AmountOfIron);
        }

        else if (collectedResourceType == ResourceType.Diamond)
        {
            AmountOfDiamonds++;
            amount = AmountOfDiamonds;
            AmountOfDiamondsSinceLastInventoryLaunch++;
            //Debug.Log("Amount of diamonds " + AmountOfDiamonds);
        }

        ResourcePickUpPrompt.Instance.ShowResource(collectedResourceType, amount);

        //ShowHideInventory.HideInventory();
    }

    // Update is called once per frame
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
}
