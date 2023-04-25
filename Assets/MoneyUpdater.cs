using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUpdater : MonoBehaviour
{
    public TextMeshProUGUI moneyAmount;

    // Start is called before the first frame update
    private void Awake()
    {
        moneyAmount = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        moneyAmount.text = GameManager.Instance.InventoryController.Money.ToString("0.00") + "€";
    }
}
