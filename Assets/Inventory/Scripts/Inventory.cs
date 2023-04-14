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
    public List<ItemScript> InventoryItemScripts = new List<ItemScript>();

    public void OnInventoryControllerInit()
    {
        maxWeight = 100000;
        UpdateWeight();
    }

    //private void Start()
    //{
    //    maxWeight = 100000;
    //    UpdateWeight();
    //}
    //Tarkistaa onko pelaajalla itemiä
    public ItemSO CheckForItem(int id)
    {
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
        
        //Debug.Log("About to add item " + Time.time);
        
        if (item == null)
        {
            playerItems.Add(itemToAdd);
            Object prefab = Resources.Load("Prefabs/item");
            

            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
            //newItem.GetComponent<ItemScript>().Setup();
            ItemScript itemScript = newItem.GetComponent<ItemScript>();
            itemScript.Setup(this);
            itemScript.AddItem(amount, itemToAdd);
            InventoryItemScripts.Add(itemScript);
            currentWeight += itemToAdd.weight * amount;
            UpdateWeight();
            //Debug.LogError("ADDED A NEw ITEM should now exist in itemscripts " + itemToAdd.itemName);
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
        //Debug.LogWarning("Removing item " + id);
        
        ItemSO item = CheckForItem(id);
        if(item != null)
        {
            //ItemScript itemScript = GameObject.Find("InventoryPanel/Scroll/View/Layout/" + id.ToString()).GetComponent<ItemScript>();
            ItemScript itemScript = GetItemScript(id);



            if (itemScript.currentItemAmount <= amount)
            {
                InventoryItemScripts.Remove(itemScript);
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
        weightDisplay.text = currentWeight.ToString("0.0") + "/" + maxWeight.ToString("0.0");
    }

    public void SortByWeight()
    {
        SortByName(false);

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " weight is " + ItemScripts[i].currentItemWeight);
        //}

        InventoryItemScripts = InventoryItemScripts.OrderByDescending(x => x.currentItemWeight).ToList();

        //There was a ready made functionality for this one already
        //SwapListToDescendingOrder();

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " weight is " + +ItemScripts[i].currentItemWeight);
        //}

        ResortItemsForInventoryView();
        //Debug.Log("Sorting items by weight");
    }

    public void SortByValue()
    {
        SortByName(false);

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " value is " + ItemScripts[i].currentTotalValue);
        //}

        InventoryItemScripts = InventoryItemScripts.OrderByDescending(x => x.currentTotalValue).ToList();

        //There was a ready made functionality for this one already
        //SwapListToDescendingOrder();


        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " value is " + +ItemScripts[i].currentTotalValue);
        //}

        ResortItemsForInventoryView();
        //Debug.Log("Sorting items by value");
    }

    public void SortByAmount()
    {
        SortByName(false);

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " amount is " + ItemScripts[i].currentItemAmount);
        //}

        InventoryItemScripts = InventoryItemScripts.OrderByDescending(x => x.currentItemAmount).ToList();

        // There was ready made functionality for this one already
        //SwapListToDescendingOrder();

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name + " amount is " + +ItemScripts[i].currentItemAmount);
        //}


        ResortItemsForInventoryView();
        //Debug.Log("Sorting items by amount");
    }

    public void SortByName(bool resortItems = true)
    {
        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name);
        //}

        InventoryItemScripts = InventoryItemScripts.OrderBy(x => x.itemToAdd.itemName).ToList();
        //playerItems = playerItems.OrderBy(x => x.weight).ToList();

        //for (int i = 0; i < ItemScripts.Count; i++)
        //{
        //    Debug.Log(i + " playeritem " + ItemScripts[i].itemToAdd.name);
        //}

        //Debug.Log("Sorting items by name");
        
        if (resortItems) 
        {
            ResortItemsForInventoryView();
        }
    }

    public void SwapListToDescendingOrder()
    {
        List<ItemScript> descending = new List<ItemScript>();

        for (int i = InventoryItemScripts.Count - 1; i >= 0; i--)
        {
            descending.Add(InventoryItemScripts[i]);
        }

        InventoryItemScripts = descending;

        for (int i = 0; i < InventoryItemScripts.Count; i++)
        {
            Debug.Log(InventoryItemScripts[i].itemToAdd.itemName);
        }
    }

    // This is a hackish solution. Probably should refactor instead of doing this!
    public void ResortItemsForInventoryView()
    {

        List<int> ids = new List<int>();
        List<int> amounts = new List<int>();

        for (int i = 0; i < InventoryItemScripts.Count; i++)
        {
            ids.Add(InventoryItemScripts[i].itemToAdd.id);
            amounts.Add(InventoryItemScripts[i].currentItemAmount);
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
        AddItem(2, 10);
    }
    public void ButtonTestGold()
    {
        AddItem(5, 2);
    }
    public void ButtonTestRemoveIron()
    {
        RemoveItem(2, 10);
    }
    public void ButtonTestRemoveGold()
    {
        RemoveItem(5, 2);
    }
    public void ButtonTestCap()
    {
        AddItem(2, 1);
    }
    public void ButtonTestSandwich()
    {
        AddItem(20, 1);
    }
    public void ButtonTestAdvancedDrill()
    {
        AddItem(8, 1);
    }
    public void ButtonTestSpacesuit()
    {
        AddItem(12, 1);
    }
    public void ButtonTestShipLaser()
    {
        AddItem(11, 1);
    }
    public void ButtonTestShipGun()
    {
        AddItem(10, 1);
    }
    public void ButtonForceAdd()
    {
        Object prefab = Resources.Load("Prefabs/item");
        GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
    }

    public bool CheckIfWeHaveRoomForItem(ItemSO item, int itemAmount)
    {
        double wouldBeWeight = ((double)item.weight * itemAmount)  + (double)currentWeight;

        if (wouldBeWeight > maxWeight)
        {

            Debug.Log("No room for item in inventory " + " itemweight is " + (item.weight * itemAmount) + " would be weight is "+ wouldBeWeight + " max weight is " + maxWeight);
            return false;
        }

        else
        {
            //Debug.Log("We HAVE ROOM for item in inventory. Would be weight is " + wouldBeWeight + " current weight is " + currentWeight);
            return true;
        }
    }

    public bool TryToBuyItemWithMoney(float moneyRequired,
                                      bool substractAmount)
    {
        //Debug.LogError("Money required to buy is " + moneyRequired);

        if (GameManager.Instance.InventoryController.Money >= moneyRequired)
        {
            if (substractAmount) 
            {
                GameManager.Instance.InventoryController.Money -= moneyRequired;
                //Debug.LogError("New amount of money is " + GameManager.Instance.InventoryController.Money);
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

    public void UpdateWeight(double weightToAdd)
    {
        currentWeight += weightToAdd;

        //Debug.Log("Current weight is " + currentWeight + " weight to add is " + weightToAdd);
    }

    public ItemScript GetItemScript(int id)
    {
        ItemScript itemScript = null;

        for (int i = 0; i < InventoryItemScripts.Count; i++)
        {
            if (InventoryItemScripts[i].itemToAdd.id == id)
            {
                itemScript = InventoryItemScripts[i];
                break;
            }
        }

        return itemScript;
    }

    public void OnItemSold(int id,
                           int sellAmount)
    { 
        RemoveItem(id, sellAmount);



        //Debug.Log("Item id " + id + " SOLD. Sell amount is " + sellAmount);

        // update inventory amount
    } 

    public void OnItemBought(int id, 
                             int newAmount,
                             int buyAmount)
    {
        AddItem(id, buyAmount);

        //Debug.Log("Item id " + id + " BOUGHT. New amount is " + newAmount + " buy amount is " + buyAmount);
        //Debug.LogError("Update total value accordingly");

    }

    public void ForceItemAmount(ItemSO item,
                                int newAmount)
    {
        // Maybe this whole thing should be made dictionary based eventually? Just maybe
        // But this thing would need heavy refactoring anyways.
        // Now, this is just to get things working again FAST, without a heavy rewrite yet.
        // Refactoring should be done though, if and when this proves problematic performance wise!!!
        // Not much critical though happens when this is called, so maybe we can get away with this??
        // Don't really want to refactor and optimize this thing yet and perhaps prematurely and in vain.


        //Debug.LogWarning("Forcing item amount");

        ItemSO foundItem = CheckForItem(item.id);

        if (foundItem == null)
        {
            //Debug.LogWarning("Didn't have an item in inventory. Adding it. Should also add to InventoryItemScripts");
            
            if (newAmount > 0) 
            {
                AddItem(item.id, newAmount);
            }

            else
            {
                RemoveItem(item.id, 99999);
            }
        }

        for (int i = 0; i < InventoryItemScripts.Count; i++)
        {
            if (InventoryItemScripts[i].itemToAdd == item)
            {
                InventoryItemScripts[i].currentItemAmount = newAmount;

                InventoryItemScripts[i].currentItemWeight = newAmount * item.weight;                
                InventoryItemScripts[i].UpdateAmount();
                //InventoryItemScripts[i].UpdateShopAmount();
                //InventoryItemScripts[i].Setup(this);
                //InventoryItemScripts[i].Setup(this);
                //Debug.LogWarning("Forcing item amount of item " + item.itemName + " amount is " + newAmount);             
                break;
            }
        }

        //Debug.Log("Forcing item amounts");
    }
}
