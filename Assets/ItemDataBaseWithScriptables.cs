using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataBaseWithScriptables : MonoBehaviour
{
    public static ItemDataBaseWithScriptables Instance;
    public ItemDataBaseSO ItemDataBaseSO;


    public void Init()
    {
        Instance = this;
        ItemDataBaseSO.OnInit();

        //for (int i = 0; i < ItemDataBaseSO.AllItems.Count; i++)
        //{
        //    Debug.Log("We have an item " + ItemDataBaseSO.AllItems[i].itemName);
        //}

        //TestGetters();
    }

    private void TestGetters()
    {



        // Getter testers

        //for (int i = 0; i < 100; i++)
        //{
        //    ItemSO item = ItemDataBaseSO.GetItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    ConsumableItemSO item = ItemDataBaseSO.GetConsumableItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    DrillItemSO item = ItemDataBaseSO.GetDrillItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    EquipmentItemSO item = ItemDataBaseSO.GetEquipmentItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}


        //for (int i = 0; i < 100; i++)
        //{
        //    FuelItemSO item = ItemDataBaseSO.GetFuelItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    PlayerWeaponItemSO item = ItemDataBaseSO.GetPlayerWeaponItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    Resource item = ItemDataBaseSO.GetResourceItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    ShipItemSO item = ItemDataBaseSO.GetShipItem(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}

        //for (int i = 0; i < 100; i++)
        //{
        //    ShipWeaponItemSO item = ItemDataBaseSO.GetShipWeapon(i);

        //    if (item != null)
        //    {
        //        Debug.Log("Item is " + item.itemName + " item id is " + item.id);
        //    }

        //    else
        //    {
        //        Debug.LogError("Null item");
        //    }
        //}
    }
}

