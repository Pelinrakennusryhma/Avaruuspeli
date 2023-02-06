using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public enum ResourceType
    {
        None = 0,
        TestDice = 1
    }

    public static ResourceInventory Instance;

    public static int AmountOfTestDice;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this; 
    }

    public void CollectResource(ResourceType collectedResourceType)
    {
        Debug.Log("Collected " + collectedResourceType.ToString());

        if (collectedResourceType == ResourceType.TestDice)
        {
            AmountOfTestDice++;
            Debug.Log("Amount of test dice " + AmountOfTestDice);
        }
    }
}
