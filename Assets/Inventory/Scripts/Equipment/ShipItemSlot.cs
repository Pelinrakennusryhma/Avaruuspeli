using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipItemSlot : MonoBehaviour
{
    [field: SerializeField] public ShipItemSlotType Type { get; private set; }
    [SerializeField] Image icon;
    [SerializeField] TMP_Text itemName;

    [SerializeField] public ItemSO equippedItem { get; private set; }
    void Start()
    {
        
    }

    public void Equip(ItemSO item)
    {
        if(item.itemIcon != null)
        {
            icon.sprite = item.itemIcon;
        }
        itemName.text = item.itemName;
        equippedItem = item;
    }

    public void Unequip()
    {
        equippedItem = null;
        itemName.text = "";
        icon.sprite = null;
    }
}
