using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShipItemSlot : MonoBehaviour, IPointerClickHandler
{
    [field: SerializeField] public ShipItemSlotType Type { get; private set; }
    [SerializeField] Image icon;
    [SerializeField] TMP_Text itemName;

    [SerializeField] public ItemSO equippedItem { get; private set; }
    [SerializeField] ShipEquipment shipEquipment;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (CanUnEquip())
            {
                Unequip();
            }
        }
    }

    public void Equip(ItemSO item)
    {
        icon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        equippedItem = item;
    }

    public void Unequip()
    {
        shipEquipment.UnEquip(equippedItem, this);

        equippedItem = null;
        itemName.text = "";
        icon.sprite = null;
    }

    bool CanUnEquip()
    {
        switch (Type)
        {
            case ShipItemSlotType.Model:
            case ShipItemSlotType.Hull:
            case ShipItemSlotType.PrimaryWeapon:
                return false;
            case ShipItemSlotType.SecondaryWeapon:
            case ShipItemSlotType.Utility1:
            case ShipItemSlotType.Utility2:
                return true;
            default:
                return false;
        }
    }
}
