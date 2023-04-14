using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeightUpdater : MonoBehaviour
{
    public TextMeshProUGUI weightText;

    // Start is called before the first frame update
    private void Awake()
    {
        weightText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //float currentWeight = Mathf.Round((float)GameManager.Instance.InventoryController.Inventory.currentWeight);
        //float maximumWeight = Mathf.Round((float)GameManager.Instance.InventoryController.Inventory.maxWeight);        
        //weightText.text = "Weight: " + currentWeight.ToString("0.0") +
        //          " / " + maximumWeight.ToString("0.0");
        weightText.text = "Weight: " + GameManager.Instance.InventoryController.Inventory.currentWeight.ToString("0.0") +
                          " / " + GameManager.Instance.InventoryController.Inventory.maxWeight.ToString("0.0");



    }
}
