using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItemVendor : MonoBehaviour
{
    public ShopItemScript ShopItemScript;

    public void Awake()
    {
        ShopItemScript = GetComponent<ShopItemScript>();
    }

    public void OnButtonPressed()
    {
        int buyAmount = Mathf.Clamp(int.Parse(ShopItemScript.buyAmount.text), 0, ShopItemScript.ItemAmount + 1);
        GameManager.Instance.InventoryController.ShopHeadsUp.UpdateShopAmount(false, 
                                                                              ShopItemScript.ID, 
                                                                              666,
                                                                              buyAmount);

        GameManager.Instance.InventoryController.Inventory.OnItemBought(ShopItemScript.ID, 
                                                                        ShopItemScript.ItemAmount + buyAmount,
                                                                        buyAmount);
        Debug.Log("Buy amount is " + buyAmount);
    }
}
