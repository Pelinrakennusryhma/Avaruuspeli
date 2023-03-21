using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public int id;
    public string name;
    public int value;
    public double weight;
    public string type;
    public bool stackable;
    public string description;

    public Item(int id, string name, int value, double weight, string type, bool stackable, string description)
    {
        this.id = id;
        this.name = name;
        this.value = value;
        this.weight = weight;
        this.type = type;
        this.stackable = stackable;
        this.description = description;
    }
}
