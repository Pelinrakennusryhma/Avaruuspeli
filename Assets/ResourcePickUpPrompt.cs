using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourcePickUpPrompt : MonoBehaviour
{
    public static ResourcePickUpPrompt Instance;
    private TextMeshProUGUI textMeshPro;

    private float HideTimer;

    public void Awake()
    {
        Instance = this;
        textMeshPro = GetComponentInChildren<TextMeshProUGUI>(true);
        textMeshPro.text = "";
        textMeshPro.gameObject.SetActive(false);
    }

    public void ShowResource(ResourceInventory.ResourceType resourceType,
                             int amount)
    {
        HideTimer = 3.0f;
        textMeshPro.gameObject.SetActive(true);




        switch (resourceType)
        {
            case ResourceInventory.ResourceType.None:
                break;
            case ResourceInventory.ResourceType.TestDice:
                textMeshPro.text = "Dice: " + amount.ToString();
                break;

            case ResourceInventory.ResourceType.Gold:
                textMeshPro.text = "Gold: " + amount.ToString();
                break;

            case ResourceInventory.ResourceType.Silver:
                textMeshPro.text = "Silver: " + amount.ToString();
                break;

            case ResourceInventory.ResourceType.Copper:
                textMeshPro.text = "Copper: " + amount.ToString();
                break;

            case ResourceInventory.ResourceType.Iron:
                textMeshPro.text = "Iron: " + amount.ToString();
                break;

            case ResourceInventory.ResourceType.Diamond:
                textMeshPro.text = "Diamonds: " + amount.ToString();
                break;

            default:
                break;
        }        
        
        //Debug.Log("resource type is " + resourceType.ToString());
    }

    public void Update()
    {
        if (HideTimer > 0.0f)
        {
            HideTimer -= Time.deltaTime;

            if (HideTimer <= 0.0f)
            {
                textMeshPro.gameObject.SetActive(false);
            }
        }
    }
}
