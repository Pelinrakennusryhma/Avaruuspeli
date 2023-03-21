using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Resource", order = 0)]
public class Resource : Item
{
    public Resource rare;
    public float rareChance;
}

