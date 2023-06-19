using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

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
        if (PlayerShopItems != null)
        {
            for (int i = 0; i < PlayerShopItems.Count; i++)
            {
                Destroy(PlayerShopItems[i].gameObject);
                Debug.Log("Destroying player item");
            }
        }

        if (VendorShopItems != null)
        {
            for (int i = 0; i < VendorShopItems.Count; i++)
            {
                Destroy(VendorShopItems[i].gameObject);
                Debug.Log("Destroying vendor item");
            }
        }

        PlayerShopItems = new List<ShopItemScript>();
        VendorShopItems = new List<ShopItemScript>();

        //Debug.Log("Initializing heads up shop");
    }

    public void SetVendor(Vendor vendor)
    {
        CurrentVendor = vendor;
        //Debug.Log("Setting vendor for heads up shop");
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
                && CurrentVendor.SellMultiplierss == null)
            || (CurrentVendor != null 
                && CurrentVendor.SellMultiplierss.Length != GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.AllItems.Count))
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

        //Debug.Log("Start shopping " + Time.time);
    }

    public void FinishShopping()
    {
        //Debug.LogError("Finish shopping");

        for (int i = 0; i < PlayerShopItems.Count; i++)
        {
            Destroy(PlayerShopItems[i].gameObject);
            //Debug.Log("Destroying player items");
        }

        PlayerShopItems.Clear();

        if (CurrentVendor != null 
            && CurrentVendor.Items != null)
        {
            CurrentVendor.Items.Clear();        
            
            if (VendorShopItems != null)
            {
                for (int i = 0; i < VendorShopItems.Count; i++)
                {
                    CurrentVendor.Items.Add(new Vendor.VendorInventoryItem(VendorShopItems[i].item.id, VendorShopItems[i].ItemAmount));
                    
                    //Debug.Log("Destroying a vendor shop item " + VendorShopItems[i].item.itemName);
                    Destroy(VendorShopItems[i].gameObject);
                }
            }
        }

        else
        {
            if (VendorShopItems != null)
            {
                for (int i = 0; i < VendorShopItems.Count; i++)
                {
                    //CurrentVendor.Items.Add(new Vendor.VendorInventoryItem(VendorShopItems[i].item.id, VendorShopItems[i].ItemAmount));

                    Destroy(VendorShopItems[i].gameObject);
                    //Debug.Log("Destroying vendot items");
                }

                VendorShopItems.Clear();
            }
        }



        //for (int i = 0; i < CurrentVendor.Items.Count; i++)
        //{
        //    Debug.Log("Currently the vendor has " + CurrentVendor.Items[i].ItemId + " of amount " + CurrentVendor.Items[i].ItemAmount);
        //}

        if (CurrentVendor != null) 
        {
            if (CurrentVendor.GalaxyID != 0
                && CurrentVendor.StarSystemID != 0
                && CurrentVendor.PlanetID != 0)
            {
                //Debug.Log("Finishing shopping. About to save vendor " + CurrentVendor.GalaxyID + " " + CurrentVendor.StarSystemID + " " + CurrentVendor.PlanetID);
                GameManager.Instance.SaverLoader.SaveVendor(CurrentVendor);

            }
        }

        VendorShopItems.Clear();
    }

    public void AddPlayerItems()
    {
        // DEstryo any previously shown items
        //Debug.Log("Player shop items count before adding items is " + PlayerShopItems.Count);

        for (int i = 0; i < PlayerShopItems.Count; i++)
        {
            Destroy(PlayerShopItems[i].gameObject);
            //Debug.Log("Destroying player item");
        }

        PlayerShopItems = new List<ShopItemScript>();

        List<ItemScript> playerItems = GameManager.Instance.InventoryController.Inventory.InventoryItemScripts;

        for (int i = 0; i < playerItems.Count; i++)
        {
            GameObject shopObject = Instantiate(PlayerItemPrefab, PlayerItemsLayout.gameObject.transform);

            ShopItemScript shopItem = shopObject.GetComponent<ShopItemScript>();
            PlayerShopItems.Add(shopItem);

            ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(playerItems[i].itemToAdd.id);
            shopItem.Setup(item, 
                           GameManager.Instance.InventoryController.Inventory, 
                           CurrentVendor.GetBuyMultiplier(item.id),
                           true);
            shopItem.UpdateAmount(GameManager.Instance.InventoryController.Inventory.GetItemScript(playerItems[i].itemToAdd.id).currentItemAmount);

            //Debug.Log("Player item " + playerItems[i].itemToAdd.name);
        }

        //Debug.Log("Player shop items count after adding items is " + PlayerShopItems.Count);

        //Debug.Log("Adding player items");
    }

    public void AddVendorItems()
    {
        for (int i = 0; i < VendorShopItems.Count; i++)
        {
            Destroy(VendorShopItems[i].gameObject);
            //Debug.Log("Destroying vendor item");
        }

        VendorShopItems = new List<ShopItemScript>();

        for (int i = 0; i < CurrentVendor.Items.Count; i++)
        {
            GameObject shopObject = Instantiate(VendorItemPrefab, VendorItemsLayout.gameObject.transform);

            ShopItemScript shopItem = shopObject.GetComponent<ShopItemScript>();
            VendorShopItems.Add(shopItem);


            //Debug.Log("Gettin an item for vendor " + CurrentVendor.Items[i].ItemId);

            

            ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(CurrentVendor.Items[i].ItemId);

            //if (shopItem == null)
            //{
            //    Debug.LogError("Null shop item");
            //}



            shopItem.Setup(item, 
                           GameManager.Instance.InventoryController.Inventory, 
                           CurrentVendor.GetSellMultiplier(item.id),
                           false);
            shopItem.UpdateAmount(CurrentVendor.Items[i].ItemAmount);

            //Debug.Log("Vendor item " + item.itemName);
        }

        //Debug.Log("Adding vendor items");
    }

    public void AddItemToPlayerList(int itemId, int amount)
    {
        GameObject shopObject = Instantiate(PlayerItemPrefab, PlayerItemsLayout.gameObject.transform);

        ShopItemScript shopItem = shopObject.GetComponent<ShopItemScript>();
        PlayerShopItems.Add(shopItem);

        ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemId);
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

        ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(itemId);
        shopItem.Setup(item,
                       GameManager.Instance.InventoryController.Inventory,
                       CurrentVendor.GetSellMultiplier(item.id),
                       false);
        shopItem.UpdateAmount(amount);

        Debug.Log("Should add item " + itemId + " amount " + amount + " to vendor list");
    }

    public void UpdateShopAmount(bool isPlayerItem,
                                 int id,
                                 int buyAmount)
    {
        //Debug.Log("New amount is " + " buy amount is " + buyAmount);

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
                //Debug.Log("Should have removed player item");
            }

            else
            {
                VendorShopItems.Remove(itemScript);
                //Debug.Log("Should have removed vendor item");
            }

            Destroy(itemScript.gameObject);
        }

        if (!hasItemAlready)
        {
            //Debug.Log("Does not have the item. is player item " + isPlayerItem);

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
            //Debug.Log("Has item should add");

            if (!isPlayerItem)
            {
                for (int i = 0; i < PlayerShopItems.Count; i++)
                {
                    if (PlayerShopItems[i].ID == id)
                    {
                        PlayerShopItems[i].UpdateAmount(PlayerShopItems[i].ItemAmount + buyAmount);
                        //itemScript = PlayerShopItems[i];
                        //Debug.Log("Should add to player items. item id " + id);
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
                        //Debug.Log("Should add to vendor items. item id " + id);
                        break;
                    }
                }
            }
        }
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

    public void SortPlayerItemsByName()
    {
        // Destroy previous
        for (int i = 0; i < PlayerShopItems.Count; i++)
        {
            Destroy(PlayerShopItems[i].gameObject);
        }

        PlayerShopItems.Clear();

        // Get a list of playeritems
        List<ItemScript> playerItems = GameManager.Instance.InventoryController.Inventory.InventoryItemScripts;

        // sort them
        playerItems = playerItems.OrderBy(x => x.itemToAdd.itemName).ToList();

        // display them

        for (int i = 0; i < playerItems.Count; i++)
        {
            AddItemToPlayerList(playerItems[i].itemToAdd.id, playerItems[i].currentItemAmount);
        }

        // Set scrollbar to top
        PlayerScrollBar.value = 1.0f;

        //Debug.LogError("MISSING FUNCTIONALITY: Sort player items by name");
    }

    public void SortPlayerItemsByWeight()
    {
        // Destroy previous
        for (int i = 0; i < PlayerShopItems.Count; i++)
        {
            Destroy(PlayerShopItems[i].gameObject);
        }

        PlayerShopItems.Clear();

        // Get a list of playeritems
        List<ItemScript> playerItems = GameManager.Instance.InventoryController.Inventory.InventoryItemScripts;

        // sort them
        // swap to descending order

        playerItems = playerItems.OrderByDescending(x => x.currentItemWeight).ToList();



        // display them
        for (int i = 0; i < playerItems.Count; i++)
        {
            AddItemToPlayerList(playerItems[i].itemToAdd.id, playerItems[i].currentItemAmount);
        }

        // Set scrollbar to top
        PlayerScrollBar.value = 1.0f;

    }

    public void SortPlayerItemsByValue()
    {
        // Destroy previous

        for (int i = 0; i < PlayerShopItems.Count; i++)
        {
            Destroy(PlayerShopItems[i].gameObject);
        }

        PlayerShopItems.Clear();

        // Get a list of playeritems
        List<ItemScript> playerItems = GameManager.Instance.InventoryController.Inventory.InventoryItemScripts;

        // sort them
        // swap to descending order
        // There is a order operation for this descending thing already! nice find

        playerItems = playerItems.OrderByDescending(x => x.currentTotalValue).ToList();

        // display them
        for (int i = 0; i < playerItems.Count; i++)
        {
            AddItemToPlayerList(playerItems[i].itemToAdd.id, playerItems[i].currentItemAmount);
        }

        // Set scrollbar to top
        PlayerScrollBar.value = 1.0f;
        //Debug.LogError("MISSING FUNCTIONALITY: Sort player items by value");
    }


}
