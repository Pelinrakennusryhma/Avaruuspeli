using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipItem/ShipHull", order = 9)]
public class ShipHull : ShipItemSO
{
    public int healthAmount;
}
