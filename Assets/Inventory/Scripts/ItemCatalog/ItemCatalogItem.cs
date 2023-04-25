using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemCatalogItem : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    private GameObject canvas;
    public ItemSO itemToAdd;
    void Awake()
    {
        //Debug.LogWarning("Awake called on item catalog item " + Time.time);

        //canvas = GameObject.Find("Canvas");
        canvas = GameManager.Instance.InventoryController.CanvasScript.gameObject;

        // This works only because Awake is called right after instantiating

        //itemToAdd = GameObject.Find("ItemCatalog").GetComponent<ItemCatalog>().itemToAdd;
        itemToAdd = GameManager.Instance.InventoryController.ItemCatalog.itemToAdd;


        //Debug.LogError("Replace the finds with something else!!!");
        ItemSO item = itemToAdd;

        // itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);

        if (item.itemIcon != null)
        {
            itemImage.sprite = item.itemIcon;
            //Debug.LogWarning("We had a good sprite. Well done");
        }

        else
        {
            itemImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            //Debug.LogError("No sprite, putting a blank out there");
        }

        //Debug.LogError("Replace this with scriptable object's sprite");
        itemName.text = itemToAdd.itemName;
    }

    //Painettaessa tuhoaa vanhat info paneelit, ja näyttää tämän itemin info paneelin.
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            GameObject[] gos = GameObject.FindGameObjectsWithTag("InfoPanel");

            foreach (GameObject go in gos)
            {
                Destroy(go);
            }

            //GameObject.Find("Canvas").GetComponent<CanvasScript>().InfoAboutItem(itemToAdd);
            GameManager.Instance.InventoryController.CanvasScript.InfoAboutItem(itemToAdd);


            Object prefab = Resources.Load("Prefabs/ItemInfoPanel");

            GameObject newItem = Instantiate(prefab, canvas.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
            Vector3 mouseLocation = Input.mousePosition;
            newItem.transform.position = mouseLocation;
        }
    }
}
