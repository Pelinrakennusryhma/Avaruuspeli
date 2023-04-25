using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpacesuitSlot : MonoBehaviour, IPointerClickHandler
{
    public ContextMenu contextMenuScript;
    public Equipment equipment;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {            
            if (equipment.equippedSpacesuit == null)
            {
                //Debug.LogWarning("Equipped spacesuit is null. Returning");
                return;
            }

            contextMenuScript.HideAll();
            contextMenuScript.ShowUnequip();
            contextMenuScript.SetPositionToMouse();
            
            contextMenuScript.itemID = equipment.equippedSpacesuit.id;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
        }
    }
}
