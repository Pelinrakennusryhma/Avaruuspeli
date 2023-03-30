using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopHeadsUp : MonoBehaviour
{
    public ScrollRect PlayerItems;
    public ScrollRect VendorItems;

    public VerticalLayoutGroup PlayerItemsLayout;
    public VerticalLayoutGroup VendorItemsLayout;

    public GameObject PlayerItemPrefab;
    public GameObject VendorItemPrefab;

    public List<ShopItemScript> PlayerShopItems;
    public List<ShopItemScript> VendorShopItems;

    public Vendor CurrentVendor;

    public void Init()
    {
        PlayerShopItems = new List<ShopItemScript>();
        VendorShopItems = new List<ShopItemScript>();

        Debug.Log("Initializing heads up shop");
    }

    public void StartShopping()
    {
        Init();
        CurrentVendor = new Vendor();
        CurrentVendor.InitializeVendor();

        AddPlayerItems();
        AddVendorItems();
        Debug.Log("Start shopping " + Time.time);
    }

    public void AddPlayerItems()
    {
        // DEstryo any previously shown items
        for (int i = 0; i < PlayerShopItems.Count; i++)
        {
            Destroy(PlayerShopItems[i].gameObject);
        }

        PlayerShopItems = new List<ShopItemScript>();

        List<ItemScript> playerItems = GameManager.Instance.InventoryController.Inventory.ItemScripts;

        for (int i = 0; i < playerItems.Count; i++)
        {
            GameObject shopObject = Instantiate(PlayerItemPrefab, PlayerItemsLayout.gameObject.transform);

            ShopItemScript shopItem = shopObject.GetComponent<ShopItemScript>();
            PlayerShopItems.Add(shopItem);

            Item item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(playerItems[i].itemToAdd.id);
            shopItem.Setup(item, 
                           GameManager.Instance.InventoryController.Inventory, 
                           CurrentVendor.GetBuyMultiplier(item.id),
                           true);
            shopItem.UpdateAmount(GameManager.Instance.InventoryController.Inventory.GetItemScript(playerItems[i].itemToAdd.id).currentItemAmount);

            Debug.Log("Player item " + playerItems[i].itemToAdd.name);
        }

        Debug.Log("Adding player items");
    }

    public void AddVendorItems()
    {
        for (int i = 0; i < VendorShopItems.Count; i++)
        {
            Destroy(VendorShopItems[i].gameObject);
        }

        VendorShopItems = new List<ShopItemScript>();

        for (int i = 0; i < CurrentVendor.Items.Count; i++)
        {
            GameObject shopObject = Instantiate(VendorItemPrefab, VendorItemsLayout.gameObject.transform);

            ShopItemScript shopItem = shopObject.GetComponent<ShopItemScript>();
            VendorShopItems.Add(shopItem);

            Item item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(CurrentVendor.Items[i].ItemId);
            shopItem.Setup(item, 
                           GameManager.Instance.InventoryController.Inventory, 
                           CurrentVendor.GetSellMultiplier(item.id),
                           false);
            shopItem.UpdateAmount(CurrentVendor.Items[i].ItemAmount);
        }

        Debug.Log("Adding vendor items");
    }

    public void AddItemToPlayerList(int itemId, int amount)
    {
        GameObject shopObject = Instantiate(PlayerItemPrefab, PlayerItemsLayout.gameObject.transform);

        ShopItemScript shopItem = shopObject.GetComponent<ShopItemScript>();
        PlayerShopItems.Add(shopItem);

        Item item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(itemId);
        shopItem.Setup(item,
                       GameManager.Instance.InventoryController.Inventory,
                       CurrentVendor.GetBuyMultiplier(item.id),
                       true);
        shopItem.UpdateAmount(amount);

        Debug.Log("Should add item " + itemId + " amount " + amount + " to player list");
    }
    public void AddItemToVendorList(int itemId, int amount)
    {
        CurrentVendor.Items.Add(new Vendor.VendorInventoryItem(itemId, amount));

        GameObject shopObject = Instantiate(VendorItemPrefab, VendorItemsLayout.gameObject.transform);

        ShopItemScript shopItem = shopObject.GetComponent<ShopItemScript>();
        VendorShopItems.Add(shopItem);

        Item item = GameManager.Instance.InventoryController.Inventory.itemDatabase.GetItem(itemId);
        shopItem.Setup(item,
                       GameManager.Instance.InventoryController.Inventory,
                       CurrentVendor.GetSellMultiplier(item.id),
                       false);
        shopItem.UpdateAmount(amount);

        Debug.Log("Should add item " + itemId + " amount " + amount + " to vendor list");
    }

    public void OnSoldOut()
    {
        Debug.Log("Remove item from list");
    }

    public void UpdateShopAmount(bool isPlayerItem,
                                 int id,
                                 int newAmount,
                                 int buyAmount)
    {
        Debug.Log("New amount is " + newAmount + " buy amount is " + buyAmount);

        ShopItemScript itemScript = null;

        bool hasItemAlready = false;


        if (isPlayerItem)
        {
            for (int i = 0; i < PlayerShopItems.Count; i++)
            {
                if (PlayerShopItems[i].ID == id)
                {
                    PlayerShopItems[i].UpdateAmount(PlayerShopItems[i].ItemAmount - buyAmount);
                    itemScript = PlayerShopItems[i];
                    break;
                }
            }
        }

        else
        {
            for (int i = 0; i < VendorShopItems.Count; i++)
            {
                if (VendorShopItems[i].ID == id)
                {
                    VendorShopItems[i].UpdateAmount(VendorShopItems[i].ItemAmount - buyAmount);
                    itemScript = VendorShopItems[i];
                    break;
                }
            }
        }

        if (!isPlayerItem)
        {
            for (int i = 0; i < PlayerShopItems.Count; i++)
            {
                if (PlayerShopItems[i].ID == id)
                {
                    hasItemAlready = true;
                    break;
                }
            }
        }

        else
        {
            for (int i = 0; i < VendorShopItems.Count; i++)
            {
                if (VendorShopItems[i].ID == id)
                {
                    hasItemAlready = true;
                    break;
                }
            }
        }

        if (itemScript.ItemAmount <= 0)
        {
            if (isPlayerItem)
            {
                PlayerShopItems.Remove(itemScript);
                Debug.Log("Should have removed player item");
            }

            else
            {
                VendorShopItems.Remove(itemScript);
                Debug.Log("Should have removed vendor item");
            }

            Destroy(itemScript.gameObject);
        }

        if (!hasItemAlready)
        {
            Debug.Log("Does not have the item. is player item " + isPlayerItem);

            if (!isPlayerItem)
            {
                AddItemToPlayerList(id, buyAmount);
            }

            else
            {
                AddItemToVendorList(id, buyAmount);
            }
        }

        else
        {
            Debug.Log("Has item should add");

            if (!isPlayerItem)
            {
                for (int i = 0; i < PlayerShopItems.Count; i++)
                {
                    if (PlayerShopItems[i].ID == id)
                    {
                        PlayerShopItems[i].UpdateAmount(PlayerShopItems[i].ItemAmount + buyAmount);
                        //itemScript = PlayerShopItems[i];
                        Debug.Log("Should add to player items. item id " + id);
                        break;
                    }
                }
            }

            else
            {
                for (int i = 0; i < VendorShopItems.Count; i++)
                {
                    if (VendorShopItems[i].ID == id)
                    {
                        VendorShopItems[i].UpdateAmount(VendorShopItems[i].ItemAmount + buyAmount);
                        //itemScript = VendorShopItems[i];
                        Debug.Log("Should add to vendor items. item id " + id);
                        break;
                    }
                }
            }
        }

        RefreshInventory();
    }

    public void RefreshInventory()
    {

        //for (int i = 0; i < PlayerShopItems.Count; i++)
        //{

        //}
        Debug.Log("Should refresh inventory");
    }
}
