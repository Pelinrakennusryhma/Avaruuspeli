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
    private GameObject canvas;
    private GameObject contextMenu;
    public int currentItemAmount = 0;
    public Item itemToAdd;
    private ContextMenu contextMenuScript;
    private ShopItemScript shopItemScript;
    private GameObject shopItemGO;

    void Awake()
    {
        try
        {
            shopItemGO = GameObject.Find("ShopPanel/" + itemToAdd.id);
        }
        catch
        {
            shopItemGO = null;
        }

        if(shopItemGO != null)
        {
            shopItemScript = shopItemGO.GetComponent<ShopItemScript>();
        }

        canvas = GameObject.Find("Canvas");
        contextMenu = GameObject.Find("ContextMenu");
        contextMenuScript = contextMenu.GetComponent<ContextMenu>();
        //Itemin lisättäessä asettaa tiedot Inventory-skriptistä haettujen tietojen mukaan
        itemToAdd = GameObject.Find("InventoryPanel").GetComponent<Inventory>().itemToAdd;
        Item item = itemToAdd;
        itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        itemName.text = itemToAdd.name;
        itemAmount.text = currentItemAmount.ToString();
        itemValue.text = item.value.ToString();
    }

    public void UpdateShopAmount()
    {
        try
        {
            shopItemScript.UpdateAmount(currentItemAmount);
        }
        catch
        {

        }
    }
    //Päivittää inventoryssa näkyvän määrän
    public void UpdateAmount()
    {
        itemAmount.text = currentItemAmount.ToString();
        try
        {
            shopItemGO = GameObject.Find("ShopPanel/" + itemToAdd.id);
        }
        catch
        {
            shopItemGO = null;
        }
        if (shopItemGO != null)
        {
            shopItemScript = shopItemGO.GetComponent<ShopItemScript>();
        }
        if (shopItemScript != null)
        {
            UpdateShopAmount();
        }
    }

    //Lisää nykyiseen määrään 'amount'. Päivittää määrän.
    public void AddItem(int amount)
    {

        currentItemAmount += amount;
        UpdateAmount();
    }

    //Poistaa nykyisestä määrästä 'amount'. Päivittää määrän.
    public void RemoveItem(int amount)
    {
        currentItemAmount -= amount;
        UpdateAmount();
    }

    //Avaa context menun painettaessa m2. Painettaessa m1 piilottaa context menun ja tuhoaa info paneelit.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
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
                //GameObject.Find("Canvas").GetComponent<CanvasScript>().InfoAboutItem(itemToAdd);
                Object prefab = Resources.Load("Prefabs/ItemInfoPanel");
                GameObject newItem = Instantiate(prefab, canvas.transform) as GameObject;
                newItem.name = itemToAdd.id.ToString();
                Vector3 mouseLocation = Input.mousePosition;
                newItem.transform.position = mouseLocation;
            }
        }
    }
}
