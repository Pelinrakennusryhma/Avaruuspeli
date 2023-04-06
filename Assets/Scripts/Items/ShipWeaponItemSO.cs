using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipWeapon", order = 10)]
public class ShipWeaponItemSO : ItemSO
{
    [ExecuteInEditMode]
    public void Awake()
    {
        itemType = ItemType.ShipWeapon;
    }

    public override bool CheckIfDataBaseAlreadyContainsItem(ItemDataBaseSO dataBase)
    {
        bool baseContains = base.CheckIfDataBaseAlreadyContainsItem(dataBase);

        bool derivedContains = dataBase.CheckShipWeapons(this);

        if (!baseContains
            || !derivedContains)
        {
            return false;
        }

        else
        {
            return true;
        }
    }

    public override void AddToDataBase(ItemDataBaseSO dataBase)
    {
        base.AddToDataBase(dataBase);

        if (!dataBase.CheckShipWeapons(this)) 
        {
            dataBase.AddToShipWeapons(this);
        }
    }

    public override void RemoveFromDataBase(ItemDataBaseSO dataBase)
    {
        base.RemoveFromDataBase(dataBase);
        dataBase.RemoveFromShipWeapons(this);
    }
}
