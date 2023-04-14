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

    public List<FuelItemSO> fuels = new List<FuelItemSO>();
    public List<DrillItemSO> drills = new List<DrillItemSO>();
    public List<PlayerWeaponItemSO> playerWeapons = new List<PlayerWeaponItemSO>();
    public List<ShipItemSO> shipItems = new List<ShipItemSO>();
    public List<ShipWeaponItemSO> shipWeapons = new List<ShipWeaponItemSO>();

    public ItemSO itemToAdd;

    void Start()
    {
        allItems = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems;
        resources = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.Resources;
        equipment = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.EquipmentItems;
        consumables = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.ConsumableItems;

        fuels = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.FuelItems;
        drills = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.DrillItems;
        playerWeapons = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.PlayerWeaponItems;
        shipItems = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.ShipItems;
        shipWeapons = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.ShipWeaponItems;


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
        //Debug.LogError("Missing functionality to show all items");

        foreach (ItemSO item in allItems)
        {
            CreateItem(item);

            //itemToAdd = item;
            //Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            //GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            //newItem.name = item.id.ToString();
        }
    }


    // FROM NOW ON THERE'S A LOT OF SIMILAR COPY/PASTE CODE
    // MAYBE THIS COULD BE REFACTORED?
    // BUT MAYBE NOT, BECAUSE WE NEED FUNCTIONS FOR BUTTONS


    public void ShowResources()
    {
        //Debug.Log("About to show resources " + Time.time);

        foreach(Resource item in resources)
        {
            CreateItem(item);
            //itemToAdd = item;
            //Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            //GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            //newItem.name = item.id.ToString();
        }
    }
    public void ShowEquipment()
    {
        //Debug.Log("About to show equipment " + Time.time);


        foreach (EquipmentItemSO item in equipment)
        {
            CreateItem(item);

            //itemToAdd = item;
            //Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            //GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            //newItem.name = item.id.ToString();
        }
    }

    private void CreateItem(ItemSO item)
    {
        itemToAdd = item;
        Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
        GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
        newItem.name = item.id.ToString();
    }

    public void ShowConsumables()
    {
        //Debug.Log("About to show consumables " + Time.time);


        foreach (ConsumableItemSO item in consumables)
        {
            CreateItem(item);
            //itemToAdd = item;
            //Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            //GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            //newItem.name = item.id.ToString();
        }
    }

    public void ShowFuels()
    {
        //Debug.Log("About to show fuels " + Time.time);


        foreach (FuelItemSO item in fuels)
        {
            CreateItem(item);
        }
    }

    public void ShowDrills()
    {
        //Debug.Log("About to show drills " + Time.time);


        foreach (DrillItemSO item in drills)
        {
            CreateItem(item);
        }
    }

    public void ShowPlayerWeapons()
    {
        //Debug.Log("About to show ship weapons " + Time.time);


        foreach (PlayerWeaponItemSO item in playerWeapons)
        {
            CreateItem(item);
        }
    }

    public void ShowShipItems()
    {
        //Debug.Log("About to show ship items " + Time.time);


        foreach (ShipItemSO item in shipItems)
        {
            CreateItem(item);
        }
    }

    public void ShowShipWeapons()
    {
        //Debug.Log("About to show ship weapons " + Time.time);


        foreach (ShipWeaponItemSO item in shipWeapons)
        {
            CreateItem(item);
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

        //Debug.Log("About to show all items");
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

    public void ShowOnlyFuels()
    {
        ResetCatalog();
        ShowFuels();
    }


    //fuels 
    //drills
    //playerWeapons
    //shipItems
    //shipWeapons

    public void ShowOnlyDrills()
    {
        ResetCatalog();
        ShowDrills();
    }

    public void ShowOnlyPlayerWeapons()
    {
        ResetCatalog();
        ShowPlayerWeapons();
    }

    public void ShowOnlyShipItems()
    {
        ResetCatalog();
        ShowShipItems();
    }

    public void ShowOnlyShipWeapons()
    {
        ResetCatalog();
        ShowShipWeapons();
    }
}
