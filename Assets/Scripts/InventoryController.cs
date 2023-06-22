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
    public HydroponicsBay HydroponicsBay;

    public bool ShowingInventory;
    public bool IsInShoppingArea;
    public bool IsShopping;

    public Sprite BlankSprite;

    CursorLockMode cachedCursorLockMode = CursorLockMode.None;
    private bool cachedCursorHideMode = true;

    public float Money = 10000.0f;

    public string previousPromptText;

    public void Init()
    {
        ItemDataBaseWithScriptables.Init();
        Inventory.OnInventoryControllerInit();
        ISRUModule.Init();
        HydroponicsBay.Init();
        //Item item;
        //item = CanvasScript.contextMenu.itemDatabase.GetItem(2);
        ////Inventory.RemoveItem(item.id, 1);
        //Equipment.EquipDrill(item);
        //Shop.Init();

        //ItemDataBaseWithScriptables = GetComponentInChildren<ItemDataBaseWithScriptables>(true);

        if (GameManager.LaunchType == GameManager.TypeOfLaunch.DevGame)
        {
            SetDevMoneyAndInventory();
        }

        else if(GameManager.LaunchType == GameManager.TypeOfLaunch.LoadedGame)
        {
            Money = GameManager.Instance.SaverLoader.LoadMoney();

            int loadedObjectInHands = GameManager.Instance.SaverLoader.LoadEquippedItemInHands();
            int loadedSpaceSuit = GameManager.Instance.SaverLoader.LoadEquippedSpaceSuit();


            if (loadedObjectInHands > 0)
            {
                ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(loadedObjectInHands);
                Equipment.EquipObjectInHands(item);
            }

            if (loadedSpaceSuit > 0)
            {
                ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(loadedSpaceSuit);
                Equipment.EquipSpacesuit(item);
            }

            //Debug.Log("Equipped object in hands is " + loadedObjectInHands + " equipped spacesuit is " + loadedSpaceSuit);

            List<GeneralSaveData.InventoryItem> loadedItems = GameManager.Instance.SaverLoader.LoadInventory();

            if (loadedItems != null
                && loadedItems.Count > 0)
            {
                for (int i = 0; i < loadedItems.Count; i++)
                {
                    Inventory.AddItem(loadedItems[i].ID, loadedItems[i].Amount);
                }
            }

            //Debug.Log("Load invenotry here");

        }

        else
        {
            Money = 1.0f;
            Inventory.AddItem(20, 1);
            Inventory.AddItem(7, 1); // Can't even start the game without this.
            Inventory.AddItem(13, 2); // Oxygen bottle
            Inventory.AddItem(14, 1); // Warpdrive fuel
            Inventory.AddItem(15, 1); // rocket fuel
            Inventory.AddItem(16, 2); // Oxygen storage
            Equipment.UnequipObjectInHands();
            Equipment.UnequipSpacesuit();
        }






        Shop.Init();
        ShopHeadsUp.Init();

        OnInventoryHide(false);
    }

    private void SetDevMoneyAndInventory()
    {
        Money = 10000.0f;
        OnInventoryShow(false);

        Inventory.AddItem(29, 1);
        Inventory.AddItem(32, 1);
        Inventory.AddItem(2, 1);
        Inventory.AddItem(3, 1);
        Inventory.AddItem(4, 1);
        Inventory.AddItem(5, 1);

        Inventory.AddItem(6, 1);
        Inventory.AddItem(7, 1);
        Inventory.AddItem(8, 1);
        Inventory.AddItem(9, 1);
        Inventory.AddItem(12, 1);

        Inventory.AddItem(13, 21); // Oxygen bottle
        Inventory.AddItem(14, 100); // Warpdrive fuel
        Inventory.AddItem(15, 100); // rocket fuel
        Inventory.AddItem(16, 10); // Oxygen storage
        Inventory.AddItem(17, 1);
        Inventory.AddItem(18, 1);
        Inventory.AddItem(19, 1);
        Inventory.AddItem(20, 1);
        Inventory.AddItem(21, 1);
        Inventory.AddItem(22, 1);

        Inventory.AddItem(27, 100);
        Inventory.AddItem(28, 100);
        Inventory.AddItem(30, 100);
        Inventory.AddItem(31, 100);

        AddShipItems();

        GameManager.Instance.SaverLoader.SaveMoney(Money);
    }

    void AddShipItems()
    {
        foreach (ItemSO item in ItemDataBaseWithScriptables.ItemDataBaseSO.ShipItems)
        {
            Inventory.AddItem(item.id, 1);
        }

        foreach (ItemSO item in ItemDataBaseWithScriptables.ItemDataBaseSO.ShipWeaponItems)
        {
            Inventory.AddItem(item.id, 1);
        }
    }

    public void OnInventoryShow(bool cacheCursor)
    {

        if (cacheCursor) 
        {
            cachedCursorLockMode = Cursor.lockState;
            cachedCursorHideMode = Cursor.visible;
        }


        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
        ShowingInventory = true;
        CanvasObject.SetActive(true);
        AttachToMainCamera();

        CanvasScript.HideItemCatalog();
        CanvasScript.HideShop();
        CanvasScript.ShowEquipment();

        ISRUModule.gameObject.SetActive(false);
        HydroponicsBay.gameObject.SetActive(false);

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

    public void OnInventoryHide(bool saveToo)
    {


        if (ResourceInventory.Instance != null) 
        {
            ResourceInventory.Instance.SetResourceAmounts(Inventory);
        }

        Cursor.lockState = cachedCursorLockMode;
        Cursor.visible = cachedCursorHideMode;



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

        if (saveToo) 
        {
            GameManager.Instance.SaverLoader.SaveMoney(Money);
            GameManager.Instance.SaveLifeSupportData(false);
            SaveInventory();
        }

        //Debug.Log("Hide inventory");
    }

    public void SaveInventory()
    {
        GameManager.Instance.SaverLoader.SaveInventory(Inventory.InventoryItemScripts);
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
        OnInventoryShow(false);
        //Debug.Log("Hide isru module");
    }

    public void OnHydroponicsBayShow()
    {
        CanvasObject.SetActive(true);
        AttachToMainCamera();

        CanvasScript.HideHeadsUpShop(false);
        CanvasScript.HideItemCatalog();
        CanvasScript.HideShop();
        CanvasScript.HideEquipment();

        Inventory.gameObject.SetActive(false);

        HydroponicsBay.gameObject.SetActive(true);
        HydroponicsBay.OnViewOpened();
    }

    public void OnHydroponicsBayHide()
    {
        Inventory.gameObject.SetActive(true);
        HydroponicsBay.gameObject.SetActive(false);
        OnInventoryShow(false);
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
        OnInventoryShow(true);
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
        OnInventoryHide(true);
        CanvasScript.HideShop();
        CanvasScript.HideHeadsUpShop(true);
        ResourceInventory.Instance.OnEnterShoppingArea();
        GameManager.Instance.SaverLoader.SaveMoney(Money);
        IsShopping = false;

    }


}
