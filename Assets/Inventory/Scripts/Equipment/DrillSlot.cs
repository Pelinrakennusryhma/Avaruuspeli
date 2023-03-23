using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrillSlot : MonoBehaviour, IPointerClickHandler
{
    public ContextMenu contextMenuScript;
    public Equipment equipment;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            contextMenuScript.HideAll();
            contextMenuScript.ShowUnequip();
            contextMenuScript.SetPositionToMouse();
            contextMenuScript.itemID = equipment.equippedDrill.id;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
        }
    }
}
