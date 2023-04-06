using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemValue;
    [SerializeField] private TextMeshProUGUI itemDescription;

    private ItemSO infoAbout;
    void Start()
    {
        infoAbout = FindObjectOfType<CanvasScript>().infoAbout;
        ItemSO item = infoAbout;
        //itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        //Debug.LogError("Replace this with scriptable objects sprite");

        if (item.itemIcon != null)
        {
            itemImage.sprite = item.itemIcon;
            Debug.LogWarning("Non null icon. proceed");
        }

        else
        {
            itemImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
            Debug.LogError("Null sprite. Replacing with a blank one");
        }



        itemName.text = infoAbout.itemName;
        itemType.text = item.itemType.ToString();
        Debug.LogError("REplace this possibly with different type of item type fetching");
        itemValue.text = item.value.ToString();
        itemDescription.text = item.description;
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }
}
