using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public List<Item> items = new List<Item>();

    public void OnInit()
    {
        BuildDatabase();
    }

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
             new Item(10, "Diamond", 1000, 200.0, "Resource", true, "Tämä on timanttia")
        };
    }

    //void BuildDatabase()
    //{
    //    items = new List<Item> {
    //         new Item(1, "Stone", 1, "Resource", true, "Tämä on stone"),
    //         new Item(2, "Iron", 1, "Resource", true, "Tämä on iron"),
    //         new Item(3, "Copper", 10, "Resource", true, "Tämä on cpeer"),
    //         new Item(4, "Silver", 50, "Resource", true, "Tämä on silver"),
    //         new Item(5, "Gold", 100, "Resource", true, "Tämä on gold"),
    //         new Item(6, "Diamond", 1000, "Resource", true, "Tämä on diamond"),
    //         new Item(7, "Basic drill", 10, "Drill", false, "Tämä on basic drill"),
    //         new Item(8, "Advanced drill", 20, "Drill", false, "Tämä on advanced drill"),
    //         new Item(9, "Lazergun", 10, "PlayerWeapon", false, "Tämä on lazergun"),
    //         new Item(10, "Ship Gun", 40, "ShipWeapon", false, "Tämä on ship gun"),
    //         new Item(11, "Ship lazer", 60, "ShipWeapon", false, "Tämä on ship lazer"),
    //         new Item(12, "Spacesuit", 5, "Spacesuit", false, "Tämä on spacesuit"),
    //         new Item(13, "Oxygen bottle", 5, "OxygenBottle", false, "Tämä on oxygen bottle"),
    //         new Item(14, "Warpdrive fuel", 5, "Fuel", true, "Tämä on warpdrive fuel"),
    //         new Item(15, "Rocket fuel", 5, "Fuel", true, "Tämä on rocket fuel"),
    //         new Item(16, "Oxygen storage", 5, "OxygenStorage", true, "Tämä on oxygen storage"),
    //         new Item(17, "Scanner", 20, "Ship", false, "Tämä on scanner"),
    //         new Item(18, "Speed", 25, "Ship", false, "Tämä on speed"),
    //         new Item(19, "Ship choice", 1, "Ship", false, "Tämä on alus ship choice"),
    //         new Item(20, "Sanwich", 1, "Consumable", true, "Tämä on sanwich"),
    //    };
    //}
}
