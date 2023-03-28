using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Item/Item", order = 0)]
public class ItemSO : ScriptableObject
{
    public int id;
    public string itemName;
    public string plural;
    public int value;
    public string description;
    public GameObject itemPrefab;
    public GameObject[] itemPrefabVariants;
    public Sprite itemIcon;
}

