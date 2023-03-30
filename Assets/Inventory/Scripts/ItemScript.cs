using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ItemScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemAmount;
    [SerializeField] private TextMeshProUGUI itemValue;
    [SerializeField] private TextMeshProUGUI itemWeight;
    //private GameObject canvas;
    //private GameObject contextMenu;
    public int currentItemAmount = 0;
    public double currentItemWeight = 0;
    public float currentTotalValue = 0;

    public Item itemToAdd;

    public CanvasScript CanvasScript;
    public ContextMenu contextMenuScript;
    private ShopItemScript shopItemScript;



    public void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        CanvasScript = FindObjectOfType<CanvasScript>();

        //canvas = GameObject.Find("Canvas");
        //contextMenu = GameObject.Find("ContextMenu");
        //contextMenuScript = contextMenu.GetComponent<ContextMenu>();
        contextMenuScript = FindObjectOfType<ContextMenu>();
        //Itemin lisättäessä asettaa tiedot Inventory-skriptistä haettujen tietojen mukaan
        itemToAdd = GameObject.Find("InventoryPanel").GetComponent<Inventory>().itemToAdd;
        Item item = itemToAdd;
        itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        itemName.text = itemToAdd.name;
        itemAmount.text = currentItemAmount.ToString();
        itemValue.text = item.value.ToString();
        itemWeight.text = item.weight.ToString();

        ShopItemScript[] allShopItems = GameManager.Instance.InventoryController.Shop.ShopItems;

        for (int i = 0; i < allShopItems.Length; i++)
        {
            if (allShopItems[i].ID == itemToAdd.id)
            {
                shopItemScript = allShopItems[i];

            }
        }
    }

    public void UpdateShopAmount()
    {
        Debug.Log("Updating shop amount");

 
        //shopItemScript.UpdateAmount(currentItemAmount);

    }
    //Päivittää inventoryssa näkyvän määrän
    public void UpdateAmount()
    {
        itemAmount.text = currentItemAmount.ToString();
        itemWeight.text = currentItemWeight.ToString();
        currentTotalValue = currentItemAmount * itemToAdd.value;
        itemValue.text = currentTotalValue.ToString();

        //try
        //{
        //    shopItemGO = GameObject.Find("ShopPanel/" + itemToAdd.id);
        //}
        //catch
        //{
        //    shopItemGO = null;
        //}


        //if (shopItemScript == null)
        //{
        //    ShopItemScript[] allShopItems = GameManager.Instance.InventoryController.Shop.ShopItems;

        //    for (int i = 0; i < allShopItems.Length; i++)
        //    {
        //        if (allShopItems[i].ID == itemToAdd.id)
        //        {
        //            shopItemScript = allShopItems[i];

        //        }
        //    }
        //}

        //UpdateShopAmount();      
    }

    //Lisää nykyiseen määrään 'amount'. Päivittää määrän.
    public void AddItem(int amount, Item item)
    {
        itemToAdd = item;
        currentItemAmount += amount;
        currentItemWeight += itemToAdd.weight * amount;
        UpdateAmount();
    }

    //Poistaa nykyisestä määrästä 'amount'. Päivittää määrän.
    public void RemoveItem(int amount)
    {
        currentItemAmount -= amount;
        currentItemWeight -= itemToAdd.weight * amount;
        UpdateAmount();
    }

    //Avaa context menun painettaessa m2. Painettaessa m1 piilottaa context menun ja tuhoaa info paneelit.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log(itemToAdd);
            contextMenuScript.ShowOptions(itemToAdd.type);
            contextMenuScript.SetPositionToMouse();
            contextMenuScript.itemID = itemToAdd.id;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                GameObject[] gos = GameObject.FindGameObjectsWithTag("InfoPanel");
                foreach (GameObject go in gos)
                {
                    Destroy(go);
                }
                FindObjectOfType<CanvasScript>().InfoAboutItem(itemToAdd);
                Object prefab = Resources.Load("Prefabs/ItemInfoPanel");
                GameObject newItem = Instantiate(prefab, CanvasScript.transform) as GameObject;
                newItem.name = itemToAdd.id.ToString();
                Vector3 mouseLocation = Input.mousePosition;
                newItem.transform.position = mouseLocation;
            }
        }
    }
}
