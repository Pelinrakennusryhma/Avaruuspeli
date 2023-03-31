using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemScript : MonoBehaviour
{
    public int ID;
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] public TMP_InputField buyAmount;
    [SerializeField] public TMP_InputField sellAmount;
    public Inventory inventory;
    public Item item;

    public CanvasScript canvasScript;
    public int ItemAmount;

    public float adjustedPrice;

    private ShopNumberTwo shopNumberTwo;
    private ShopHeadsUp shopHeadsUp;
    public bool IsPlayerItem;

    public void SetReferenceToShop(ShopHeadsUp headsUpShop)
    {
        shopHeadsUp = headsUpShop;
        shopNumberTwo = null;
    }

    public void SetReferenceToShop(ShopNumberTwo shopNumberTwo)
    {
        shopHeadsUp = null;
        this.shopNumberTwo = shopNumberTwo;
    }

    public void Setup(Item item, 
                      Inventory inventory,
                      float priceMultiplier,
                      bool isPlayerItem)
    {
        IsPlayerItem = isPlayerItem;
        adjustedPrice = item.value * priceMultiplier;

        this.inventory = inventory;

        canvasScript = GameManager.Instance.InventoryController.CanvasScript;
        this.item = item;
        ID = item.id;
        itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        itemName.text = item.name;
        itemPrice.text = adjustedPrice.ToString("0.00") + "€";

        //Tarkistaa montako pelaajalla on itemiä ja muuttaa numeron näyttämään kuinka monta niitä on.
        //GameObject inventoryItem = GameObject.Find("InventoryPanel/Scroll/View/Layout/" + item.id);

        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(item.id);
        if (inventoryItemScript != null)
        {
            ItemAmount = inventoryItemScript.currentItemAmount;
            itemAmount.text = "Owned: " + ItemAmount.ToString();
        }
        else
        {
            itemAmount.text = "Owned: 0";
        }

        if (canvasScript == null)
        {
            Debug.LogError("Null canvas");
        }

    }

    //Päivittää kuinka monta kauppa näyttää pelaajalla olevan itemiä.
    public void UpdateAmount(int currentItemAmount)
    {
        this.ItemAmount = currentItemAmount;
        itemAmount.text = "Owned: " + this.ItemAmount.ToString();

        if (shopNumberTwo != null)
        {
            //shopNumberTwo.UpdateShopAmount(ID, currentItemAmount);
        }

        if (shopHeadsUp != null)
        {
            //shopHeadsUp.UpdateShopAmount(IsPlayerItem, ID, currentItemAmount);
        }

        //GameManager.Instance.InventoryController.Shop.UpdateShopAmount(ID, currentItemAmount);

        //if (playerItemAmount == 0) 
        //{
        //    itemAmount.text = "hahaha";
        //}

        //else
        //{
        //    itemAmount.text = "isompi kuin nolla";
        //}
        //Debug.Log("Updating amount " + Time.time + " new amount is " + this.ItemAmount + " current item amount is " + currentItemAmount + " text is " + itemAmount.text);
    }

    //Kaupan Sell nappi.
    public void ButtonSell()
    {
        if (ItemAmount >= int.Parse(sellAmount.text))
        {
            canvasScript.money += item.value * int.Parse(sellAmount.text);
            inventory.RemoveItem(item.id, int.Parse(sellAmount.text));        
            
            Debug.Log("Sell " + Time.time);
        }

        int amount = 0;
        ItemScript itemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(item.id);

        if (itemScript != null)
        {
            amount = itemScript.currentItemAmount;
        }

        ItemAmount = amount;

        UpdateAmount(ItemAmount);
        Debug.Log("Pressed sell button");
        //GameManager.Instance.InventoryController.Inventory.OnItemSold();
    }
    //Kaupan Buy nappi.
    public void ButtonBuy()
    {
        bool alreadyHasSingletonItem = false;

        if (!item.stackable
            && inventory.CheckForItem(item.id) != null) 
        {
            alreadyHasSingletonItem = true;
        }

        Debug.Log("Buy " + Time.time + " item id " + item.name + " already has singleton item " + alreadyHasSingletonItem);


        double buyingAmount = int.Parse(buyAmount.text);
        if ((buyingAmount * item.weight) + inventory.currentWeight <= inventory.maxWeight
            && !alreadyHasSingletonItem)
        {
            double price = adjustedPrice * buyingAmount;
            if (canvasScript.money >= price)
            {
                inventory.AddItem(item.id, int.Parse(buyAmount.text));
                canvasScript.money -= price;
            }
        }

        int amount = 0;
        ItemScript itemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(item.id);

        if (itemScript != null)
        {
            amount = itemScript.currentItemAmount;
        }

        ItemAmount = amount;

        UpdateAmount(ItemAmount);
        Debug.Log("Pressed sell button");
        //GameManager.Instance.InventoryController.Inventory.OnItemSold(); GameManager.Instance.InventoryController.Inventory.OnItemBought();
    }
}
