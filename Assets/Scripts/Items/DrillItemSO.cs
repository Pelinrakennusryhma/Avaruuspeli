using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Drill", order = 4)]
public class DrillItemSO : ItemSO
{
    [ExecuteInEditMode]
    public void Awake()
    {
        itemType = ItemType.Drill;
    }

    public override bool CheckIfDataBaseAlreadyContainsItem(ItemDataBaseSO dataBase)
    {
        bool baseContains = base.CheckIfDataBaseAlreadyContainsItem(dataBase);

        bool derivedContains = dataBase.CheckDrills(this);

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

        if (!dataBase.CheckDrills(this)) 
        {
            dataBase.AddToDrills(this);
        } 
    }

    public override void RemoveFromDataBase(ItemDataBaseSO dataBase)
    {
        base.RemoveFromDataBase(dataBase);
        dataBase.RemoveFromDrills(this);
    }
}
