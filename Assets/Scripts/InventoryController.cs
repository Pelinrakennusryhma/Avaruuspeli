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
    public ISRUModule ISRUModule;

    public bool ShowingInventory;
    public bool IsInShoppingArea;
    public bool IsShopping;

    public Sprite BlankSprite;

    CursorLockMode cachedCursorLockMode = CursorLockMode.None;

    public float Money = 10000.0f;

    public string previousPromptText;

    public void Init()
    {        
        ItemDataBaseWithScriptables.Init();
        Inventory.OnInventoryControllerInit();
        ISRUModule.Init();
        //Item item;
        //item = CanvasScript.contextMenu.itemDatabase.GetItem(2);
        ////Inventory.RemoveItem(item.id, 1);
        //Equipment.EquipDrill(item);
        //Shop.Init();

        //ItemDataBaseWithScriptables = GetComponentInChildren<ItemDataBaseWithScriptables>(true);

        Money = 10000.0f;
        OnInventoryShow();

        Inventory.AddItem(29, 1);
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

        Inventory.AddItem(13, 21); // Oxygen bottle
        Inventory.AddItem(14, 100); // Warpdrive fuel
        Inventory.AddItem(15, 100); // rocket fuel
        Inventory.AddItem(16, 1);
        Inventory.AddItem(17, 1);
        Inventory.AddItem(18, 1);
        Inventory.AddItem(19, 1);
        Inventory.AddItem(20, 1);
        Inventory.AddItem(21, 1);
        Inventory.AddItem(22, 1);

        Inventory.AddItem(27, 100);
        Inventory.AddItem(28, 100);
        Inventory.AddItem(30, 100);

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

        ISRUModule.gameObject.SetActive(false);

        if (ResourceGatherer.Instance != null) 
        {
            ResourceGatherer.ToolType currentTool = ResourceGatherer.Instance.Tool;

            ItemSO item;

            if (currentTool == ResourceGatherer.ToolType.BasicDrill)
            {
                item = ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(7);
                //Inventory.RemoveItem(item.id, 1);
                Debug.LogWarning("Don't add and remove drill from inventory during equip/unequip?");

                Equipment.EquipObjectInHands(item);
            }

            else if (currentTool == ResourceGatherer.ToolType.AdvancedDrill)
            {
                item = ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(8);
                //Inventory.RemoveItem(item.id, 1);
                Debug.LogWarning("Don't add and remove drill from inventory during equip/unequip?");

                Equipment.EquipObjectInHands(item);
            }

            else if (currentTool == ResourceGatherer.ToolType.DiamondDrill)
            {
                item = ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(21);
                //Inventory.RemoveItem(item.id, 1);
                Debug.LogWarning("Don't add and remove drill from inventory during equip/unequip?");

                Equipment.EquipObjectInHands(item);
            }
        }

        if (PlayerHands.Instance != null)
        {
            ItemSO item;

            if (PlayerHands.Instance.CurrentWeapon == Weapon.WeaponType.LaserGun)
            {
                item = ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(9);
                Equipment.EquipObjectInHands(item);
            }

            else if (PlayerHands.Instance.CurrentWeapon == Weapon.WeaponType.MeleeWeapon)
            {
                item = ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(22);
                Equipment.EquipObjectInHands(item);
            }
            //Debug.LogWarning("Player hands are not null. Should equipWeapon");
        }

        if (ResourceInventory.Instance != null)
        {
            ResourceInventory.Instance.UnloadGatheredItems(Inventory);
        }

        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.CallEventInventoryOpened();
        }

        //GameEvents.Instance.CallEventPlayerExitedPromptTrigger();
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

        if (GameEvents.Instance != null)
        {
            GameEvents.Instance.CallEventInventoryClosed();
        }

        if (MotherShipOnWorldMapController.Instance != null)
        {
            MotherShipOnWorldMapController.Instance.FuelSystem.UpdateAllFuelAmounts();
            //Debug.LogWarning("Updating all fuel amounts");
        }

        else
        {
           // Debug.LogError("Null mothership instance. Don't update warpdrive amounts");
        }
        //Debug.Log("Hide inventory");
    }

    public void AttachToMainCamera()
    {
        CanvasObject.transform.SetParent(Camera.main.transform, false);

        if (WorldMapScene.Instance != null)
        {
            WorldMapScene.Instance.HideCanvas();
        }
    }

    public void DetachFromMainCamera()
    {
        CanvasObject.transform.SetParent(transform, false);

        if (WorldMapScene.Instance != null)
        {
            WorldMapScene.Instance.ShowCanvas();
        }
    }

    public void OnDrill1Equipped()
    {
        //CanvasScript.
    }

    public ResourceGatherer.ToolType GetCurrentEquippedTool()
    {
        ResourceGatherer.ToolType currentTool = ResourceGatherer.ToolType.None;

        if (Equipment.equippedObjectInHands != null)
        {
            if (Equipment.equippedObjectInHands.id == 7)
            {
                currentTool = ResourceGatherer.ToolType.BasicDrill;
            }

            else if  (Equipment.equippedObjectInHands.id == 7)
            {
                currentTool = ResourceGatherer.ToolType.AdvancedDrill;
            }

            else if (Equipment.equippedObjectInHands.id == 21)
            {
                currentTool = ResourceGatherer.ToolType.DiamondDrill;
                Debug.Log("We are currently euipping diamond drill");
            }

        }

        return currentTool;
    }

    public Weapon.WeaponType GetCurrentEquippedWeapon()
    {
        Weapon.WeaponType currentWeapon = Weapon.WeaponType.None;

        if (Equipment.equippedObjectInHands != null) 
        {
            if (Equipment.equippedObjectInHands.id == 9)
            {
                currentWeapon = Weapon.WeaponType.LaserGun;
            }

            else if (Equipment.equippedObjectInHands.id == 22)
            {
                currentWeapon = Weapon.WeaponType.MeleeWeapon;
            }
        }

        return currentWeapon;
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

    public void OnISRUShow()
    {
        CanvasObject.SetActive(true);
        AttachToMainCamera();

        CanvasScript.HideHeadsUpShop(false);
        CanvasScript.HideItemCatalog();
        CanvasScript.HideShop();
        CanvasScript.HideEquipment();

        Inventory.gameObject.SetActive(false);

        ISRUModule.gameObject.SetActive(true);
        ISRUModule.OnViewOpened();

        //Debug.Log("Show isru module");
    }

    public void OnISRUHide()
    {
        Inventory.gameObject.SetActive(true);
        ISRUModule.gameObject.SetActive(false);
        OnInventoryShow();
        //Debug.Log("Hide isru module");
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
