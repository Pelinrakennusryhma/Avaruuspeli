using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject CanvasObject;

    public CanvasScript CanvasScript;

    public Inventory Inventory;
    public Equipment Equipment;

    public bool ShowingInventory;
    public bool IsInShoppingArea;

    CursorLockMode cachedCursorLockMode = CursorLockMode.None;

    public void Awake()
    {
        //Item item;
        //item = CanvasScript.contextMenu.itemDatabase.GetItem(2);
        ////Inventory.RemoveItem(item.id, 1);
        //Equipment.EquipDrill(item);
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

        Debug.Log("Show inventory");
    }

    public void OnInventoryHide()
    {
        Cursor.lockState = cachedCursorLockMode;
        Time.timeScale = 1.0f;
        ShowingInventory = false;
        CanvasObject.SetActive(false);
        DetachFromMainCamera();
        Debug.Log("Hide inventory");
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
        OnInventoryShow();
        CanvasScript.HideEquipment();
        CanvasScript.HideItemCatalog();
        CanvasScript.ShowShop();
        ResourceInventory.Instance.OnStartShopping();
    }

    public void OnExitShop()
    {
        OnInventoryHide();
        CanvasScript.HideShop();
        ResourceInventory.Instance.OnEnterShoppingArea();
    }
}
