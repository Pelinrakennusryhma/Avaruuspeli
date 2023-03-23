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

    private Item infoAbout;
    void Start()
    {
        //infoAbout = GameObject.Find("Canvas").GetComponent<CanvasScript>().infoAbout;
        infoAbout = FindObjectOfType<CanvasScript>().infoAbout;
        Item item = infoAbout;
        itemImage.sprite = Resources.Load<Sprite>("Sprites/" + item.name);
        itemName.text = infoAbout.name;
        itemType.text = item.type;
        itemValue.text = item.value.ToString();
        itemDescription.text = item.description;
    }

    public void ClosePanel()
    {
        Destroy(gameObject);
    }
}
