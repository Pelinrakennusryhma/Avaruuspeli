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
    [SerializeField] ContextMenu contextMenu;

    public void Equip(ItemSO item)
    {
        icon.sprite = item.itemIcon;
        itemName.text = item.itemName;
        equippedItem = item;
    }

    public void Unequip()
    {
        equippedItem = null;
        itemName.text = "";
        icon.sprite = null;
        contextMenu.HideAll();
    }

    bool CanUnEquip()
    {
        switch (Type)
        {
            case ShipItemSlotType.Model:
            case ShipItemSlotType.Hull:
            case ShipItemSlotType.PrimaryWeapon:
            case ShipItemSlotType.Thrusters:
                return false;
            case ShipItemSlotType.SecondaryWeapon:
            case ShipItemSlotType.Utility1:
            case ShipItemSlotType.Utility2:
                return true;
            default:
                return false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        shipEquipment.clickedSlot = this;
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            contextMenu.HideAll();

            if(equippedItem != null)
            {
                if (CanUnEquip())
                {
                    contextMenu.ShowUnEquipShipItem();

                    contextMenu.SetPositionToMouse();
                    contextMenu.itemID = equippedItem.id;
                }
            } 
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenu.HideMenu();
            //if (eventData.button == PointerEventData.InputButton.Left)
            //{
            //    GameObject[] gos = GameObject.FindGameObjectsWithTag("InfoPanel");
            //    foreach (GameObject go in gos)
            //    {
            //        Destroy(go);
            //    }
            //    FindObjectOfType<CanvasScript>().InfoAboutItem(itemToAdd);
            //    Object prefab = Resources.Load("Prefabs/ItemInfoPanel");
            //    GameObject newItem = Instantiate(prefab, CanvasScript.transform) as GameObject;
            //    newItem.name = itemToAdd.id.ToString();
            //    Vector3 mouseLocation = Input.mousePosition;
            //    newItem.transform.position = mouseLocation;
            //}
        }
    }
}
