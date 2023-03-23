using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public List<Item> playerItems = new List<Item>();
    public ItemDatabase itemDatabase;
    public Item itemToAdd;
    public Item equippedDrill;
    public Item equippedSpacesuit;
    public Item equippedShipWeapon;

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
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
            newItem.GetComponent<ItemScript>().AddItem(amount);

        }
        else if(item.stackable)
        {
            GameObject.Find("InventoryPanel/Scroll/View/Layout/" + id).GetComponentInChildren<ItemScript>(true).AddItem(amount);
        }

    }

    //Poistaa pelaajalta itemin. Jos pelaajalla on jo ennestään sitä enemmän kuin poistettava määrä, poistaa määrästä. Jos pelaajalla on saman verran tai vähemmän kuin poistettava määrä, poistaa rivin inventorysta.
    public void RemoveItem(int id, int amount)
    {

        Item item = CheckForItem(id);
        if(item != null)
        {
            ItemScript itemScript = GameObject.Find("InventoryPanel/Scroll/View/Layout/" + id.ToString()).GetComponent<ItemScript>();
            if (itemScript.currentItemAmount <= amount)
            {
                playerItems.Remove(item);
                itemScript.currentItemAmount = 0;
                itemScript.UpdateShopAmount();
                Destroy(GameObject.Find("InventoryPanel/Scroll/View/Layout/" + id.ToString()));
            }
            else
            {
                itemScript.RemoveItem(amount);
            }
        }

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
}
