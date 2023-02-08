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
        Iron = 5
    }

    public static ResourceInventory Instance;

    public static int AmountOfTestDice;
    public static int AmountOfGold;
    public static int AmountOfSilver;
    public static int AmountOfCopper;
    public static int AmountOfIron;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this; 
    }

    public void CollectResource(ResourceType collectedResourceType)
    {
        //Debug.Log("Collected " + collectedResourceType.ToString());

        int amount = 0;

        if (collectedResourceType == ResourceType.TestDice)
        {
            AmountOfTestDice++;
            amount = AmountOfTestDice;
            Debug.Log("Amount of test dice " + AmountOfTestDice);
        }

        else if (collectedResourceType == ResourceType.Gold)
        {
            AmountOfGold++;
            amount = AmountOfGold;
            Debug.Log("Amount of gold " + AmountOfGold);
        }

        else if(collectedResourceType == ResourceType.Silver)
        {
            AmountOfSilver++;
            amount = AmountOfSilver;
            Debug.Log("Amount of silver " + AmountOfSilver);
        }

        else if (collectedResourceType == ResourceType.Copper)
        {
            AmountOfCopper++;
            amount = AmountOfCopper;
            Debug.Log("Amount of copper " + AmountOfCopper);
        }
        else if (collectedResourceType == ResourceType.Iron)
        {
            AmountOfIron++;
            amount = AmountOfIron;
            Debug.Log("Amount of iron " + AmountOfIron);
        }

        ResourcePickUpPrompt.Instance.ShowResource(collectedResourceType, amount);
    }
}
