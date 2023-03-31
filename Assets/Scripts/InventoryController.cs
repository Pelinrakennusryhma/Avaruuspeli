using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject CanvasObject;

    public CanvasScript CanvasScript;

    public Inventory Inventory;
    public Equipment Equipment;
    public ShopNumberTwo Shop;
    public ShopHeadsUp ShopHeadsUp;

    public bool ShowingInventory;
    public bool IsInShoppingArea;

    CursorLockMode cachedCursorLockMode = CursorLockMode.None;

    public float Money = 10000.0f;
    public void Init()
    {
        //Item item;
        //item = CanvasScript.contextMenu.itemDatabase.GetItem(2);
        ////Inventory.RemoveItem(item.id, 1);
        //Equipment.EquipDrill(item);
        //Shop.Init();
        Money = 10000.0f;
        OnInventoryShow();

        Inventory.AddItem(2, 1);
        Inventory.AddItem(3, 1);
        Inventory.AddItem(4, 1);
        Inventory.AddItem(5, 1);

        Inventory.AddItem(6, 1);
        Inventory.AddItem(7, 1);
        Inventory.AddItem(8, 1);
        Inventory.AddItem(9, 1);
        Inventory.AddItem(10, 1);

        Shop.Init();
        ShopHeadsUp.Init();

        OnInventoryHide();
    }

    public void OnInventoryShow()
    {
        cachedCursorLockMode = Cursor.lockState;
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;
        ShowingInventory = true;
        CanvasObject.SetActive(true);
        AttachToMainCamera();

        CanvasScript.HideItemCatalog();
        CanvasScript.HideShop();
        CanvasScript.ShowEquipment();

        if (ResourceGatherer.Instance != null) 
        {
            ResourceGatherer.ToolType currentTool = ResourceGatherer.Instance.Tool;

            Item item;

            if (currentTool == ResourceGatherer.ToolType.BasicDrill)
            {
                item = CanvasScript.contextMenu.itemDatabase.GetItem(2);
                Inventory.RemoveItem(item.id, 1);
                Equipment.EquipDrill(item);
            }

            else if (currentTool == ResourceGatherer.ToolType.AdvancedDrill)
            {
                item = CanvasScript.contextMenu.itemDatabase.GetItem(4);
                Inventory.RemoveItem(item.id, 1);
                Equipment.EquipDrill(item);
            }
        }

        if (ResourceInventory.Instance != null)
        {
            ResourceInventory.Instance.UnloadGatheredItems(Inventory);
        }

        //Debug.Log("Show inventory");
    }

    public void OnInventoryHide()
    {
        Cursor.lockState = cachedCursorLockMode;
        Time.timeScale = 1.0f;
        ShowingInventory = false;
        CanvasObject.SetActive(false);
        DetachFromMainCamera();
        //Debug.Log("Hide inventory");
    }

    public void AttachToMainCamera()
    {
        CanvasObject.transform.SetParent(Camera.main.transform, false);
    }

    public void DetachFromMainCamera()
    {
        CanvasObject.transform.SetParent(transform, false);
    }

    public void OnDrill1Equipped()
    {
        //CanvasScript.
    }

    public ResourceGatherer.ToolType GetCurrentEquippedTool()
    {
        ResourceGatherer.ToolType currentTool = ResourceGatherer.ToolType.None;

        if (Equipment.equippedDrill != null)
        {
            if (Equipment.equippedDrill.id == 2)
            {
                currentTool = ResourceGatherer.ToolType.BasicDrill;
            }

            else if  (Equipment.equippedDrill.id == 2)
            {
                currentTool = ResourceGatherer.ToolType.AdvancedDrill;
            }

        }

        return currentTool;
    }

    public void OnEnterShoppingArea()
    {
        IsInShoppingArea = true;
        ResourceInventory.Instance.OnEnterShoppingArea();
    }

    public void OnExitShoppingArea()
    {
        IsInShoppingArea = false;
        ResourceInventory.Instance.OnExitShoppingArea();
    }

    public void Update()
    {
        if (IsInShoppingArea)
        {
            if (Input.GetKeyDown(KeyCode.E)) 
            {
                if (!ShowingInventory)
                {
                    OnEnterShop();
                }

                else
                {
                    OnExitShop();
                }

            }
        }
    }

    public void OnEnterShop()
    {
        Shop.Init();
        OnInventoryShow();
        CanvasScript.ShowEquipment();
        CanvasScript.HideItemCatalog();
        CanvasScript.ShowShop();
        ResourceInventory.Instance.OnStartShopping();

        bool useHeadsUpShop = true;

        if (useHeadsUpShop)
        {
            ShopHeadsUp.SetVendor(GameManager.Instance.CurrentPlanet.Vendor);
            CanvasScript.HideEquipment();
            CanvasScript.HideItemCatalog();
            CanvasScript.HideShop();
            CanvasScript.ShowHeadsUpShop();
        }
    }

    public void OnExitShop()
    {
        OnInventoryHide();
        CanvasScript.HideShop();
        CanvasScript.HideHeadsUpShop();
        ResourceInventory.Instance.OnEnterShoppingArea();
    }
}
