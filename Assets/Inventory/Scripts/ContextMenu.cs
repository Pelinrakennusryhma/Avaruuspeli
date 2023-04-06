using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContextMenu : MonoBehaviour
{
    [SerializeField] private GameObject buttonUse;
    [SerializeField] private GameObject buttonEquip;
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
    public CanvasScript canvasScript;
    public bool shopping;

    //Piilottaa context menusta kaikki vaihtoehdot ja sitten näyttää kaikki itemin tyyppiin liittyvät vaihtoehdot. Näyttää 'Sell' jos pelaaja on kaupassa.
    public void ShowOptions(string type)
    {
        HideAll();
        if (type == "Resource")
        {
            ShowDiscard();
        }
        else if ((type == "Drill") || (type == "Spacesuit"))
        {
            ShowEquip();
            ShowDiscard();
        }
        else if(type == "ShipWeapon") {
            ShowEquip1();
            ShowEquip2();
            ShowDiscard();
        }
        else if(type == "Consumable")
        {
            ShowUse();
            ShowDiscard();
        }

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
        equipment.UnequipDrill();
        HideMenu();
    }
    public void UnequipSpacesuit()
    {
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

        Debug.LogError("We are doing checks with ids. Maybe we should do it with a TYPE. Maybe...");

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
    public void EquipDrill()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        inventory.RemoveItem(item.id, 1);
        equipment.EquipDrill(item);
        HideMenu();
    }
    public void EquipSpacesuit()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        inventory.RemoveItem(item.id, 1);
        equipment.EquipSpacesuit(item);
        HideMenu();
    }
    //Context menun 'Equip 1' vaihtoehto. 
    public void EquipShipWeapon1()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        inventory.RemoveItem(item.id, 1);
        equipment.EquipShipWeapon1(item);
        HideMenu();
    }
    //Context menun 'Equip 2' vaihtoehto. 
    public void EquipShipWeapon2()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
        inventory.RemoveItem(item.id, 1);
        equipment.EquipShipWeapon2(item);
        HideMenu();
    }

    //Context menun 'Equip' vaihtoehto. Equippaa pelaajalta itemin josta context menu on avattu. Ship weapon 1 ja 2 menevät suoraan oman napin kautta.
    public void EquipItem()
    {
        ItemSO item;
        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemID);
                    
        Debug.LogError("We are checking for an item with id. Maybe we could and should do the check with a TYPE");

        if (item.id == 7
            || item.id == 8)
        {

            EquipDrill();
        }
        else if(item.id == 12)
        {
            EquipSpacesuit();
        }
    }
    //Context menun 'Discard' vaihtoehto
    public void DiscardItem()
    {
        inventory.RemoveItem(itemID, 999999999);
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
        Debug.LogError("Replace the find with something else!!!");
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
}
