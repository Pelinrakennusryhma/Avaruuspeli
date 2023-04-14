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
        bool hasEnoughMoney = false;
        bool hasEnoughRoom = false;

        bool alreadyHasSingletonItem = false;
        ItemSO item = GameManager.Instance.InventoryController.Inventory.CheckForItem(ShopItemScript.ID);

        if (item != null
            && !item.isStackable)
        {
            alreadyHasSingletonItem = true;
            //Debug.Log("AlreadyHasSingletonItem ");
        }

        else
        {
            //Debug.Log("Does not have singleton item yet.");
        }

        hasEnoughRoom = GameManager.Instance.InventoryController.Inventory.CheckIfWeHaveRoomForItem(ShopItemScript.item, 
                                                                                                    int.Parse(ShopItemScript.buyAmount.text));

        if (!alreadyHasSingletonItem 
            && hasEnoughRoom)
        {
            //Debug.Log("WE have enough room for items");

            hasEnoughMoney = GameManager.Instance.InventoryController.Inventory.TryToBuyItemWithMoney(int.Parse(ShopItemScript.buyAmount.text) 
                                                                                                      * ShopItemScript.adjustedPrice,
                                                                                                      true);

            //if (hasEnoughMoney)
            //{
            //    Debug.Log("Had enough money to buy items");
            //}
            //else
            //{
            //    Debug.Log("Not enough money to buy item");
            //}
        }

        else
        {
            hasEnoughMoney = GameManager.Instance.InventoryController.Inventory.TryToBuyItemWithMoney(int.Parse(ShopItemScript.buyAmount.text)
                                                                                                      * ShopItemScript.adjustedPrice,
                                                                                                      false);
        }

        if (!hasEnoughMoney)
        {
            GameManager.Instance.InventoryController.ShopHeadsUp.PlayerIsOutOfMoneyToBuyItem();
            //Debug.Log("Current money is " + GameManager.Instance.InventoryController.Money);
        }

        else if (!hasEnoughRoom)
        {
            GameManager.Instance.InventoryController.ShopHeadsUp.PlayerIsOutOfRoomToBuyItem();
        }

        if (alreadyHasSingletonItem)
        {
            GameManager.Instance.InventoryController.ShopHeadsUp.PlayerIsTryingToBuyMultipleSingletonItems();
        }


        if (hasEnoughMoney
            && hasEnoughRoom
            && !alreadyHasSingletonItem)
        {

            int buyAmount = Mathf.Clamp(int.Parse(ShopItemScript.buyAmount.text), 0, ShopItemScript.ItemAmount + 1);
            GameManager.Instance.InventoryController.ShopHeadsUp.UpdateShopAmount(false,
                                                                                  ShopItemScript.ID,
                                                                                  buyAmount);

            GameManager.Instance.InventoryController.Inventory.OnItemBought(ShopItemScript.ID,
                                                                            ShopItemScript.ItemAmount + buyAmount,
                                                                            buyAmount);

            GameManager.Instance.InventoryController.Inventory.UpdateWeight((float)ShopItemScript.item.weight * buyAmount);

            GameManager.Instance.InventoryController.ShopHeadsUp.OnSuccesfullBuy();
            //Debug.Log("Buy amount is " + buyAmount);
        }
    }
}
