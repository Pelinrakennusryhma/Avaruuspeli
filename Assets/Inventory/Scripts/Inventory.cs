using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    [SerializeField] private TextMeshProUGUI weightDisplay;
    public List<ItemSO> playerItems = new List<ItemSO>();
    //public ItemDatabase itemDatabase;
    public ItemSO itemToAdd;
    public ItemSO equippedDrill;
    public ItemSO equippedSpacesuit;
    public ItemSO equippedShipWeapon;
    public double maxWeight;
    public double currentWeight = 0;

    public GameObject ScrollviewLayout;
    public List<ItemScript> ItemScripts = new List<ItemScript>();

    private void Start()
    {
        maxWeight = 100000;
        UpdateWeight();
    }
    //Tarkistaa onko pelaajalla itemiä
    public ItemSO CheckForItem(int id)
    {
        Debug.LogError("Maybe replace this with another kind of check?");
        return playerItems.Find(item => item.id == id);
    }
    
    //Antaa pelaajalle itemin. Jos pelaajalla ei ole ennestään sitä, lisää uuden rivin inventoryyn. Jos pelaajalla on jo inventoryssa se ja tavara on stackattava, lisää määrään lisää.
    public void AddItem(int id, int amount)
    {
        ItemSO item = CheckForItem(id);

        //if (GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO == null)
        //{
        //    Debug.LogError("NULL GAME MANAGER");
        //}

        //else
        //{
        //    Debug.LogError("WE have a valid item database");
        //}

        //if (!GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.HasBeenInitted)
        //{
        //    Debug.LogError("Database has not been initted");
        //}

        itemToAdd = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(id);
        
        Debug.Log("About to add item " + Time.time);
        
        if (item == null)
        {
            playerItems.Add(itemToAdd);
            Object prefab = Resources.Load("Prefabs/item");
            Debug.LogError("REplace this resources load with something else?");
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
            itemScript.Setup(this);
            itemScript.AddItem(amount, itemToAdd);
            ItemScripts.Add(itemScript);
            currentWeight += itemToAdd.weight * amount;
            UpdateWeight();
            //Debug.LogError("ADDED ITEM");
        }
        else if(item.isStackable)
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

            ItemScript itemScript = GetItemScript(id);

            //if (itemScript != null) 
            //{
                itemScript.AddItem(amount, item);

                currentWeight += item.weight * amount;
                UpdateWeight();
            //}
        }
    }

    //Poistaa pelaajalta itemin. Jos pelaajalla on jo ennestään sitä enemmän kuin poistettava määrä, poistaa määrästä. Jos pelaajalla on saman verran tai vähemmän kuin poistettava määrä, poistaa rivin inventorysta.
    public void RemoveItem(int id, int amount)
    {

        ItemSO item = CheckForItem(id);
        if(item != null)
        {
            //ItemScript itemScript = GameObject.Find("InventoryPanel/Scroll/View/Layout/" + id.ToString()).GetComponent<ItemScript>();
            ItemScript itemScript = GetItemScript(id);

            if (itemScript == null)
            {
                Debug.LogError("Item script is null. Returning");
                //return;
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
                //ItemScripts.Remove(itemScript);


                itemScript.RemoveItem(amount);
                currentWeight -= item.weight * amount;
                UpdateWeight();
            }
        }

        else
        {
            Debug.LogError("Item is null");
        }

    }

    public void UpdateWeight()
    {
        weightDisplay.text = currentWeight + "/" + maxWeight;
    }

    public void SortByWeight()
    {
        SortByName(false);

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " weight is " + ItemScripts[i].currentItemWeight);
        //}

        ItemScripts = ItemScripts.OrderBy(x => x.currentItemWeight).ToList();
        //playerItems = playerItems.OrderBy(x => x.weight).ToList();

        SwapListToDescendingOrder();

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " weight is " + +ItemScripts[i].currentItemWeight);
        //}

        ResortItemsForInventoryView();
        Debug.Log("Sorting items by weight");
    }

    public void SortByValue()
    {
        SortByName(false);

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " value is " + ItemScripts[i].currentTotalValue);
        //}

        ItemScripts = ItemScripts.OrderBy(x => x.currentTotalValue).ToList();

        SwapListToDescendingOrder();


        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " value is " + +ItemScripts[i].currentTotalValue);
        //}

        ResortItemsForInventoryView();
        Debug.Log("Sorting items by value");
    }

    public void SortByAmount()
    {
        SortByName(false);

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " amount is " + ItemScripts[i].currentItemAmount);
        //}

        ItemScripts = ItemScripts.OrderBy(x => x.currentItemAmount).ToList();

        SwapListToDescendingOrder();

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " amount is " + +ItemScripts[i].currentItemAmount);
        //}


        ResortItemsForInventoryView();
        Debug.Log("Sorting items by amount");
    }

    public void SortByName(bool resortItems = true)
    {
        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name);
        //}

        ItemScripts = ItemScripts.OrderBy(x => x.itemToAdd.itemName).ToList();
        //playerItems = playerItems.OrderBy(x => x.weight).ToList();

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name);
        //}

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
            Debug.Log(ItemScripts[i].itemToAdd.itemName);
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

    public bool CheckIfWeHaveRoomForItem(ItemSO item, int itemAmount)
    {
        float wouldBeWeight = ((float)item.weight * itemAmount)  + (float)currentWeight;

        if (wouldBeWeight > maxWeight)
        {

            Debug.Log("No room for item in inventory " + " itemweight is " + item.weight * itemAmount + " would be weight is "+wouldBeWeight + " max weight is " + maxWeight);
            return false;
        }

        else
        {
            Debug.Log("We HAVE ROOM for item in inventory. Would be weight is " + wouldBeWeight + " current weight is " + currentWeight);
            return true;
        }
    }

    public bool TryToBuyItemWithMoney(float moneyRequired,
                                      bool substractAmount)
    {
        Debug.LogError("Money required to buy is " + moneyRequired);

        if (GameManager.Instance.InventoryController.Money >= moneyRequired)
        {
            if (substractAmount) 
            {
                GameManager.Instance.InventoryController.Money -= moneyRequired;
                Debug.LogError("New amount of money is " + GameManager.Instance.InventoryController.Money);
            }

            return true;
        }
        
        else
        {
            return false;
        }
    }

    public void AddMoney(float moneyToAdd)
    {
        GameManager.Instance.InventoryController.Money += moneyToAdd;
    }

    public void UpdateWeight(float weightToAdd)
    {
       // currentWeight += weightToAdd;

        Debug.Log("Current weight is " + currentWeight + " weight to add is " + weightToAdd);
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
                           int sellAmount)
    { 
        RemoveItem(id, sellAmount);



        Debug.Log("Item id " + id + " SOLD. Sell amount is " + sellAmount);

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
