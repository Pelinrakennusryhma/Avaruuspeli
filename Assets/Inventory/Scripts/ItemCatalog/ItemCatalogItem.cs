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
    public Item itemToAdd;
    void Awake()
    {
        canvas = GameObject.Find("Canvas");
        itemToAdd = GameObject.Find("ItemCatalog").GetComponent<ItemCatalog>().itemToAdd;
        Item item = itemToAdd;

        itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        itemName.text = itemToAdd.name;
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
            GameObject.Find("Canvas").GetComponent<CanvasScript>().InfoAboutItem(itemToAdd);
            Object prefab = Resources.Load("Prefabs/ItemInfoPanel");
            GameObject newItem = Instantiate(prefab, canvas.transform) as GameObject;
            newItem.name = itemToAdd.id.ToString();
            Vector3 mouseLocation = Input.mousePosition;
            newItem.transform.position = mouseLocation;
        }
    }
}
