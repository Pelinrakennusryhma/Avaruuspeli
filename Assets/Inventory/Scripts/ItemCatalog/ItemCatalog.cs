using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatalog : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    //public ItemDatabase itemDatabase;
    public List<ItemSO> allItems = new List<ItemSO>();
    public List<Resource> resources = new List<Resource>();
    public List<EquipmentItemSO> equipment = new List<EquipmentItemSO>();
    public List<ConsumableItemSO> consumables = new List<ConsumableItemSO>();
    public ItemSO itemToAdd;

    void Start()
    {
        allItems = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems;
        resources = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.Resources;
        equipment = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.EquipmentItems;
        consumables = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.ConsumableItems;
        Debug.LogError("Should probably add other item types too. But requires more and rehauled UI and stuff");

        ////Lajittelee itemit omiin ryhmiin
        //foreach (Item item in itemDatabase.items)
        //{
        //    if(item.type == "Resource")
        //    {
        //        resources.Add(item);
        //    }
        //    else if ((item.type == "Drill") || (item.type == "Spacesuit") || (item.type == "ShipWeapon"))
        //    {
        //        equipment.Add(item);
        //    }
        //    else if (item.type == "Consumable")
        //    {
        //        consumables.Add(item);
        //    }

        //}
        ShowAll();
    }

    public void ShowAllItems()
    {
        Debug.LogError("Missing functionality to show all items");

        foreach (ItemSO item in allItems)
        {
            itemToAdd = item;
            Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = item.id.ToString();
        }
    }

    public void ShowResources()
    {
        Debug.Log("About to show resources " + Time.time);

        foreach(Resource item in resources)
        {
            itemToAdd = item;
            Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = item.id.ToString();
        }
    }
    public void ShowEquipment()
    {
        Debug.Log("About to show equipment " + Time.time);


        foreach (EquipmentItemSO item in equipment)
        {
            itemToAdd = item;
            Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = item.id.ToString();
        }
    }
    public void ShowConsumables()
    {
        Debug.Log("About to show consumables " + Time.time);


        foreach (ConsumableItemSO item in consumables)
        {
            itemToAdd = item;
            Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = item.id.ToString();
        }
    }
    public void ResetCatalog()
    {
        GameObject[] gos = GameObject.FindGameObjectsWithTag("CatalogItem");
        foreach (GameObject go in gos)
        {
            Destroy(go);
        }
    }
    public void ShowAll()
    {

        Debug.Log("About to show all items");
        ResetCatalog();

        ShowAllItems();

        //ShowResources();
        //ShowEquipment();
        //ShowConsumables();
    }

    public void ShowOnlyResources()
    {
        ResetCatalog();
        ShowResources();
    }
    public void ShowOnlyEquipment()
    {
        ResetCatalog();
        ShowEquipment();
    }
    public void ShowOnlyConsumables()
    {
        ResetCatalog();
        ShowConsumables();
    }
}
