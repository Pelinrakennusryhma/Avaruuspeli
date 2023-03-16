using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceInventory : MonoBehaviour
{
    public static ResourceInventory Instance;

    Dictionary<Item, int> inventory = new Dictionary<Item, int>();

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this; 
    }

    public void CollectResource(Item collectedResourceType, int amount = 1)
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
}
