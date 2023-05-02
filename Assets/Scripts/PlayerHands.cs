using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHands : MonoBehaviour
{
    public ResourceGatherer.ToolType CurrentTool;
    public Weapon.WeaponType CurrentWeapon;

    public static PlayerHands Instance;

    public GameObject DiamondDrill;
    public GameObject AdvancedDrill;
    public GameObject BasicDrill;

    public LaserGun LaserGun;
    public MeleeWeapon MeleeWeapon;

    public FirstPersonPlayerControls Controls;

    public GameObject FireTarget;
    public void Awake()
    {
        Instance = this;

        Controls = FindObjectOfType<FirstPersonPlayerControls>();
        LaserGun.SetFireTarget(FireTarget);

        if (Controls == null)
        {
            Debug.LogError("Null controls");
        }

        else
        {
            //Debug.LogWarning("We found controls just fine");
        }

        SetTool(ResourceGatherer.ToolType.None);
        SetWeapon(Weapon.WeaponType.None);
    }

    public void Start()
    {
        CurrentWeapon = GameManager.Instance.InventoryController.GetCurrentEquippedWeapon();
        SetWeapon(CurrentWeapon);
        //Debug.Log("Current equipped weapon is " + CurrentWeapon);
    }

    public void SetTool(ResourceGatherer.ToolType tool)
    {
        //SetWeapon(Weapon.WeaponType.None);

        AdvancedDrill.SetActive(false);
        BasicDrill.SetActive(false);
        DiamondDrill.SetActive(false);

        if (ResourceGatherer.Instance != null)
        {
            ResourceGatherer.Instance.Tool = tool;
            //Debug.LogWarning("Non null resource gatherer" + Time.time);
        }

        else
        {
            //Debug.LogError("Null resourcegatherer");
        }

        switch (tool)
        {
            case ResourceGatherer.ToolType.None:
                break;

            case ResourceGatherer.ToolType.BasicDrill:
                //Debug.Log("We are here 1");
                BasicDrill.SetActive(true);
                break;

            case ResourceGatherer.ToolType.AdvancedDrill:
                //Debug.Log("We are here 2");
                AdvancedDrill.SetActive(true);
                break;

             case ResourceGatherer.ToolType.DiamondDrill:
                DiamondDrill.SetActive(true);
                break;

            default:
                break;
        }
    }

    public void SetWeapon(Weapon.WeaponType weapon)
    {
        //SetTool(ResourceGatherer.ToolType.None);

        CurrentWeapon = weapon;

        LaserGun.gameObject.SetActive(false);
        MeleeWeapon.gameObject.SetActive(false);

        //Debug.LogWarning("Set weapon type to " + weapon.ToString() + Time.time);

        switch (weapon)
        {
            case Weapon.WeaponType.None:
                break;
            case Weapon.WeaponType.LaserGun:
                LaserGun.gameObject.SetActive(true);
                break;
            case Weapon.WeaponType.MeleeWeapon:
                MeleeWeapon.gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }

    public void Update()
    {
        if (Time.timeScale <= 0)
        {
            return;
        }

        if (Controls.Alpha4Down
            && CurrentWeapon != Weapon.WeaponType.LaserGun
            && GameManager.Instance.InventoryController.Inventory.CheckForItem(9))
        {
            GameManager.Instance.InventoryController.Equipment.EquipObjectInHands(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(9));
            //SetTool(ResourceGatherer.ToolType.None);
            //SetWeapon(Weapon.WeaponType.LaserGun);
        }

        else if (Controls.Alpha5Down
                 && CurrentWeapon != Weapon.WeaponType.MeleeWeapon
                 && GameManager.Instance.InventoryController.Inventory.CheckForItem(22))
        {
            GameManager.Instance.InventoryController.Equipment.EquipObjectInHands(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(22));
            //SetTool(ResourceGatherer.ToolType.None);
            //SetWeapon(Weapon.WeaponType.MeleeWeapon);
        }

        if (Controls.Fire1Down)
        {
            if (CurrentWeapon == Weapon.WeaponType.LaserGun)
            {
                LaserGun.OnFire1Down();
            }

            else if (CurrentWeapon == Weapon.WeaponType.MeleeWeapon)
            {
                MeleeWeapon.OnFire1Down();
            }
        }
    }
}
