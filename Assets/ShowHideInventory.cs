using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHideInventory : MonoBehaviour
{
    public GameObject InventoryObject;
    public bool ShowingInventory;

    public Inventory Inventory;
    public CanvasScript CanvasScript;
    public Equipment Equipment;

    public static ShowHideInventory Instance;


    public void Awake()
    {
        Instance = this;
        ShowInventory(); // Inventory needs to be active to work
        Inventory.AddItem(2, 1);
        Inventory.AddItem(3, 1);
        Inventory.AddItem(4, 1);
        Inventory.AddItem(5, 1);
        HideInventory();
    }

    public void ShowInventory()
    {
        Time.timeScale = 0;
        InventoryObject.gameObject.SetActive(true);
        // Equip whichever drill is equipped
        CanvasScript.ShowEquipment();
        ShowingInventory = true;
    }

    public void HideInventory()
    {
        Time.timeScale = 1.0f;
        InventoryObject.gameObject.SetActive(false);
        CanvasScript.HideEquipment();
        ShowingInventory = false;
    }


}
