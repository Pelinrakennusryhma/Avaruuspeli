using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewDatabase", menuName = "ScriptableObjects/ItemDatabase")]
public class ItemDataBaseSO : ScriptableObject
{
    #region ListsForEditorUse

    public List<ItemSO> AllItems;

    public List<ConsumableItemSO> ConsumableItems;
    public List<DrillItemSO> DrillItems;
    public List<EquipmentItemSO> EquipmentItems;
    public List<FuelItemSO> FuelItems;
    public List<PlayerWeaponItemSO> PlayerWeaponItems;
    public List<Resource> Resources;
    public List<ShipItemSO> ShipItems;
    public List<ShipWeaponItemSO> ShipWeaponItems;
    #endregion


    #region DictionariesForGameplayUse

    // key int is item id
    public Dictionary<int, ItemSO> AllItemsDict;
    public Dictionary<int, ConsumableItemSO> ConsumablesDict;
    public Dictionary<int, DrillItemSO> DrillItemsDict;
    public Dictionary<int, EquipmentItemSO> EquipmentItemsDict;
    public Dictionary<int, FuelItemSO> FuelItemsDict;
    public Dictionary<int, PlayerWeaponItemSO> PlayerWeaponItemsDict;
    public Dictionary<int, Resource> ResourcesDict;
    public Dictionary<int, ShipItemSO> ShipItemsDict;
    public Dictionary<int, ShipWeaponItemSO> ShipWeaponItemsDict;

    #endregion

    public bool HasBeenInitted = false;
    public Sprite defaultIcon;

    public void OnInit()
    {
        //Debug.Log("On Init is called on item data base SO");

        BuildDictionaries();
        HasBeenInitted = true;
    }

    private void BuildDictionaries()
    {
        AllItemsDict = new Dictionary<int, ItemSO>();
        ConsumablesDict = new Dictionary<int, ConsumableItemSO>();
        DrillItemsDict = new Dictionary<int, DrillItemSO>();
        EquipmentItemsDict = new Dictionary<int, EquipmentItemSO>();
        FuelItemsDict = new Dictionary<int, FuelItemSO>();
        PlayerWeaponItemsDict = new Dictionary<int, PlayerWeaponItemSO>();
        ResourcesDict = new Dictionary<int, Resource>();
        ShipItemsDict = new Dictionary<int, ShipItemSO>();
        ShipWeaponItemsDict = new Dictionary<int, ShipWeaponItemSO>();

        for (int i = 0; i < AllItems.Count; i++) 
        {
            AllItemsDict.Add(AllItems[i].id, AllItems[i]);
        }

        for (int i = 0; i < ConsumableItems.Count; i++)
        {
            ConsumablesDict.Add(ConsumableItems[i].id, ConsumableItems[i]);
        }

        for (int i = 0; i < DrillItems.Count; i++)
        {
            DrillItemsDict.Add(DrillItems[i].id, DrillItems[i]);
        }

        for (int i = 0; i < EquipmentItems.Count; i++)
        {
            EquipmentItemsDict.Add(EquipmentItems[i].id, EquipmentItems[i]);
        }

        for (int i = 0; i < FuelItems.Count; i++)
        {
            FuelItemsDict.Add(FuelItems[i].id, FuelItems[i]);
        }

        for (int i = 0; i < PlayerWeaponItems.Count; i++)
        {
            PlayerWeaponItemsDict.Add(PlayerWeaponItems[i].id, PlayerWeaponItems[i]);
        }

        for (int i = 0; i < Resources.Count; i++)
        {
            ResourcesDict.Add(Resources[i].id, Resources[i]);
        }

        for (int i = 0; i < ShipItems.Count; i++)
        {
            ShipItemsDict.Add(ShipItems[i].id, ShipItems[i]);
        }

        for (int i = 0; i < ShipWeaponItems.Count; i++)
        {
            ShipWeaponItemsDict.Add(ShipWeaponItems[i].id, ShipWeaponItems[i]);
        }

        #region TestLogs
        // Test logs
        //foreach (KeyValuePair<int, ItemSO> kvp in AllItemsDict)
        //{
        //    Debug.Log("added to all items dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach (KeyValuePair<int, ConsumableItemSO> kvp in ConsumablesDict)
        //{
        //    Debug.Log("added to consumables dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach (KeyValuePair<int, DrillItemSO> kvp in DrillItemsDict)
        //{
        //    Debug.Log("added to drill items dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach (KeyValuePair<int, EquipmentItemSO> kvp in EquipmentItemsDict)
        //{
        //    Debug.Log("added to equipment items dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach (KeyValuePair<int, FuelItemSO> kvp in FuelItemsDict)
        //{
        //    Debug.Log("added to fuel items dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach(KeyValuePair<int, PlayerWeaponItemSO> kvp in PlayerWeaponItemsDict)
        //{
        //    Debug.Log("added to player weapon items dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach (KeyValuePair<int, Resource> kvp in ResourcesDict)
        //{
        //    Debug.Log("added to resources dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach (KeyValuePair<int, ShipItemSO> kvp in ShipItemsDict)
        //{
        //    Debug.Log("added to ship items dict " + kvp.Key + " name " + kvp.Value);
        //}

        //foreach (KeyValuePair<int, ShipWeaponItemSO> kvp in ShipWeaponItemsDict)
        //{
        //    Debug.Log("added to ship weapon items dict " + kvp.Key + " name " + kvp.Value);
        //}

        #endregion

    }

    #region Getters
    
    public ItemSO GetItem(int id)
    {
        ItemSO item = null;

        if (AllItemsDict == null)
        {
            Debug.LogError("Null all items dict");
        }

        AllItemsDict.TryGetValue(id, out item);

        if (item == null) 
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public ConsumableItemSO GetConsumableItem(int id)
    {
        ConsumableItemSO item = null;

        ConsumablesDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public DrillItemSO GetDrillItem(int id)
    {
        DrillItemSO item = null;

        DrillItemsDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public EquipmentItemSO GetEquipmentItem(int id)
    {
        EquipmentItemSO item = null;

        EquipmentItemsDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public FuelItemSO GetFuelItem(int id)
    {
        FuelItemSO item = null;

        FuelItemsDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public PlayerWeaponItemSO GetPlayerWeaponItem(int id)
    {
        PlayerWeaponItemSO item = null;

        PlayerWeaponItemsDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public Resource GetResourceItem(int id)
    {
        Resource item = null;

        ResourcesDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public ShipItemSO GetShipItem(int id)
    {
        ShipItemSO item = null;

        ShipItemsDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    public ShipWeaponItemSO GetShipWeapon(int id)
    {
        ShipWeaponItemSO item = null;

        ShipWeaponItemsDict.TryGetValue(id, out item);

        if (item == null)
        {
            Debug.LogError("Returning null item. Probably there's a problem with id " + id);
        }

        return item;
    }

    #endregion

    #region EditorAddRemoveAndCheck
    public void AddToAllItems(ItemSO item)
    {
        if (!AllItems.Contains(item))
        {
            AllItems.Add(item);
        }

        TryAddDefaultIcon(item);

        Debug.Log("Should add to all items. Object name is " + item.name + " Item name is " + item.itemName);
    }

    void TryAddDefaultIcon(ItemSO item)
    {
        if (item.itemIcon == null && defaultIcon != null)
        {
            item.itemIcon = defaultIcon;
        }
    }

    public void AddToConsumables(ConsumableItemSO item)
    {
        if (!ConsumableItems.Contains(item))
        {
            ConsumableItems.Add(item);
        }

        Debug.Log("Should add to consumables. Object name is " + item.name + " Item name is " + item.itemName);
    }

    public void AddToDrills(DrillItemSO item)
    {
        if (!DrillItems.Contains(item))
        {
            DrillItems.Add(item);
        }

        Debug.Log("Should add to drills. Object name is " + item.name + " Item name is " + item.itemName);
    }

    public void AddToEquipment(EquipmentItemSO item)
    {
        if (!EquipmentItems.Contains(item))
        {
            EquipmentItems.Add(item);
        }

        Debug.Log("Should add to equipment. Object name is " + item.name + " Item name is " + item.itemName);
    }

    public void AddToFuel(FuelItemSO item)
    {
        if (!FuelItems.Contains(item))
        {
            FuelItems.Add(item);
        }

        Debug.Log("Should add to fuel. Object name is " + item.name + " Item name is " + item.itemName);
    }

    public void AddToPlayerWeapons(PlayerWeaponItemSO item)
    {
        if (!PlayerWeaponItems.Contains(item))
        {
            PlayerWeaponItems.Add(item);
        }

        Debug.Log("Should add to player weapons. Object name is " + item.name + " Item name is " + item.itemName);
    }

    public void AddToResources(Resource item)
    {
        if (!Resources.Contains(item))
        {
            Resources.Add(item);
        }

        Debug.Log("Should add to resources. Object name is " + item.name + " Item name is " + item.itemName);
    }

    public void AddToShipItems(ShipItemSO item)
    {
        if (!ShipItems.Contains(item))
        {
            ShipItems.Add(item);
        }

        Debug.Log("Should add to ship items. Object name is " + item.name + " Item name is " + item.itemName);
    }

    public void AddToShipWeapons(ShipWeaponItemSO item)
    {
        if (!ShipWeaponItems.Contains(item))
        {
            ShipWeaponItems.Add(item);
        }

        Debug.Log("Should add to ship weapons. Object name is " + item.name + " Item name is " + item.itemName);
    }


    // NOTE: THERE PROBABLY IS AN APPROACH WITH SOME GENERIC METHOD WITH TYPE,
    // BUT I AM NOT FAMILIAR WITH THAT AND DON'T KNOW WHAT TO SEARCH FOR
    // Also: we have to check different arrays anyways.
    public bool CheckIfDataBaseAlreadyContainsItem(ItemSO item)
    {
        if (AllItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool CheckConsumables(ConsumableItemSO item)
    {
        Debug.Log("Should CHECK consumables. Object name is " + item.name + " Item name is " + item.itemName);

        if (ConsumableItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }


    }

    public bool CheckDrills(DrillItemSO item)
    {
        Debug.Log("Should CHECK drills. Object name is " + item.name + " Item name is " + item.itemName);

        if (DrillItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }


    }

    public bool CheckEquipment(EquipmentItemSO item)
    {
        Debug.Log("Should CHECK equipment. Object name is " + item.name + " Item name is " + item.itemName);

        if (EquipmentItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }


    }

    public bool CheckFuel(FuelItemSO item)
    {
        Debug.Log("Should CHECK fuel. Object name is " + item.name + " Item name is " + item.itemName);

        if (FuelItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }


    }

    public bool CheckPlayerWeapons(PlayerWeaponItemSO item)
    {
        Debug.Log("Should CHECK player weapons. Object name is " + item.name + " Item name is " + item.itemName);
        
        if (PlayerWeaponItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }


    }

    public bool CheckResources(Resource item)
    {
        Debug.Log("Should CHECK resources. Object name is " + item.name + " Item name is " + item.itemName);
        
        if (Resources.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }


    }

    public bool CheckShipItems(ShipItemSO item)
    {        
        Debug.Log("Should CHECK ship items. Object name is " + item.name + " Item name is " + item.itemName);
        
        if (ShipItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }


    }

    public bool CheckShipWeapons(ShipWeaponItemSO item)
    {        
        Debug.Log("Should CHECK  ship weapons. Object name is " + item.name + " Item name is " + item.itemName);
        
        if (ShipWeaponItems.Contains(item))
        {
            return true;
        }

        else
        {
            return false;
        }

    }

    public void RemoveItemFromAllItems(ItemSO item)
    {
        Debug.Log("REMOVE from all items. Object name is " + item.name + " Item name is " + item.itemName);

        AllItems.Remove(item);
    }

    public void RemoveFromConsumables(ConsumableItemSO item)
    {
        Debug.Log("REMOVE from consumables. Object name is " + item.name + " Item name is " + item.itemName);

        ConsumableItems.Remove(item);
    }

    public void RemoveFromDrills(DrillItemSO item)
    {
        Debug.Log("REMOVE from drills. Object name is " + item.name + " Item name is " + item.itemName);

        DrillItems.Remove(item);
    }

    public void RemoveFromEquipment(EquipmentItemSO item)
    {
        Debug.Log("REMOVE from equipment. Object name is " + item.name + " Item name is " + item.itemName);

        EquipmentItems.Remove(item);
    }

    public void RemoveFromFuel(FuelItemSO item)
    {
        Debug.Log("REMOVE from fuel. Object name is " + item.name + " Item name is " + item.itemName);

        FuelItems.Remove(item);
    }

    public void RemoveFromPlayerWeapons(PlayerWeaponItemSO item)
    {
        Debug.Log("REMOVE from player weapons. Object name is " + item.name + " Item name is " + item.itemName);

        PlayerWeaponItems.Remove(item);
    }

    public void RemoveFromResources(Resource item)
    {
        Debug.Log("REMOVE from resources. Object name is " + item.name + " Item name is " + item.itemName);

        Resources.Remove(item);
    }

    public void RemoveFromShipItems(ShipItemSO item)
    {
        Debug.Log("REMOVE from ship items. Object name is " + item.name + " Item name is " + item.itemName);

        ShipItems.Remove(item);
    }

    public void RemoveFromShipWeapons(ShipWeaponItemSO item)
    {
        Debug.Log("REMOVE from ship weapons. Object name is " + item.name + " Item name is " + item.itemName);

        ShipWeaponItems.Remove(item);
    }

    #endregion
}
