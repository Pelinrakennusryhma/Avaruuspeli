using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image drillImage;
    [SerializeField] private UnityEngine.UI.Image spacesuitImage;
    [SerializeField] private UnityEngine.UI.Image shipWeapon1Image;
    [SerializeField] private UnityEngine.UI.Image shipWeapon2Image;
    public ItemSO equippedDrill;
    public ItemSO equippedSpacesuit;
    public ItemSO equippedShipWeapon1;
    public ItemSO equippedShipWeapon2;
    public Inventory inventory;

    //Tavaroitten unequip ja equip. Kutsutaan ContextMenu.cs kautta.
    public void UnequipDrill()
    {
        if (ResourceGatherer.Instance != null) 
        {
            ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.None;
            ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.None);
        }

        if (equippedDrill != null)
        {
            inventory.AddItem(equippedDrill.id, 1);
            equippedDrill = null;
            //drillImage.sprite = Resources.Load<Sprite>("Sprites/Empty");
            drillImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Replace with some scriptable object stuff");
        }
    }
    public void UnequipSpacesuit()
    {
        if (equippedSpacesuit != null)
        {
            inventory.AddItem(equippedSpacesuit.id, 1);
            equippedSpacesuit = null;
            //spacesuitImage.sprite = Resources.Load<Sprite>("Sprites/Empty");
            spacesuitImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Replace with some scriptable object stuff");
        }
    }
    public void EquipDrill(ItemSO item)
    {
        UnequipDrill();
        equippedDrill = item;
        //drillImage.sprite = Resources.Load<Sprite>("Sprites/" + equippedDrill.name);

        if (equippedDrill.itemIcon != null)
        {
            drillImage.sprite = equippedDrill.itemIcon;
            Debug.LogWarning("We had a sprite for a drill");
        }

        else
        {
            drillImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Had to put to blank sprite out there, because we didn't have an image");
        }
        Debug.LogError("Replace with some scriptable object stuff");

        Debug.Log("Equipping drill. Item id is " + item.id);
        
        if (ResourceGatherer.Instance != null) 
        {
            if (item.id == 7)
            {
                ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.BasicDrill;
                ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.BasicDrill);
            }   
            
            else if (item.id == 8)     
            {
                ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.AdvancedDrill;
                ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.AdvancedDrill);
        
            }
        }
   
    }

    public void EquipSpacesuit(ItemSO item)
    {
        UnequipSpacesuit();
        equippedSpacesuit = item;
        //spacesuitImage.sprite = Resources.Load<Sprite>("Sprites/" + equippedSpacesuit.name);

        if (equippedSpacesuit.itemIcon != null)
        {
            spacesuitImage.sprite = equippedDrill.itemIcon;
            Debug.LogWarning("We had a sprite for a spacesuit");
        }

        else
        {
            spacesuitImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Had to put to blank sprite out there, because we didn't have an image");
        }

        Debug.LogError("Replace with some scriptable object stuff");
    }

    public void UnequipShipWeapon1()
    {
        if (equippedShipWeapon1 != null)
        {
            inventory.AddItem(equippedShipWeapon1.id, 1);
            equippedShipWeapon1 = null;
            //shipWeapon1Image.sprite = Resources.Load<Sprite>("Sprites/Empty");

            shipWeapon1Image.sprite = GameManager.Instance.InventoryController.BlankSprite;

            Debug.LogError("Replace with some scriptable object stuff");
        }
    }
    public void UnequipShipWeapon2()
    {
        if (equippedShipWeapon2 != null)
        {
            inventory.AddItem(equippedShipWeapon2.id, 1);
            equippedShipWeapon2 = null;
            //shipWeapon2Image.sprite = Resources.Load<Sprite>("Sprites/Empty");

            shipWeapon1Image.sprite = GameManager.Instance.InventoryController.BlankSprite;

            Debug.LogError("Replace with some scriptable object stuff");
        }
    }
    public void EquipShipWeapon1(ItemSO item)
    {
        UnequipShipWeapon1();
        equippedShipWeapon1 = item;
        //shipWeapon1Image.sprite = Resources.Load<Sprite>("Sprites/" + equippedShipWeapon1.name);

        if (equippedShipWeapon1.itemIcon != null)
        {
            shipWeapon1Image.sprite = equippedShipWeapon1.itemIcon;
            Debug.LogWarning("We have a good sprite. Put it out there");
        }

        else
        {
            shipWeapon1Image.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Had to put a blank sprite out there");
        }

        Debug.LogError("Replace with some scriptable object stuff");
    }
    public void EquipShipWeapon2(ItemSO item)
    {
        UnequipShipWeapon2();
        equippedShipWeapon2 = item;
        //shipWeapon2Image.sprite = Resources.Load<Sprite>("Sprites/" + equippedShipWeapon2.name);

        if (equippedShipWeapon2.itemIcon != null)
        {
            shipWeapon2Image.sprite = equippedShipWeapon2.itemIcon;
            Debug.LogWarning("We have a good sprite. Put it out there");
        }

        else
        {
            shipWeapon2Image.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Had to put a blank sprite out there");
        }

        Debug.LogError("Replace with some scriptable object stuff");
    }
}
