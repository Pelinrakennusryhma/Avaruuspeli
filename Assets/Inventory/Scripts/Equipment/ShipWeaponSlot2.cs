using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShipWeaponSlot2 : MonoBehaviour, IPointerClickHandler
{
    public ContextMenu contextMenuScript;
    public Equipment equipment;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (equipment.equippedShipWeapon2 == null)
            {
                //Debug.LogWarning("Nothing equipped. Returning.");
                return;
            }

            contextMenuScript.HideAll();
            contextMenuScript.ShowUnequip2();
            contextMenuScript.SetPositionToMouse();
            contextMenuScript.itemID = equipment.equippedShipWeapon2.id;
        }
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            contextMenuScript.HideMenu();
        }
    }
}
