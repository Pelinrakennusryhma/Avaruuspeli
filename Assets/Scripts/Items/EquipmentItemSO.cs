using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Equipment", order = 5)]
public class EquipmentItemSO : ItemSO
{
    [ExecuteInEditMode]
    public void Awake()
    {
        itemType = ItemType.Equipment;
    }

    public override bool CheckIfDataBaseAlreadyContainsItem(ItemDataBaseSO dataBase)
    {
        bool baseContains = base.CheckIfDataBaseAlreadyContainsItem(dataBase);

        bool derivedContains = dataBase.CheckEquipment(this);

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

        if (!dataBase.CheckEquipment(this)) 
        {
            dataBase.AddToEquipment(this);
        }
    }

    public override void RemoveFromDataBase(ItemDataBaseSO dataBase)
    {
        base.RemoveFromDataBase(dataBase);
        dataBase.RemoveFromEquipment(this);
    }
}
