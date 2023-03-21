using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image drillImage;
    [SerializeField] private UnityEngine.UI.Image spacesuitImage;
    [SerializeField] private UnityEngine.UI.Image shipWeapon1Image;
    [SerializeField] private UnityEngine.UI.Image shipWeapon2Image;
    public Item equippedDrill;
    public Item equippedSpacesuit;
    public Item equippedShipWeapon1;
    public Item equippedShipWeapon2;
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
            drillImage.sprite = Resources.Load<Sprite>("Sprites/Empty");
        }
    }
    public void UnequipSpacesuit()
    {
        if (equippedSpacesuit != null)
        {
            inventory.AddItem(equippedSpacesuit.id, 1);
            equippedSpacesuit = null;
            spacesuitImage.sprite = Resources.Load<Sprite>("Sprites/Empty");
        }
    }
    public void EquipDrill(Item item)
    {
        UnequipDrill();
        equippedDrill = item;
        drillImage.sprite = Resources.Load<Sprite>("Sprites/" + equippedDrill.name);

        Debug.Log("Equipping drill. Item id is " + item.id);
        
        if (ResourceGatherer.Instance != null) 
        {
            if (item.id == 2)
            {
                ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.BasicDrill;
                ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.BasicDrill);
            }   
            
            else if (item.id == 4)     
            {
                ResourceGatherer.Instance.Tool = ResourceGatherer.ToolType.AdvancedDrill;
                ResourceGatherer.Instance.Hands.SetTool(ResourceGatherer.ToolType.AdvancedDrill);
        
            }
        }
   
    }

    public void EquipSpacesuit(Item item)
    {
        UnequipSpacesuit();
        equippedSpacesuit = item;
        spacesuitImage.sprite = Resources.Load<Sprite>("Sprites/" + equippedSpacesuit.name);
    }

    public void UnequipShipWeapon1()
    {
        if (equippedShipWeapon1 != null)
        {
            inventory.AddItem(equippedShipWeapon1.id, 1);
            equippedShipWeapon1 = null;
            shipWeapon1Image.sprite = Resources.Load<Sprite>("Sprites/Empty");
        }
    }
    public void UnequipShipWeapon2()
    {
        if (equippedShipWeapon2 != null)
        {
            inventory.AddItem(equippedShipWeapon2.id, 1);
            equippedShipWeapon2 = null;
            shipWeapon2Image.sprite = Resources.Load<Sprite>("Sprites/Empty");
        }
    }
    public void EquipShipWeapon1(Item item)
    {
        UnequipShipWeapon1();
        equippedShipWeapon1 = item;
        shipWeapon1Image.sprite = Resources.Load<Sprite>("Sprites/" + equippedShipWeapon1.name);
    }
    public void EquipShipWeapon2(Item item)
    {
        UnequipShipWeapon2();
        equippedShipWeapon2 = item;
        shipWeapon2Image.sprite = Resources.Load<Sprite>("Sprites/" + equippedShipWeapon2.name);
    }
}
