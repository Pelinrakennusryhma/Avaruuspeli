using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum ShipItemSlotType
{
    Model = 0,
    Hull = 1,
    PrimaryWeapon = 2,
    SecondaryWeapon = 3,
    Utility1 = 4,
    Utility2 = 5,
}

public class ShipEquipment : MonoBehaviour
{
    [SerializeField] SpaceshipData playerShipData;
    [SerializeField] Transform itemSlotsParent;
    [SerializeField] GameObject blockerPanel;
    [field: SerializeField]
    public Dictionary<ShipItemSlotType, ShipItemSlot> itemSlots 
    { get; private set; } = new Dictionary<ShipItemSlotType, ShipItemSlot>();

    int utilSlotId = 0;

    private void OnEnable()
    {
        // only enable if NOT in SpaceshipScene
        // TODO: disable actually equipping stuff somehow
        if(SceneManager.GetActiveScene().buildIndex == 1)
        {
            blockerPanel.SetActive(true);
        } else
        {
            blockerPanel.SetActive(false);
        }
    }

    public void Init()
    {
        InitSlots();
        LoadData();
        FillSlots();
    }

    private void InitSlots()
    {
        for (int i = 0; i < itemSlotsParent.childCount; i++)
        {
            Transform child = itemSlotsParent.GetChild(i);
            ShipItemSlot slot = child.GetComponent<ShipItemSlot>();
            itemSlots.Add(slot.Type, slot);
        }
    }

    private void LoadData()
    {
        // TODO: Load from disk to PlayerShipData
    }

    private void FillSlots()
    {
        Equip(playerShipData.shipModel, false);
        Equip(playerShipData.hull, false);
        Equip(playerShipData.primaryWeapon, false);
        Equip(playerShipData.secondaryWeapon, false);
        Equip(playerShipData.utilities[0], false);
        Equip(playerShipData.utilities[1], false);
    }

    public void Equip(ItemSO item, bool saveToShipData=true)
    {
        Debug.Log("Equipping ship item: " + item.itemName + "type: " + item.GetType());
        ShipItemSlot slot = GetItemSlot(item);
        slot.Equip(item);

        if (saveToShipData)
        {
            SaveToShipData(item, slot);
        }

        // TODO: Add to PlayerShipData
        // Save to disk application quit
    }

    void SaveToShipData(ItemSO item, ShipItemSlot slot)
    {
        if (slot.equippedItem is ShipModel)
        {
            playerShipData.shipModel = (ShipModel)slot.equippedItem;
        }
        else if (slot.equippedItem is ShipHull)
        {
            playerShipData.hull = (ShipHull)slot.equippedItem;
        }
        else if (slot.equippedItem is ShipWeaponItemPrimary)
        {
            playerShipData.primaryWeapon = (ShipWeaponItemPrimary)slot.equippedItem;
        }
        else if (slot.equippedItem is ShipWeaponItemSecondary)
        {
            playerShipData.secondaryWeapon = (ShipWeaponItemSecondary)slot.equippedItem;
        }
        else if (slot.equippedItem is ShipUtility)
        {
            if(slot.Type == ShipItemSlotType.Utility1)
            {
                playerShipData.utilities[0] = (ShipUtility)slot.equippedItem;
            } else
            {
                playerShipData.utilities[1] = (ShipUtility)slot.equippedItem;
            }
        }
        else
        {
            throw new Exception("unknown ship item type, can't save to SpaceshipData");
        }
    }

    ShipItemSlot GetItemSlot(ItemSO item)
    {
        ShipItemSlot slot;

        if (item is ShipModel)
        {
            slot = itemSlots[ShipItemSlotType.Model];
        }
        else if (item is ShipHull) 
        {
            slot = itemSlots[ShipItemSlotType.Hull];
        } 
        else if (item is ShipWeaponItemPrimary)
        {
            slot = itemSlots[ShipItemSlotType.PrimaryWeapon];
        }
        else if (item is ShipWeaponItemSecondary)
        {
            slot = itemSlots[ShipItemSlotType.SecondaryWeapon];
        }
        else if (item is ShipUtility)
        {
            if(utilSlotId == 0)
            {
                slot = itemSlots[ShipItemSlotType.Utility1];
            } else
            {
                slot = itemSlots[ShipItemSlotType.Utility2];
            }

            utilSlotId++;
            if(utilSlotId > 1)
            {
                utilSlotId = 0;
            }
        }
        else
        {
            throw new Exception("unknown ship item type");
        }

        return slot;
    }
}
