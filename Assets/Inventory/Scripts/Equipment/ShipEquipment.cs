using System;
using System.Collections.Generic;
using UnityEngine;

public enum ShipItemSlotType
{
    Model = 0,
    Hull = 1,
    Thrusters = 2,
    PrimaryWeapon = 3,
    SecondaryWeapon = 4,
    Utility1 = 5,
    Utility2 = 6
}

public class ShipEquipment : MonoBehaviour
{
    [SerializeField] SpaceshipData playerShipData; // used for storing data during runtime
    [SerializeField] SpaceshipData newGameShipData;
    [SerializeField] SpaceshipData devGameShipData;
    [SerializeField] Inventory inventory;
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
        int[] savedData = GameManager.Instance.SaverLoader.LoadEquippedShipItems();
        SpaceshipData data = Instantiate(playerShipData);

        ItemDataBaseSO db = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO;

        // -1 is used for null purposes when saving to disk
        data.shipModel       = savedData[0] > 0 ? db.GetItem(savedData[0]) as ShipModel : null;
        data.hull            = savedData[1] > 0 ? db.GetItem(savedData[1]) as ShipHull : null;
        data.thrusters       = savedData[2] > 0 ? db.GetItem(savedData[2]) as ShipThrusters : null;
        data.primaryWeapon   = savedData[3] > 0 ? db.GetItem(savedData[3]) as ShipWeaponItemPrimary : null;
        data.secondaryWeapon = savedData[4] > 0 ? db.GetItem(savedData[4]) as ShipWeaponItemSecondary : null;
        data.utilities[0]    = savedData[5] > 0 ? db.GetItem(savedData[5]) as ShipUtility : null;
        data.utilities[1]    = savedData[6] > 0 ? db.GetItem(savedData[6]) as ShipUtility : null;

        return data;
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
        playerShipData.shipModel = data.shipModel;
        playerShipData.hull = data.hull;
        playerShipData.thrusters = data.thrusters;
        playerShipData.primaryWeapon = data.primaryWeapon;
        playerShipData.secondaryWeapon = data.secondaryWeapon;
        playerShipData.utilities[0] = data.utilities[0];
        playerShipData.utilities[1] = data.utilities[1];
    }

    private void FillSlots()
    {
        Equip(playerShipData.shipModel);
        Equip(playerShipData.hull);
        Equip(playerShipData.thrusters);
        Equip(playerShipData.primaryWeapon);
        Equip(playerShipData.secondaryWeapon);
        Equip(playerShipData.utilities[0]);
        Equip(playerShipData.utilities[1]);
    }

    public void Equip(ItemSO item)
    {
        if(item != null)
        {
            ShipItemSlot slot = GetItemSlot(item);
            slot.Equip(item);

            if (inventory.CheckForItem(item.id))
            {
                inventory.RemoveItem(item.id, 1);
            }

            SaveToShipData(item, slot);
            SaveToDisk(item, slot);
        }
    }

    public void UnEquip()
    {
        inventory.AddItem(clickedSlot.equippedItem.id, 1);
        clickedSlot.Unequip();

        SaveToShipData(null, clickedSlot);
        SaveToDisk(null, clickedSlot);
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
            case ShipItemSlotType.Thrusters:
                playerShipData.thrusters = (ShipThrusters)item;
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
    }
    private void SaveToDisk(ItemSO item, ShipItemSlot slot)
    {
        int itemID = item == null ? -1 : item.id;

        GameManager.Instance.SaverLoader.SaveEquippedShipItem(itemID, (int)slot.Type);
        GameManager.Instance.SaverLoader.SaveInventory(inventory.InventoryItemScripts);
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
        else if (item is ShipThrusters)
        {
            slot = itemSlots[ShipItemSlotType.Thrusters];
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
