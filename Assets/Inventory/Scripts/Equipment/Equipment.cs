using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image imageOfObjectInHands;
    [SerializeField] private UnityEngine.UI.Image spacesuitImage;
    [SerializeField] private UnityEngine.UI.Image shipWeapon1Image;
    [SerializeField] private UnityEngine.UI.Image shipWeapon2Image;
    public ItemSO equippedObjectInHands;
    public ItemSO equippedSpacesuit;
    public ItemSO equippedShipWeapon1;
    public ItemSO equippedShipWeapon2;
    public Inventory inventory;

    //Tavaroitten unequip ja equip. Kutsutaan ContextMenu.cs kautta.
    public void UnequipObjectInHands()
    {
        if (ResourceGatherer.Instance != null) 
        {
            ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.None;
            ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.None);
        }

        if (PlayerHands.Instance != null)
        {
            PlayerHands.Instance.SetWeapon(Weapon.WeaponType.None);
            
        }

        if (equippedObjectInHands != null)
        {
            //inventory.AddItem(equippedDrill.id, 1);
            //Debug.LogWarning("Don't add and remove drill from inventory during equip/unequip?");
            
            equippedObjectInHands = null;
            //drillImage.sprite = Resources.Load<Sprite>("Sprites/Empty");
            imageOfObjectInHands.sprite = GameManager.Instance.InventoryController.BlankSprite;

        }            
        
        //Debug.LogError("Unequipping drill");
    }
    public void UnequipSpacesuit()
    {
        if (equippedSpacesuit != null)
        {
            //inventory.AddItem(equippedSpacesuit.id, 1);
            //Debug.LogWarning("Don't add and remove spacesuit from inventory during equip/unequip?");
            equippedSpacesuit = null;
            //spacesuitImage.sprite = Resources.Load<Sprite>("Sprites/Empty");
            spacesuitImage.sprite = GameManager.Instance.InventoryController.BlankSprite;


            GameManager.Instance.LifeSupportSystem.OnSpaceSuitUnequipped();

        }
    }
    public void EquipObjectInHands(ItemSO item)
    {
        UnequipObjectInHands();
        equippedObjectInHands = item;
        //drillImage.sprite = Resources.Load<Sprite>("Sprites/" + equippedDrill.name);

        if (equippedObjectInHands.itemIcon != null)
        {
            imageOfObjectInHands.sprite = equippedObjectInHands.itemIcon;
            //Debug.LogWarning("We had a sprite for a drill or gun");
        }

        else
        {
            imageOfObjectInHands.sprite = GameManager.Instance.InventoryController.BlankSprite;
            //Debug.LogError("Had to put to blank sprite out there, because we didn't have an image");
        }

        //Debug.Log("Equipping object in hands. Item id is " + item.id + " at time " + Time.time);
        
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

            else if (item.id == 21)
            {
                ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.DiamondDrill;
                ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.DiamondDrill);

            }

            else
            {
                ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.None;
                ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.None);
            }
        }

        if (PlayerHands.Instance != null)
        {
            if (item.id == 9)
            {
                PlayerHands.Instance.SetTool(ResourceGatherer.ToolType.None);
                PlayerHands.Instance.SetWeapon(Weapon.WeaponType.LaserGun);
                //Debug.Log("Equip laser gun" + Time.time);
            }

            else if (item.id == 22)
            {
                PlayerHands.Instance.SetTool(ResourceGatherer.ToolType.None);
                PlayerHands.Instance.SetWeapon(Weapon.WeaponType.MeleeWeapon);
                //Debug.Log("Equip melee weapon" + Time.time);
            }

            else
            {
                PlayerHands.Instance.SetWeapon(Weapon.WeaponType.None);
                //Debug.Log("Do not equip a weapon");
                //PlayerHands.Instance.SetTool(ResourceGatherer.ToolType.None);
            }

            //Debug.LogWarning("Non null player hands instance");
        }
   
    }

    public void EquipSpacesuit(ItemSO item)
    {
        UnequipSpacesuit();
        equippedSpacesuit = item;

        Debug.LogWarning("Equipping spacesuit");

        //spacesuitImage.sprite = Resources.Load<Sprite>("Sprites/" + equippedSpacesuit.name);

        if (equippedSpacesuit.itemIcon != null)
        {
            spacesuitImage.sprite = equippedSpacesuit.itemIcon;
            //Debug.LogWarning("We had a sprite for a spacesuit");
        }

        else
        {
            spacesuitImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            //Debug.LogError("Had to put to blank sprite out there, because we didn't have an image");
        }

        GameManager.Instance.LifeSupportSystem.OnSpaceSuitEquipped(item);

        //Debug.LogError("Replace with some scriptable object stuff");
    }

    public void UnequipShipWeapon1()
    {
        if (equippedShipWeapon1 != null)
        {
            inventory.AddItem(equippedShipWeapon1.id, 1);
            equippedShipWeapon1 = null;
            //shipWeapon1Image.sprite = Resources.Load<Sprite>("Sprites/Empty");


            shipWeapon1Image.sprite = GameManager.Instance.InventoryController.BlankSprite;

            //Debug.LogError("Replace with some scriptable object stuff");
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

            //Debug.LogError("Replace with some scriptable object stuff");
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

        //Debug.LogError("Replace with some scriptable object stuff");
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

        //Debug.LogError("Replace with some scriptable object stuff");
    }

    public bool CheckIfAnItemIsEquipped(ItemSO item)
    {
        bool isAnEquippedItem = false;

        if (item == equippedObjectInHands
            || item == equippedSpacesuit
            || item == equippedShipWeapon1
            || item == equippedShipWeapon2)
        {
            isAnEquippedItem = true;
            Debug.LogWarning("Tried to discard an equipped item, but we won't allow that for now.");
        }

        else
        {
            Debug.LogWarning("We didn't catch an equipped item with a check. Item is " + item.id + " name is " + item.itemName);
        }

        //if ((equippedDrill != null && item.id == equippedDrill.id)
        //    || (equippedSpacesuit != null && item.id == equippedSpacesuit.id)
        //    || (equippedShipWeapon1 != null && item.id == equippedShipWeapon1.id)
        //    || (equippedShipWeapon2 != null && item.id == equippedShipWeapon2.id))
        //{
        //    isAnEquippedItem = true;
        //    Debug.LogWarning("Tried to discard an equipped item, but we won't allow that for now.");
        //}

        //else
        //{
        //    Debug.LogWarning("We didn't catch an equipped item with a check. Item is " + item.id + " name is " + item.itemName);
        //}

        return isAnEquippedItem;
    }
}
