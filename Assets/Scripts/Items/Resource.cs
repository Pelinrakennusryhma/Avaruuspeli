using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Resource", order = 8)]
public class Resource : ItemSO
{
    public Resource rare;
    public float rareChance;

    [ExecuteInEditMode]
    public void Awake()
    {
        itemType = ItemType.Resource;
    }

    public override bool CheckIfDataBaseAlreadyContainsItem(ItemDataBaseSO dataBase)
    {
        bool baseContains = base.CheckIfDataBaseAlreadyContainsItem(dataBase);

        bool derivedContains = dataBase.CheckResources(this);

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

        if (!dataBase.CheckResources(this)) 
        {
            dataBase.AddToResources(this);
        }
    }

    public override void RemoveFromDataBase(ItemDataBaseSO dataBase)
    {
        base.RemoveFromDataBase(dataBase);
        dataBase.RemoveFromResources(this);
    }
}

