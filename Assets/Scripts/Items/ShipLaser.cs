using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/ShipWeapon/ShipLaser", order = 10)]
public class ShipLaser : ShipWeaponItemSO
{
    public float damage;
    public float velocity;
    public float rateOfFire;

}
