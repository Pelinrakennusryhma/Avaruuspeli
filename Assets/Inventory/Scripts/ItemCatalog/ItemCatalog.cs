using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCatalog : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    public ItemDatabase itemDatabase;
    public List<Item> resources = new List<Item>();
    public List<Item> equipment = new List<Item>();
    public List<Item> consumables = new List<Item>();
    public Item itemToAdd;

    void Start()
    {
        //Lajittelee itemit omiin ryhmiin
        foreach(Item item in itemDatabase.items)
        {
            if(item.type == "Resource")
            {
                resources.Add(item);
            }
            else if ((item.type == "Drill") || (item.type == "Spacesuit") || (item.type == "ShipWeapon"))
            {
                equipment.Add(item);
            }
            else if (item.type == "Consumable")
            {
                consumables.Add(item);
            }

        }
        ShowAll();
    }

    public void ShowResources()
    {
        foreach(Item item in resources)
        {
            itemToAdd = item;
            Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
        }
    }
    public void ShowEquipment()
    {
        foreach (Item item in equipment)
        {
            itemToAdd = item;
            Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
        }
    }
    public void ShowConsumables()
    {
        foreach (Item item in consumables)
        {
            itemToAdd = item;
            Object prefab = Resources.Load("Prefabs/ItemCatalogItem");
            GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
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
        ResetCatalog();
        ShowResources();
        ShowEquipment();
        ShowConsumables();
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
