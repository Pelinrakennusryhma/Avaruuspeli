using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject layout;
    [SerializeField] private TextMeshProUGUI playerMoney;
    public ItemDatabase itemDatabase;
    public Item itemToAdd;
    public CanvasScript canvasScript;

    void Start()
    {
        NewItem(0);
        NewItem(1);
        NewItem(8);
        NewItem(9);
        NewItem(10);
    }

    void Update()
    {
        //Päivittää pelaajan rahat
        playerMoney.text = canvasScript.money.ToString("0.00") + "€";
    }
    //Lisää kauppaan uuden itemin myyntiin ID:n mukaan.
    public void NewItem(int id)
    {
        itemToAdd = itemDatabase.GetItem(id);
        Object prefab = Resources.Load("Prefabs/ShopItem");
        GameObject newItem = Instantiate(prefab, layout.transform) as GameObject;
        newItem.name = itemToAdd.id.ToString();
    }
}
