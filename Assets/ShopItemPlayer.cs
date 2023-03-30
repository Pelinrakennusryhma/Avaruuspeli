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
                                                                              ShopItemScript.ID, 
                                                                              666,
                                                                              sellAmount);

        GameManager.Instance.InventoryController.Inventory.OnItemSold(ShopItemScript.ID,
                                                                      ShopItemScript.ItemAmount + sellAmount,
                                                                      sellAmount);
        Debug.Log("Sell amount is " + sellAmount);
    }
}
