using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item/Resource", order = 0)]
public class Resource : ItemSO
{
    public Resource rare;
    public float rareChance;
}

