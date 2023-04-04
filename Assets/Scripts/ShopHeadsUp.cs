using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    public TextMeshProUGUI PlayerMoney;
    public TextMeshProUGUI WeightDisplay;
    public TextMeshProUGUI InfoText;

    public Scrollbar PlayerScrollBar;
    public Scrollbar VendorScrollBar;

    public void Init()
    {
        PlayerShopItems = new List<ShopItemScript>();
        VendorShopItems = new List<ShopItemScript>();

        Debug.Log("Initializing heads up shop");
    }

    public void SetVendor(Vendor vendor)
    {
        CurrentVendor = vendor;
        Debug.Log("Setting vendor for heads up shop");
    }

    public void StartShopping()
    {
        Init();

        int galaxyId = GameManager.Instance.CurrentGalaxyData.ID;
        int starSystemId = GameManager.Instance.CurrentStarSystemData.ID;
        int planetId = GameManager.Instance.CurrentPlanetData.ID;

        CurrentVendor = GameManager.Instance.SaverLoader.GetVendor(galaxyId, 
                                                                   starSystemId, 
                                                                   planetId);

        if (CurrentVendor == null
            || (CurrentVendor != null 
                && CurrentVendor.SellMultipliers.Length != GameManager.Instance.InventoryController.Inventory.itemDatabase.items.Count))
        {
            CurrentVendor = new Vendor();        
            CurrentVendor.InitializeVendor(galaxyId,
                                           starSystemId,
                                           planetId);

            Debug.LogError("NULL vendor. Created a new one");
        }



        AddPlayerItems();
        AddVendorItems();
        PlayerScrollBar.value = 1.0f;
        VendorScrollBar.value = 1.0f;

        InfoText.text = "";

        Debug.Log("Start shopping " + Time.time);
    }

    public void FinishShopping()
    {
        Debug.LogError("Missing functionality: SAVE the items the vendor has!!!");

        for (int i = 0; i < PlayerShopItems.Count; i++)
        {
            Destroy(PlayerShopItems[i].gameObject);
        }

        PlayerShopItems.Clear();

        CurrentVendor.Items.Clear();

        for (int i = 0; i < VendorShopItems.Count; i++)
        {
            CurrentVendor.Items.Add(new Vendor.VendorInventoryItem(VendorShopItems[i].item.id, VendorShopItems[i].ItemAmount));

            Destroy(VendorShopItems[i].gameObject);
        }

        for (int i = 0; i < CurrentVendor.Items.Count; i++)
        {
            Debug.Log("Currently the vendor has " + CurrentVendor.Items[i].ItemId + " of amount " + CurrentVendor.Items[i].ItemAmount);
        }

        GameManager.Instance.SaverLoader.SaveVendor(CurrentVendor);
        VendorShopItems.Clear();
    }

    public void AddPlayerItems()
    {
        // DEstryo any previously shown items
        //for (int i = 0; i < PlayerShopItems.Count; i++)
        //{
        //    Destroy(PlayerShopItems[i].gameObject);
        //}

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

            //Debug.Log("Player item " + playerItems[i].itemToAdd.name);
        }

        Debug.Log("Adding player items");
    }

    public void AddVendorItems()
    {
        //for (int i = 0; i < VendorShopItems.Count; i++)
        //{
        //    Destroy(VendorShopItems[i].gameObject);
        //}

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

    public void UpdateMoney()
    {

    }

    public void UpdateWeight()
    {

    }

    public void PlayerIsOutOfMoneyToBuyItem()
    {
        InfoText.text = "You don't have enough money to buy this";
    }

    public void PlayerIsOutOfRoomToBuyItem()
    {
        InfoText.text = "You don't have enough room in inventory to buy this";
    }

    public void PlayerIsTryingToBuyMultipleSingletonItems()
    {
        InfoText.text = "You already have this item. One is enough.";
    }

    public void OnSuccesfullBuy()
    {
        InfoText.text = "";
    }
}
