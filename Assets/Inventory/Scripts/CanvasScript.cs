using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasScript : MonoBehaviour
{
    [SerializeField] private GameObject itemCatalogPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject equipmentPanel;
    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private ShopHeadsUp headsUpShop;
    public ItemSO infoAbout;
    public ContextMenu contextMenu;
    //Pelaajan rahat
    public double money = 10000;

    public void Awake()
    {
        headsUpShop.gameObject.SetActive(false);
    }

    public void InfoAboutItem(ItemSO item)
    {
        infoAbout = item;
    }
    public void ShowItemCatalog()
    {
        HideHeadsUpShop(false);
        inventoryPanel.SetActive(false);
        HideEquipment();
        itemCatalogPanel.SetActive(true);
    }
    public void HideItemCatalog()
    {
        itemCatalogPanel.SetActive(false);
        inventoryPanel.SetActive(true);
        ShowEquipment();
    }

    public void ShowShop()
    {
        shopPanel.SetActive(true);
        contextMenu.ShoppingOn();
    }
    public void HideShop()
    {
        shopPanel.SetActive(false);
        contextMenu.ShoppingOff();
    }
    public void ShowEquipment()
    {
        equipmentPanel.SetActive(true);
    }
    public void HideEquipment()
    {
        equipmentPanel.SetActive(false);
    }

    public void ShowHeadsUpShop()
    {
        //Debug.Log("Show heads up shop called");

        HideShop();
        HideItemCatalog();        
        HideEquipment();
        inventoryPanel.SetActive(false);
        headsUpShop.gameObject.SetActive(true);
        headsUpShop.Init();
        headsUpShop.StartShopping();
    }

    public void HideHeadsUpShop(bool finishShopping)
    {
       // Debug.Log("Hide heads up shop called");

        if (finishShopping) 
        {
            headsUpShop.FinishShopping();
            //Debug.LogError("Finishing shopping");
        }

        else
        {
            //Debug.LogError("We are not shopping anymore");
        }

        inventoryPanel.SetActive(true);
        ShowEquipment();
        headsUpShop.gameObject.SetActive(false);


        
    }
}
