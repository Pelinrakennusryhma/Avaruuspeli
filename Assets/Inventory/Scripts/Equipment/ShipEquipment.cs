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
    [SerializeField] SpaceshipData playerShipData; // used for storing data during runtime
    [SerializeField] SpaceshipData newGameShipData;
    [SerializeField] SpaceshipData devGameShipData;
    [SerializeField] Transform itemSlotsParent;
    [SerializeField] GameObject blockerPanel;
    [field: SerializeField]
    public Dictionary<ShipItemSlotType, ShipItemSlot> itemSlots 
    { get; private set; } = new Dictionary<ShipItemSlotType, ShipItemSlot>();

    // toggled between 0 and 1 when equipping utils
    int utilSlotId = 0;

    public ShipItemSlot clickedSlot;

    private void OnEnable()
    {
        if(Globals.Instance != null && Globals.Instance.IsSpaceshipScene())
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
        FillPlayerShipData();
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

    private SpaceshipData LoadData()
    {
        return newGameShipData;
        // TODO: Load from disk to PlayerShipData
    }

    private SpaceshipData PickShipData()
    {
        SpaceshipData data = null;
        switch (GameManager.LaunchType)
        {
            case GameManager.TypeOfLaunch.None:
                data = devGameShipData;
                break;
            case GameManager.TypeOfLaunch.NewGame:
                data = newGameShipData;
                break;
            case GameManager.TypeOfLaunch.LoadedGame:
                data = LoadData();
                break;
            case GameManager.TypeOfLaunch.DevGame:
                data = devGameShipData;
                break;
            default:
                data = devGameShipData;
                break;
        }

        return data;
    }

    private void FillPlayerShipData()
    {
        SpaceshipData data = PickShipData();
        playerShipData = Instantiate(data);
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
        if(item != null)
        {
            ShipItemSlot slot = GetItemSlot(item);
            slot.Equip(item);

            if (saveToShipData)
            {
                SaveToShipData(item, slot);
            }
        }
    }

    public void UnEquip()
    {
        clickedSlot.Unequip();
        SaveToShipData(null, clickedSlot);
        
    }

    void SaveToShipData(ItemSO item, ShipItemSlot slot)
    {
        switch (slot.Type)
        {
            case ShipItemSlotType.Model:
                playerShipData.shipModel = (ShipModel)item;
                break;
            case ShipItemSlotType.Hull:
                playerShipData.hull = (ShipHull)item;
                break;
            case ShipItemSlotType.PrimaryWeapon:
                playerShipData.primaryWeapon = (ShipWeaponItemPrimary)item;
                break;
            case ShipItemSlotType.SecondaryWeapon:
                playerShipData.secondaryWeapon = (ShipWeaponItemSecondary)item;
                break;
            case ShipItemSlotType.Utility1:
                playerShipData.utilities[0] = (ShipUtility)item;
                break;
            case ShipItemSlotType.Utility2:
                playerShipData.utilities[1] = (ShipUtility)item;
                break;
            default:
                throw new Exception("unknown ship item type, can't save to SpaceshipData");
        }

        int itemID = item == null ? -1 : item.id;

        GameManager.Instance.SaverLoader.SaveEquippedShipItem(itemID, (int)slot.Type);
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
            if(itemSlots[ShipItemSlotType.Utility1].equippedItem == null)
            {
                slot = itemSlots[ShipItemSlotType.Utility1];
            } else if(itemSlots[ShipItemSlotType.Utility2].equippedItem == null)
            {
                slot = itemSlots[ShipItemSlotType.Utility2];
            } else
            {
                if (utilSlotId == 0)
                {
                    slot = itemSlots[ShipItemSlotType.Utility1];
                }
                else
                {
                    slot = itemSlots[ShipItemSlotType.Utility2];
                }

                utilSlotId++;
                if (utilSlotId > 1)
                {
                    utilSlotId = 0;
                }
            }

        }
        else
        {
            throw new Exception("unknown ship item type");
        }

        return slot;
    }
}
