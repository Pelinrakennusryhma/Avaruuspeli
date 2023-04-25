using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item", order = 2)]
public class ItemSO : ScriptableObject
{
    public enum ItemType
    {
        None = 0,
        Consumable = 1,
        Drill = 2,
        Equipment = 3,
        PlayerWeapon = 4,
        Resource = 5,
        ShipItem = 6,
        ShipWeapon = 7,
        Fuel = 8
    }

    public int id;
    public string itemName;
    public string plural;
    public int value;
    public string description;
    public GameObject itemPrefab;
    public GameObject[] itemPrefabVariants;
    public Sprite itemIcon;
    public bool isStackable = false; // Do we even want non-stackable items at all?
    public double weight;

    public ItemType itemType;

    public virtual bool CheckIfDataBaseAlreadyContainsItem(ItemDataBaseSO dataBase)
    {
        return dataBase.CheckIfDataBaseAlreadyContainsItem(this);
    }

    public virtual void AddToDataBase(ItemDataBaseSO dataBase)
    {
        Debug.Log("Adding item to database. Object name is " + name + " item name is " + itemName);

        if (!dataBase.CheckIfDataBaseAlreadyContainsItem(this))
        {
            dataBase.AddToAllItems(this);
        }
    }

    public virtual void RemoveFromDataBase(ItemDataBaseSO dataBase)
    {
        dataBase.RemoveItemFromAllItems(this);
    }
}

