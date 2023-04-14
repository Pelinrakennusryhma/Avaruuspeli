using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject CanvasObject;

    public CanvasScript CanvasScript;

    public ContextMenu ContextMenuScript;
    public ItemCatalog ItemCatalog;

    public ItemDataBaseWithScriptables ItemDataBaseWithScriptables;
    public Inventory Inventory;
    public Equipment Equipment;
    public ShopNumberTwo Shop;
    public ShopHeadsUp ShopHeadsUp;

    public bool ShowingInventory;
    public bool IsInShoppingArea;
    public bool IsShopping;

    public Sprite BlankSprite;

    CursorLockMode cachedCursorLockMode = CursorLockMode.None;

    public float Money = 10000.0f;
    public void Init()
    {        
        ItemDataBaseWithScriptables.Init();
        Inventory.OnInventoryControllerInit();
        //Item item;
        //item = CanvasScript.contextMenu.itemDatabase.GetItem(2);
        ////Inventory.RemoveItem(item.id, 1);
        //Equipment.EquipDrill(item);
        //Shop.Init();

        //ItemDataBaseWithScriptables = GetComponentInChildren<ItemDataBaseWithScriptables>(true);

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
        Inventory.AddItem(11, 1);
        Inventory.AddItem(12, 1);
        Inventory.AddItem(13, 1);
        Inventory.AddItem(14, 1);
        Inventory.AddItem(15, 1);
        Inventory.AddItem(16, 1);
        Inventory.AddItem(17, 1);
        Inventory.AddItem(18, 1);
        Inventory.AddItem(19, 1);
        Inventory.AddItem(20, 1);

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

            ItemSO item;

            if (currentTool == ResourceGatherer.ToolType.BasicDrill)
            {
                item = ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(7);
                //Inventory.RemoveItem(item.id, 1);
                Debug.LogWarning("Don't add and remove drill from inventory during equip/unequip?");

                Equipment.EquipDrill(item);
            }

            else if (currentTool == ResourceGatherer.ToolType.AdvancedDrill)
            {
                item = ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(8);
                //Inventory.RemoveItem(item.id, 1);
                Debug.LogWarning("Don't add and remove drill from inventory during equip/unequip?");

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
        if (ResourceInventory.Instance != null) 
        {
            ResourceInventory.Instance.SetResourceAmounts(Inventory);
        }

        Cursor.lockState = cachedCursorLockMode;
        Time.timeScale = 1.0f;
        ShowingInventory = false;
        CanvasScript.HideHeadsUpShop(false);
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
        IsShopping = true;
        Shop.Init();
        OnInventoryShow();
        CanvasScript.ShowEquipment();
        CanvasScript.HideItemCatalog();
        CanvasScript.ShowShop();
        ResourceInventory.Instance.OnStartShopping();

        bool useHeadsUpShop = true;

        if (useHeadsUpShop)
        {
            CanvasScript.HideEquipment();
            CanvasScript.HideItemCatalog();
            CanvasScript.HideShop();
            CanvasScript.ShowHeadsUpShop();


            if (GameManager.Instance.CurrentPlanet
                != null
                && GameManager.Instance.CurrentPlanet.Vendor != null)
            {
                ShopHeadsUp.SetVendor(GameManager.Instance.CurrentPlanet.Vendor);
            }

            else
            {
                // This an editor launch straight in the scene. Just create a new vendor, when it is
                // Noticed that the vendor is actually null
                ShopHeadsUp.SetVendor(null);
            }

        }
    }

    public void OnExitShop()
    {
        OnInventoryHide();
        CanvasScript.HideShop();
        CanvasScript.HideHeadsUpShop(true);
        ResourceInventory.Instance.OnEnterShoppingArea();        
        IsShopping = false;
    }


}
