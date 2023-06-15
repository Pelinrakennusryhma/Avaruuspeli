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
    Dictionary<ShipItemSlotType, ShipItemSlot> itemSlots = new Dictionary<ShipItemSlotType, ShipItemSlot>();

    private void Start()
    {
        for (int i = 0; i < itemSlotsParent.childCount; i++)
        {
            Transform child = itemSlotsParent.GetChild(i);
            ShipItemSlot slot = child.GetComponent<ShipItemSlot>();
            itemSlots.Add(slot.Type, slot);
        }
    }
    public void Equip(ItemSO item)
    {
        Debug.Log("Equipping ship item: " + item.itemName + "type: " + item.GetType());
        ShipItemSlot slot = GetItemSlot(item);
        slot.Equip(item);
    }

    ShipItemSlot GetItemSlot(ItemSO item)
    {
        Type itemType = item.GetType();

        ShipItemSlot slot;

        if (itemType == typeof(ShipModel))
        {
            slot = itemSlots[ShipItemSlotType.Model];
        }
        else if (itemType == typeof(ShipHull)) 
        {
            slot = itemSlots[ShipItemSlotType.Hull];
        } 
        else if (itemType == typeof(ShipWeaponItemPrimary))
        {
            slot = itemSlots[ShipItemSlotType.PrimaryWeapon];
        }
        else
        {
            throw new Exception("unknown ship item type");
        }

        return slot;
    }
}
