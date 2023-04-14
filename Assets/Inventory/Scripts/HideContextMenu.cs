using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HideContextMenu : MonoBehaviour, IPointerClickHandler
{
    ///private GameObject contextMenu;
    private ContextMenu contextMenuScript;

    void Start()
    {
        //contextMenu = GameObject.Find("ContextMenu");
        //Debug.LogError("Replace the find with something else!!!");
        //contextMenuScript = contextMenu.GetComponent<ContextMenu>();
        contextMenuScript = GameManager.Instance.InventoryController.ContextMenuScript;
    }

    //Painettaessa piilottaa context menun. Tuhoaa myös info paneelin
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            contextMenuScript.HideMenu();
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
            GameObject[] gos = GameObject.FindGameObjectsWithTag("InfoPanel");

            foreach (GameObject go in gos)
            {
                Destroy(go);
            }
        }
    }
}
