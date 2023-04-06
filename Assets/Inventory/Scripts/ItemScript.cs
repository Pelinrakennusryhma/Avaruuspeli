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

    public ItemSO itemToAdd;

    public CanvasScript CanvasScript;
    public ContextMenu contextMenuScript;
    private ShopItemScript shopItemScript;



    public void Awake()
    {
        //Setup();
    }

    public void Setup(Inventory inventory)
    {
        CanvasScript = GameManager.Instance.InventoryController.CanvasScript;

        //canvas = GameObject.Find("Canvas");
        //contextMenu = GameObject.Find("ContextMenu");
        //contextMenuScript = contextMenu.GetComponent<ContextMenu>();
        contextMenuScript = GameManager.Instance.InventoryController.ContextMenuScript;


        //Itemin lisättäessä asettaa tiedot Inventory-skriptistä haettujen tietojen mukaan
        //itemToAdd = GameObject.Find("InventoryPanel").GetComponent<Inventory>().itemToAdd;

        itemToAdd = inventory.itemToAdd;

        Debug.Log("Setup called on itemscript " + Time.time);

        Debug.LogError("Replace the find with something else!!!");
        ItemSO item = itemToAdd;

        if (itemToAdd.itemIcon != null)
        {        
            itemImage.sprite = itemToAdd.itemIcon;
            Debug.LogWarning("Non null icon. proceed");
        }

        else
        {
            itemImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Null sprite. Replacing with a blank one");
        }

        //itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        //Debug.LogError("Replace this with scriptable object's sprite");


        itemName.text = itemToAdd.itemName;
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
    public void AddItem(int amount, ItemSO item)
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
            contextMenuScript.ShowOptions(itemToAdd.itemType.ToString());
            Debug.LogError("Possibly replace the above call with something else that fetches a good type");
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
