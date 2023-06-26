using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonUse;
    [SerializeField] private GameObject buttonEquip;
    [SerializeField] private Button buttonEquipShipItem;
    [SerializeField] private Button buttonUnEquipShipItem;
    [SerializeField] private GameObject buttonUnequip1;
    [SerializeField] private GameObject buttonUnequip2;
    [SerializeField] private GameObject buttonEquip1;
    [SerializeField] private GameObject buttonEquip2;
    [SerializeField] private GameObject buttonUnequip;
    [SerializeField] private GameObject buttonSell;
    [SerializeField] private GameObject buttonDiscard;
    public int itemID;
    public Inventory inventory;
    //public ItemDatabase itemDatabase;
    public Equipment equipment;
    [SerializeField] private ShipEquipment shipEquipment;
    public CanvasScript canvasScript;
    public bool shopping;

    //Piilottaa context menusta kaikki vaihtoehdot ja sitten näyttää kaikki itemin tyyppiin liittyvät vaihtoehdot. Näyttää 'Sell' jos pelaaja on kaupassa.
    public void ShowOptions(ItemSO.ItemType  itemType,
                            int itemId)
    {
        this.itemID = itemId;

        HideContextMenu.FindAndDestroyInfoPanels();
        HideAll();

        //Debug.LogWarning("The string searches have been replaced with enum use.");

        switch (itemType)
        {
            case ItemSO.ItemType.None:
                ShowDiscard();
                break;
            case ItemSO.ItemType.Consumable:
                ShowUse();
                ShowDiscard();
                break;

            case ItemSO.ItemType.Drill:
                ShowEquip();
                ShowDiscard();
                break;

            case ItemSO.ItemType.Equipment:

                //Debug.Log("Item id is " + itemID);

                if (itemID == 29) 
                {
                    ShowUse(); // ISRU MODULE
                }

                else if (itemID == 32)
                {
                    ShowUse(); // Hydroponics bay
                }

                else
                {
                    ShowEquip();
                }

                ShowDiscard();
                break;

            case ItemSO.ItemType.PlayerWeapon:
                //Debug.LogError("Missing functionality: player weapons can't yet be equipped in any way.");
                ShowEquip();
                ShowDiscard();
                break;
            case ItemSO.ItemType.Resource:
                ShowDiscard();
                break;
            case ItemSO.ItemType.ShipItem:
                ShowEquipShipItem();
                ShowDiscard();
                break;
            case ItemSO.ItemType.ShipWeapon:
                ShowEquipShipItem();
                ShowDiscard();
                break;
            case ItemSO.ItemType.Fuel:
                //Debug.LogError("Missing functionality: fuels are missing context menus.");
                ShowDiscard();
                break;
            default:
                break;
        }

        //if (type == "Resource")
        //{
        //    ShowDiscard();
        //}
        //else if ((type == "Drill") || (type == "Spacesuit"))
        //{
        //    ShowEquip();
        //    ShowDiscard();
        //}
        //else if(type == "ShipWeapon") {
        //    ShowEquip1();
        //    ShowEquip2();
        //    ShowDiscard();
        //}
        //else if(type == "Consumable")
        //{
        //    ShowUse();
        //    ShowDiscard();
        //}

        if (shopping == true)
        {
            ShowSell();
        }
    }

    //Tuo context menun hiiren luokse
    public void SetPositionToMouse()
    {
        Vector3 mouseLocation = Input.mousePosition;
        gameObject.transform.position = mouseLocation;
    }

    //Piilottaa context menun pois näkyvistä
    public void HideMenu()
    {
        gameObject.transform.position = new Vector3(-1000, 1000, 0);
    }
    public void UnequipDrill()
    {
        equipment.UnequipObjectInHands();
        HideMenu();
    }
    public void UnequipSpacesuit()
    {
        Debug.LogWarning("Unequipping spacesuit");
        equipment.UnequipSpacesuit();
        HideMenu();
    }
    //Context menun 'Unequip' (2/3) vaihtoehto. 
    public void UnequipShipWeapon1()
    {
        equipment.UnequipShipWeapon1();
        HideMenu();
    }
    //Context menun 'Unequip' (3/3) vaihtoehto. 
    public void UnequipShipWeapon2()
    {
        equipment.UnequipShipWeapon2();
        HideMenu();
    }

    //Context menun 'Unequip' (1/3) vaihtoehto. Unequippaa pelaajalta itemin josta context menu on avattu. Ship weapon 1 ja 2 menevät suoraan oman napin kautta.
    public void UnequipItem()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);

        Debug.LogError("Unequipping item with id " + itemID);

        if (item.id == 7
            || item.id == 8)
        {
            UnequipDrill();
        }
        else if (item.id == 12)
        {
            UnequipSpacesuit();
        }
    }
    public void EquipDrillOrWeapon()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        //inventory.RemoveItem(item.id, 1);
        //Debug.LogWarning("Don't add and remove drill from inventory during equip/unequip?");

        equipment.EquipObjectInHands(item);
        HideMenu();
    }
    public void EquipSpacesuit()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        //inventory.RemoveItem(item.id, 1);
        //Debug.LogWarning("Don't add and remove spacesuit during equip/unequip");
        equipment.EquipSpacesuit(item);
        HideMenu();
    }
    //Context menun 'Equip 1' vaihtoehto. 
    public void EquipShipWeapon1()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        inventory.RemoveItem(item.id, 1);
        Debug.LogWarning("We are adding and removing ship weapon from invenotry during equip/unequip. This is old functionality and should be made consistent with the rest of items.");
        equipment.EquipShipWeapon1(item);
        HideMenu();
    }
    //Context menun 'Equip 2' vaihtoehto. 
    public void EquipShipWeapon2()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        inventory.RemoveItem(item.id, 1);
        Debug.LogWarning("We are adding and removing ship weapon from invenotry during equip/unequip. This is old functionality and should be made consistent with the rest of items.");

        equipment.EquipShipWeapon2(item);
        HideMenu();
    }

    //Context menun 'Equip' vaihtoehto. Equippaa pelaajalta itemin josta context menu on avattu. Ship weapon 1 ja 2 menevät suoraan oman napin kautta.
    public void EquipItem()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
                    
        if (item.id == 7
            || item.id == 8
            || item.id == 9
            || item.id == 21
            || item.id == 22)
        {

            EquipDrillOrWeapon();
        }
        else if(item.id == 12)
        {
            EquipSpacesuit();
        }
    }

    public void EquipShipItem()
    {
        ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        shipEquipment.Equip(item);
        HideMenu();
    }
    //Context menun 'Discard' vaihtoehto
    public void DiscardItem()
    {
        ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        Debug.LogWarning("Checking that we don't discard an equipped item!!");

        bool isAnEquippedItem = false;

        if (GameManager.Instance.InventoryController.Equipment.CheckIfAnItemIsEquipped(item))
        {
            isAnEquippedItem = true;
            Debug.LogWarning("Tried to discard an equipped item, but we won't allow that for now.");
        }

        else
        {
            Debug.LogWarning("Item wasn't equipped so we can discard it safely");
        }

        if (!isAnEquippedItem) 
        {
            if (itemID == 13)
            {
                GameManager.Instance.LifeSupportSystem.UpdateOxygenBottles(0);
            }

            else if (itemID == 16)
            {
                GameManager.Instance.ShipLifeSupportSystem.ZeroOutOxygenStorages();
            }

            else if (itemID == 31)
            {
                GameManager.Instance.ShipLifeSupportSystem.ZeroOutWaterBottles();
            }

            else if (itemID == 30)
            {
                GameManager.Instance.ShipLifeSupportSystem.ZeroOutCarbon();
            }

            else if (itemID == 14)
            {
                MotherShipOnWorldMapController.Instance.FuelSystem.UpdateWarpdriveFuelTankAmount(0);
            }

            else if (itemID == 15)
            {
                MotherShipOnWorldMapController.Instance.FuelSystem.UpdateRocketFuelTankAmount(0);
            }

            inventory.RemoveItem(itemID, 999999999);
        }

        HideMenu();
    }

    //Context menun 'Sell' vaihtoehto
    public void SellItem()
    {
        ItemScript itemScript;
        ItemSO item;
        item = inventory.CheckForItem(itemID);
        itemScript = FindCorrectItemScript();

        canvasScript.money += item.value * itemScript.currentItemAmount;
        inventory.RemoveItem(itemID, itemScript.currentItemAmount);
        HideMenu();
    }

    public ItemScript FindCorrectItemScript()
    {        
        Debug.LogError("Replace the find with something else!!! But these are dynamically created, " +
                       "so some thought is required from where to fetch this. But also, it is not " +
                       "possible to shop anymore with inventory open, so you should never even see this message!");

        return GameObject.Find("InventoryPanel/Scroll/View/Layout/" + itemID).GetComponent<ItemScript>();        
    }

    //Tuo näkyville tai piilottaa näkyvistä eri nappeja context menusta
    public void ShowUse()
    {
        buttonUse.SetActive(true);
    }
    public void HideUse()
    {
        buttonUse.SetActive(false);
    }
    public void ShowEquip()
    {
        buttonEquip.SetActive(true);
    }
    public void HideEquip()
    {
        buttonEquip.SetActive(false);
    }
    public void ShowEquipShipItem()
    {
        buttonEquipShipItem.gameObject.SetActive(true);
        if(Globals.Instance.IsSpaceshipScene())
        {
            buttonEquipShipItem.interactable = false;
        } else
        {
            buttonEquipShipItem.interactable = true;
        }
    }
    public void HideEquipShipItem()
    {
        buttonEquipShipItem.gameObject.SetActive(false);
    }
    public void ShowUnEquipShipItem()
    {
        buttonUnEquipShipItem.gameObject.SetActive(true);
        if (Globals.Instance.IsSpaceshipScene())
        {
            buttonUnEquipShipItem.interactable = false;
        }
        else
        {
            buttonUnEquipShipItem.interactable = true;
        }
    }

    public void HideUnEquipShipItem()
    {
        buttonUnEquipShipItem.gameObject.SetActive(false);
    }
    public void ShowEquip1()
    {
        buttonEquip1.SetActive(true);
    }
    public void HideEquip1()
    {
        buttonEquip1.SetActive(false);
    }
    public void ShowEquip2()
    {
        buttonEquip2.SetActive(true);
    }
    public void HideEquip2()
    {
        buttonEquip2.SetActive(false);
    }
    public void ShowUnequip1()
    {
        buttonUnequip1.SetActive(true);
    }
    public void HideUnequip1()
    {
        buttonUnequip1.SetActive(false);
    }
    public void ShowUnequip2()
    {
        buttonUnequip2.SetActive(true);
    }
    public void HideUnequip2()
    {
        buttonUnequip2.SetActive(false);
    }
    public void ShowUnequip()
    {
        buttonUnequip.SetActive(true);
    }
    public void HideUnequip()
    {
        buttonUnequip.SetActive(false);
    }
    public void ShowSell()
    {
        buttonSell.SetActive(true);
    }
    public void HideSell()
    {
        buttonSell.SetActive(false);
    }
    public void ShowDiscard()
    {
        buttonDiscard.SetActive(true);
    }
    public void HideDiscard()
    {
        buttonDiscard.SetActive(false);
    }
    //Näyttää kaikki napit
    public void ShowAll()
    {
        ShowUse();
        ShowEquip();
        ShowEquipShipItem();
        ShowUnEquipShipItem();
        ShowEquip1();
        ShowEquip2();
        ShowUnequip();
        ShowUnequip1();
        ShowUnequip2();
        ShowSell();
        ShowDiscard();
    }
    //Piilottaa kaikki napit
    public void HideAll()
    {
        HideUse();
        HideEquip();
        HideEquipShipItem();
        HideUnEquipShipItem();
        HideEquip1();
        HideEquip2();
        HideUnequip();
        HideUnequip1();
        HideUnequip2();
        HideSell();
        HideDiscard();
    }

    //Vaihtaa onko pelaaja kaupassa vai ei. 'Sell' napin näyttämistä tai piilottamista varten.
    public void ToggleShopping()
    {
        shopping = !shopping;
    }
    public void ShoppingOn()
    {
        shopping = true;
    }
    public void ShoppingOff()
    {
        shopping = false;
    }

    public void OnUseClicked()
    {
        if (itemID == 29)
        {
            GameManager.Instance.InventoryController.OnISRUShow();
        }

        else if (itemID == 32)
        {
            GameManager.Instance.InventoryController.OnHydroponicsBayShow();
        }

        if (itemID == 20)
        {
            if (GameManager.Instance.HungerTracker.CheckIfCanEatASandwich())
            {
                GameManager.Instance.HungerTracker.OnEatSandwich();
            }
        }


        HideAll();
        //Debug.Log("Use clicked");
    }
}
