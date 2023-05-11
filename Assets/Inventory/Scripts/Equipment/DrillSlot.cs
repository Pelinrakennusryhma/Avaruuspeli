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
            if (equipment.equippedObjectInHands == null)
            {
                //Debug.LogWarning("Nothing equipped. Returning");
                return;
            }

            contextMenuScript.HideAll();
            contextMenuScript.ShowUnequip();
            contextMenuScript.SetPositionToMouse();
            contextMenuScript.itemID = equipment.equippedObjectInHands.id;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
        }
    }
}
