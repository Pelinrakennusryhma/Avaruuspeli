using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

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
    [SerializeField] Transform itemSlotsParent;
    [field: SerializeField]
    public Dictionary<ShipItemSlotType, ShipItemSlot> itemSlots 
    { get; private set; } = new Dictionary<ShipItemSlotType, ShipItemSlot>();

    int utilSlotId = 0;

    private void Awake()
    {
        for (int i = 0; i < itemSlotsParent.childCount; i++)
        {
            Transform child = itemSlotsParent.GetChild(i);
            ShipItemSlot slot = child.GetComponent<ShipItemSlot>();
            itemSlots.Add(slot.Type, slot);
        }
        // TODO: Load from disk to PlayerShipData
        // Load from PlayershipData to Slots
    }

    public void Equip(ItemSO item)
    {
        Debug.Log("Equipping ship item: " + item.itemName + "type: " + item.GetType());
        ShipItemSlot slot = GetItemSlot(item);
        slot.Equip(item);

        // TODO: Add to PlayerShipData
        // Save to disk application quit
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
