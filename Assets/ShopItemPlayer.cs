using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemPlayer : MonoBehaviour
{
    public ShopItemScript ShopItemScript;

    public void Awake()
    {
        ShopItemScript = GetComponent<ShopItemScript>();
    }

    public void OnButtonPressed()
    {


        int sellAmount = Mathf.Clamp(int.Parse(ShopItemScript.sellAmount.text), 0, ShopItemScript.ItemAmount + 1);
        GameManager.Instance.InventoryController.ShopHeadsUp.UpdateShopAmount(true,
                                                                              ShopItemScript.ID,                                                                              sellAmount);

        GameManager.Instance.InventoryController.Inventory.OnItemSold(ShopItemScript.ID,
                                                                      sellAmount);

        GameManager.Instance.InventoryController.Inventory.AddMoney(ShopItemScript.adjustedPrice * sellAmount);

        GameManager.Instance.InventoryController.ShopHeadsUp.OnSuccesfullBuy();

        //Debug.Log("Sell amount is " + sellAmount);
        
    }
}
