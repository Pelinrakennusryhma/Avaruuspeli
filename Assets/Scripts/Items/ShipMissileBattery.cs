using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipWeapon/ShipMissileBattery", order = 10)]
public class ShipMissileBattery : ShipWeaponItemSO
{
    public float cooldown = 0.25f;
    public float explosionDamage = 90f;
    public float explosionRadius = 20f;
    public float missileSpeed = 150f;
    public int missileCapacity = 5;

}
