using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    [SerializeField] private TextMeshProUGUI weightDisplay;
    public List<Item> playerItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public Item itemToAdd;
    public Item equippedDrill;
    public Item equippedSpacesuit;
    public Item equippedShipWeapon;
    public double maxWeight;
    public double currentWeight = 0;

    public GameObject ScrollviewLayout;
    public List<ItemScript> ItemScripts = new List<ItemScript>();

    private void Start()
    {
        UpdateWeight();
    }
    //Tarkistaa onko pelaajalla itemiä
    public Item CheckForItem(int id)
    {
        return playerItems.Find(item => item.id == id);
    }
    
    //Antaa pelaajalle itemin. Jos pelaajalla ei ole ennestään sitä, lisää uuden rivin inventoryyn. Jos pelaajalla on jo inventoryssa se ja tavara on stackattava, lisää määrään lisää.
    public void AddItem(int id, int amount)
    {
        Item item = CheckForItem(id);
        itemToAdd = itemDatabase.GetItem(id);
        if (item == null)
        {
            playerItems.Add(itemToAdd);
            Object prefab = Resources.Load("Prefabs/item");
            if (prefab == null)
            {
                Debug.LogError("Null prefab");
            }

            if (layout == null)
            {
                Debug.LogError("Null layoout");
            }

            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
            //newItem.GetComponent<ItemScript>().Setup();
            ItemScript itemScript = newItem.GetComponent<ItemScript>();
            itemScript.AddItem(amount, itemToAdd);
            ItemScripts.Add(itemScript);
            currentWeight += itemToAdd.weight * amount;
            UpdateWeight();
            //Debug.LogError("ADDED ITEM");
        }
        else if(item.stackable)
        {
            //ItemScript[] allItems = layout.GetComponentsInChildren<ItemScript>(true);

            //for (int i = 0; i < allItems.Length; i++)
            //{
            //    if (allItems[i].itemToAdd.id == id)
            //    {
            //        allItems[i].AddItem(amount);
            //    }
            //}

            //GameObject.Find("InventoryPanel/Scroll/View/Layout/" + id).GetComponent<ItemScript>().AddItem(amount);

            GetItemScript(id).AddItem(amount, item);

            currentWeight += item.weight * amount;
            UpdateWeight();
        }
    }

    //Poistaa pelaajalta itemin. Jos pelaajalla on jo ennestään sitä enemmän kuin poistettava määrä, poistaa määrästä. Jos pelaajalla on saman verran tai vähemmän kuin poistettava määrä, poistaa rivin inventorysta.
    public void RemoveItem(int id, int amount)
    {

        Item item = CheckForItem(id);
        if(item != null)
        {
            //ItemScript itemScript = GameObject.Find("InventoryPanel/Scroll/View/Layout/" + id.ToString()).GetComponent<ItemScript>();
            ItemScript itemScript = GetItemScript(id);

            if (itemScript == null)
            {
                Debug.LogError("Item script is null. Returning");
                return;
            }

            if (itemScript.currentItemAmount <= amount)
            {
                ItemScripts.Remove(itemScript);
                playerItems.Remove(item);
                currentWeight -= item.weight * itemScript.currentItemAmount;
                itemScript.currentItemAmount = 0;
               // itemScript.UpdateShopAmount();
                Destroy(itemScript.gameObject);
                UpdateWeight();
            }
            else
            {
                ItemScripts.Remove(itemScript);
                itemScript.RemoveItem(amount);
                currentWeight -= item.weight * amount;
                UpdateWeight();
            }
        }

    }

    public void UpdateWeight()
    {
        weightDisplay.text = currentWeight + "/" + maxWeight;
    }

    public void SortByWeight()
    {
        SortByName(false);

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " weight is " + ItemScripts[i].currentItemWeight);
        }

        ItemScripts = ItemScripts.OrderBy(x => x.currentItemWeight).ToList();
        //playerItems = playerItems.OrderBy(x => x.weight).ToList();

        SwapListToDescendingOrder();

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " weight is " + +ItemScripts[i].currentItemWeight);
        }

        ResortItemsForInventoryView();
        Debug.Log("Sorting items by weight");
    }

    public void SortByValue()
    {
        SortByName(false);

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " value is " + ItemScripts[i].currentTotalValue);
        }

        ItemScripts = ItemScripts.OrderBy(x => x.currentTotalValue).ToList();

        SwapListToDescendingOrder();


        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " value is " + +ItemScripts[i].currentTotalValue);
        }

        ResortItemsForInventoryView();
        Debug.Log("Sorting items by value");
    }

    public void SortByAmount()
    {
        SortByName(false);

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " amount is " + ItemScripts[i].currentItemAmount);
        }

        ItemScripts = ItemScripts.OrderBy(x => x.currentItemAmount).ToList();

        SwapListToDescendingOrder();

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " amount is " + +ItemScripts[i].currentItemAmount);
        }


        ResortItemsForInventoryView();
        Debug.Log("Sorting items by amount");
    }

    public void SortByName(bool resortItems = true)
    {
        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name);
        }

        ItemScripts = ItemScripts.OrderBy(x => x.itemToAdd.name).ToList();
        //playerItems = playerItems.OrderBy(x => x.weight).ToList();

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name);
        }

        Debug.Log("Sorting items by name");
        
        if (resortItems) 
        {
            ResortItemsForInventoryView();
        }
    }

    public void SwapListToDescendingOrder()
    {
        List<ItemScript> descending = new List<ItemScript>();

        for (int i = ItemScripts.Count - 1; i >= 0; i--)
        {
            descending.Add(ItemScripts[i]);
        }

        ItemScripts = descending;

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            Debug.Log(ItemScripts[i].itemToAdd.name);
        }
    }

    // This is a hackish solution. Probably should refactor instead of doing this!
    public void ResortItemsForInventoryView()
    {

        List<int> ids = new List<int>();
        List<int> amounts = new List<int>();

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            ids.Add(ItemScripts[i].itemToAdd.id);
            amounts.Add(ItemScripts[i].currentItemAmount);
        }

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    RemoveItem(ItemScripts[i].itemToAdd.id, ItemScripts[i].currentItemAmount);
        //}

        //ItemScripts = new List<ItemScript>();

        for (int i = 0; i < ids.Count; i++)
        {
            RemoveItem(ids[i], amounts[i]);
        }

        for (int i = 0; i < ids.Count; i++)
        {
            AddItem(ids[i], amounts[i]);
        }

        //Debug.LogError("Sohuld resort items for inventory view. Not yet functionality for that implemented");
    }

    //Testausta varten. Poistettava myöhemmin.
    public void ButtonTestIron()
    {
        AddItem(0, 10);
    }
    public void ButtonTestGold()
    {
        AddItem(1, 2);
    }
    public void ButtonTestRemoveIron()
    {
        RemoveItem(0, 10);
    }
    public void ButtonTestRemoveGold()
    {
        RemoveItem(1, 2);
    }
    public void ButtonTestCap()
    {
        AddItem(2, 1);
    }
    public void ButtonTestSandwich()
    {
        AddItem(3, 1);
    }
    public void ButtonTestAdvancedDrill()
    {
        AddItem(4, 1);
    }
    public void ButtonTestSpacesuit()
    {
        AddItem(5, 1);
    }
    public void ButtonTestShipLaser()
    {
        AddItem(6, 1);
    }
    public void ButtonTestShipGun()
    {
        AddItem(7, 1);
    }
    public void ButtonForceAdd()
    {
        Object prefab = Resources.Load("Prefabs/item");
        GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
    }

    public bool CheckIfWeHaveRoomForItem(Item item)
    {
        if (item.weight + currentWeight > maxWeight)
        {
            Debug.Log("No room for item in inventory");
            return false;
        }

        else
        {
            Debug.Log("We HAVE ROOM for item in inventory");
            return true;
        }
    }

    public ItemScript GetItemScript(int id)
    {
        ItemScript itemScript = null;

        for (int i = 0; i < ItemScripts.Count; i++)
        {
            if (ItemScripts[i].itemToAdd.id == id)
            {
                itemScript = ItemScripts[i];
            }
        }

        return itemScript;
    }

    public void OnItemSold(int id, 
                           int newAmount,
                           int sellAmount)
    {
        RemoveItem(id, sellAmount);

        Debug.Log("Item id " + id + " SOLD. New amount is " + newAmount + " sell amount is " + sellAmount);
        Debug.LogError("Update total value accordingly");

        // update inventory amount
    } 

    public void OnItemBought(int id, 
                             int newAmount,
                             int buyAmount)
    {
        AddItem(id, buyAmount);
        Debug.Log("Item id " + id + " BOUGHT. New amount is " + newAmount + " buy amount is " + buyAmount);
        Debug.LogError("Update total value accordingly");
        // update inventory amount
    }
}
