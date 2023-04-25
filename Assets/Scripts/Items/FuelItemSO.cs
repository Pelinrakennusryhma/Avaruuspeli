using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FuelItem", order = 6)]
public class FuelItemSO : ItemSO
{
    [ExecuteInEditMode]
    public void Awake()
    {
        itemType = ItemType.Fuel;
    }

    public override bool CheckIfDataBaseAlreadyContainsItem(ItemDataBaseSO dataBase)
    {
        bool baseContains =  base.CheckIfDataBaseAlreadyContainsItem(dataBase);

        bool derivedContains = dataBase.CheckFuel(this);

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

        if (!dataBase.CheckFuel(this)) 
        {
            dataBase.AddToFuel(this);
        }
    }

    public override void RemoveFromDataBase(ItemDataBaseSO dataBase)
    {
        base.RemoveFromDataBase(dataBase);
        dataBase.RemoveFromFuel(this);
    }
}
