using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    private void Awake()
    {
        BuildDatabase();
    }

    //Hakee itemin ID:n mukaan
    public Item GetItem(int id)
    {
        return items.Find(item => item.id == id);
    }

    //Hakee itemin nimen mukaan
    public Item GetItem(string name)
    {
        return items.Find(item => item.name == name);
    }


    //Täsä voidaan lisätä uusia itemeitä. Item(id, name, value, type, stackable, description)
    //Type = Resource, Consumable, Drill, Spacesuit, ShipWeapon
    //Kuva tulee laittamalla kuva kansioon Resources/Sprites, ja nimeämällä sen saman nimiseksi kuin item.
    void BuildDatabase()
    {
        items = new List<Item> {
             new Item(0, "Iron", 1, 1.0, "Resource", true, "Tämä on iron"),
             new Item(1, "Gold", 100, 1.5,  "Resource", true, "Tämä on gold"),
             new Item(2, "Basic Drill", 10, 1.0, "Drill", false, "Tämä on basic drill"),
             new Item(3, "Sandwich", 1, 1.0, "Consumable", true, ""),
             new Item(4, "Advanced Drill", 20, 1.0, "Drill", false, ""),
             new Item(5, "Spacesuit", 5, 1.0, "Spacesuit", false, ""),
             new Item(6, "Ship Laser", 60, 1.0, "ShipWeapon", false, ""),
             new Item(7, "Ship Gun", 40, 1.0, "ShipWeapon", false, ""),
             new Item(8, "Silver", 50, 1.0, "Resource", true, "Tämä on hopeaa"),
             new Item(9, "Copper", 10, 0.5, "Resource", true, "Tämä on kuparia"),
             new Item(10, "Diamond", 1000, 1.0, "Resource", true, "Tämä on timanttia")
        };
    }
}
