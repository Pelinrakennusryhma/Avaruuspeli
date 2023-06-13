using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ISRUItemTarget : MonoBehaviour
{
    public TextMeshProUGUI ItemNameText;
    public TextMeshProUGUI ItemAmountText;

    public Image IconImage;

    public int Amount;
    public ItemSO Item;

    public ISRUModule OwnerISRU;

    public Image BackgroundPlate;
    public Image HighlighterPlate;

    public Color OriginalBackPlateColor;

    public void SetupItem(ItemSO item,
                          int amount,
                          ISRUModule owner)
    {
        OriginalBackPlateColor = BackgroundPlate.color;
        OwnerISRU = owner;
        Item = item;
        ItemNameText.text = Item.itemName;
        Amount = amount;
        ItemAmountText.text = "OWNED:\n" + Amount.ToString();

        if (Item.itemIcon != null)
        {
            IconImage.sprite = Item.itemIcon;
        }

        else
        {
            IconImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
        }
    }

    public void UpdateAmount(int newAmount)
    {
        Amount = newAmount;
        ItemAmountText.text = "OWNED:\n" + Amount.ToString();
        //Debug.Log("Updating isru item target amount. New amount is " + newAmount);
    }

    public void OnButtonPressed()
    {
        OwnerISRU.OnTargetPressed(this);
    }

    public void OnSelected()
    {
        HighlighterPlate.color = Color.green;
        //Debug.Log("Should select " + Item.itemName);
    }

    public void OnDeselected()
    {
        HighlighterPlate.color = new Color32(183, 183, 183, 255);
        //Debug.Log("Should deselect " + Item.itemName);
    }

    public void OnInvalidConversionSelected()
    {
        HighlighterPlate.color = Color.red;
        //Debug.Log("Tried to select an invalid conversion");
    }

    public void MakeNotGrayScaled()
    {
        BackgroundPlate.color = OriginalBackPlateColor;
        ItemNameText.color = Color.white;
        ItemAmountText.color = Color.white;

        if (Item.itemIcon != null)
        {
            IconImage.sprite = Item.itemIcon;
        }

        else
        {
            IconImage.sprite = GameManager.Instance.InventoryController.BlankSprite;
        }

        //Debug.Log("Making not gray scaled " + ItemNameText.text);
    }

    public void MakeGrayScaled()
    {
        ItemNameText.color = new Color32(118, 118, 118, 255);
        ItemAmountText.color = new Color32(118, 118, 118, 255);
        BackgroundPlate.color = new Color32(65, 65, 65, 255);

        if (IconImage.sprite != null) 
        {
            if (GameManager.Instance == null
                || GameManager.Instance.Helpers == null)
            {
                Debug.LogError("Null game managager");
            }

            Sprite grayScaledSprite = GameManager.Instance.Helpers.MakeTextureGrayScaled(IconImage.sprite.texture,
                                                                                         IconImage.sprite);
            IconImage.sprite = grayScaledSprite;
        }

        //Debug.Log("Making grayscaled " + ItemNameText.text);
    }
}
