using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopItemScript : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private TextMeshProUGUI itemPrice;
    [SerializeField] private TMP_InputField buyAmount;
    [SerializeField] private TMP_InputField sellAmount;
    public Inventory inventory;
    public Item item;
    public Shop shop;
    public CanvasScript canvasScript;
    private int playerItemAmount;
    void Awake()
    {
        inventory = GameObject.Find("InventoryPanel").GetComponent<Inventory>();
        shop = GameObject.Find("ShopPanel").GetComponent<Shop>();
        canvasScript = GameObject.Find("Canvas").GetComponent<CanvasScript>();
        item = shop.itemToAdd;
        itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        itemName.text = item.name;
        itemPrice.text = item.value.ToString("0.00") + "€";

        //Tarkistaa montako pelaajalla on itemiä ja muuttaa numeron näyttämään kuinka monta niitä on.
        GameObject inventoryItem = GameObject.Find("InventoryPanel/Scroll/View/Layout/" + item.id);
        if (inventoryItem != null)
        {
            playerItemAmount = inventoryItem.GetComponent<ItemScript>().currentItemAmount;
            itemAmount.text = "Owned: " + playerItemAmount.ToString();
        }
        else
        {
            itemAmount.text = "Owned: 0";
        }

    }

    //Päivittää kuinka monta kauppa näyttää pelaajalla olevan itemiä.
    public void UpdateAmount(int currentItemAmount)
    {
        playerItemAmount = currentItemAmount;
        itemAmount.text = "Owned: " + playerItemAmount.ToString();
    }

    //Kaupan Sell nappi.
    public void ButtonSell()
    {
        if (playerItemAmount >= int.Parse(sellAmount.text))
        {
            canvasScript.money += item.value * int.Parse(sellAmount.text);
            inventory.RemoveItem(item.id, int.Parse(sellAmount.text));
        }
    }
    //Kaupan Buy nappi.
    public void ButtonBuy()
    {
        double buyingAmount = int.Parse(buyAmount.text);
        if ((buyingAmount * item.weight) + inventory.currentWeight <= inventory.maxWeight)
        {
            double price = item.value * buyingAmount;
            if (canvasScript.money >= price)
            {
                inventory.AddItem(item.id, int.Parse(buyAmount.text));
                canvasScript.money -= price;
            }
        }

    }
}
