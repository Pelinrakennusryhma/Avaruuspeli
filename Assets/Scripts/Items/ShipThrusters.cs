using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipItem/ShipThrusters", order = 9)]
public class ShipThrusters : ShipItemSO
{
    public int speed;
}
