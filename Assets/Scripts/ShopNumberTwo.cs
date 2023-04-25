using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ShopNumberTwo : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    [SerializeField] private TextMeshProUGUI playerMoney;
    //public ItemDatabase itemDatabase;
    public ItemSO itemToAdd;
    public CanvasScript canvasScript;
    public GameObject ShopItemPrefab;
    public GameObject LastObjectAdded;
    public GameObject ScrollRectContent;
    public RectTransform ScrollRectTransform;

    public static ShopNumberTwo Instance;
    public ShopItemScript[] ShopItems;

    public void Awake()
    {
        //Instance = this;
    }

    public void Start()
    {
        //CreateItems();
    }
    public void Init()
    {
        Instance = this;
        //itemDatabase.OnInit();
        CreateItems();
    }



    void CreateItems()
    {
        //NewItem(0);
        NewItem(1);
        NewItem(2);
        NewItem(3);
        NewItem(4);
        NewItem(5);
        NewItem(6);
        NewItem(7);
        NewItem(8);
        NewItem(9);
        NewItem(10);
        NewItem(11);
        NewItem(12);
        NewItem(13);
        NewItem(14);
        NewItem(15);
        NewItem(16);
        NewItem(17);
        NewItem(18);
        NewItem(19);
        NewItem(20);
        NewItem(20);

        ShopItems = GetComponentsInChildren<ShopItemScript>();
    }

    void Update()
    {
        //P‰ivitt‰‰ pelaajan rahat
        playerMoney.text = canvasScript.money.ToString("0.00") + "Ä";
    }
    //Lis‰‰ kauppaan uuden itemin myyntiin ID:n mukaan.
    public void NewItem(int id)
    {
        itemToAdd = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(id);
        //Object prefab = Resources.Load("Prefabs/ShopItem");

        if (itemToAdd == null)
        {
            Debug.LogError("Tried to add item, but it was null. ID was " + id + " returning ");
            return;
        }
        GameObject newItem;


        if (LastObjectAdded == null) 
        {
            newItem = Instantiate(ShopItemPrefab, ScrollRectContent.transform);
        }

        else
        {
            float rectHeight = LastObjectAdded.GetComponent<RectTransform>().rect.height;

            Vector3 pos = new Vector3(0,
                                      LastObjectAdded.GetComponent<RectTransform>().transform.localPosition.y - rectHeight,
                                      0);

            newItem = Instantiate(ShopItemPrefab, Vector3.zero, Quaternion.identity, ScrollRectContent.transform);
            newItem.transform.localPosition = pos;
            newItem.transform.localRotation = Quaternion.Euler(0, 0, 0);

            ScrollRectTransform.sizeDelta = new Vector2(ScrollRectTransform.sizeDelta.x, ScrollRectTransform.sizeDelta.y + rectHeight);
        }

        newItem.name = itemToAdd.id.ToString();
        int amount = 0;
        ItemScript itemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(itemToAdd.id);

        //for (int i = 0; i < GameManager.Instance.InventoryController.Inventory.ItemScripts.Count; i++)
        //{
        //    Debug.Log("Item script is " + GameManager.Instance.InventoryController.Inventory.ItemScripts[i].name);
        //}

        if (itemScript != null)
        {
            amount = itemScript.currentItemAmount;
            //Debug.LogError("Amount is not zero " + amount);
        }

        else
        {
            //Debug.LogWarning("Amount is zero " + amount);
        }

        ShopItemScript shopItem = newItem.GetComponentInChildren<ShopItemScript>(true);
        shopItem.Setup(itemToAdd,
                       GameManager.Instance.InventoryController.Inventory,
                       1.0f,
                       true);
        shopItem.UpdateAmount(amount);
        LastObjectAdded = newItem;
    }

    public void UpdateShopAmount(int id, int newAmount)
    {
        for (int i = 0; i < ShopItems.Length; i++)
        {
            if (ShopItems[i].item.id == id)
            {
                ShopItems[i].UpdateAmount(newAmount);
                break;
            }
        }
    }
}
