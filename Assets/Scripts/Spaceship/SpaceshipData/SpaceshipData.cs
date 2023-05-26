using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SpaceshipData", order = 1)]
public class SpaceshipData : ScriptableObject
{
    public GameObject prefab;
    public ShipUtility[] utilities = new ShipUtility[2];
    public ShipHull hull;
    public ShipWeaponItemSO primaryWeapon;
}
