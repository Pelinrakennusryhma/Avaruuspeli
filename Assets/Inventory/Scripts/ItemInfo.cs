using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInfo : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemValue;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private ItemSO infoAbout;
    void Start()
    {
        infoAbout = GameManager.Instance.InventoryController.CanvasScript.infoAbout;
        ItemSO item = infoAbout;
        //itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        //Debug.LogError("Replace this with scriptable objects sprite");

        if (item.itemIcon != null)
        {
            itemImage.sprite = item.itemIcon;
            //Debug.LogWarning("Non null icon. proceed");
        }

        else
        {
            itemImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            //Debug.LogError("Null sprite. Replacing with a blank one");
        }



        itemName.text = infoAbout.itemName;
        itemType.text = item.itemType.ToString();
        itemValue.text = item.value.ToString();
        itemDescription.text = item.description;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
           
        }

        ClosePanel();
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }
}
