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
    public Item infoAbout;
    public ContextMenu contextMenu;
    //Pelaajan rahat
    public double money = 10000;

    public void Awake()
    {
        headsUpShop.gameObject.SetActive(false);
    }

    public void InfoAboutItem(Item item)
    {
        infoAbout = item;
    }
    public void ShowItemCatalog()
    {
        itemCatalogPanel.SetActive(true);
    }
    public void HideItemCatalog()
    {
        itemCatalogPanel.SetActive(false);
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
        HideEquipment();
        HideShop();
        HideItemCatalog();
        inventoryPanel.SetActive(false);
        headsUpShop.gameObject.SetActive(true);
        headsUpShop.Init();
        headsUpShop.StartShopping();
    }

    public void HideHeadsUpShop()
    {
        inventoryPanel.SetActive(true);
        ShowEquipment();
        headsUpShop.gameObject.SetActive(false);
    }
}
