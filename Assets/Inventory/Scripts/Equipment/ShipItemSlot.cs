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

    ItemSO equipedItem;
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
        equipedItem = item;
    }

    public void Unequip()
    {
        equipedItem = null;
        itemName.text = "";
        icon.sprite = null;
    }
}
