using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipItem/ShipUtility", order = 9)]
public class ShipUtility : ShipItemSO
{
    public MonoScript scriptToAdd;
    public float effectDuration;
    public float cooldown;
}
