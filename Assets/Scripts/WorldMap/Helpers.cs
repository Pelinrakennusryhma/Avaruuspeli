using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Helpers : MonoBehaviour
{
    public EventSystem EventSystem;
    private PointerEventData pointerEventData;
    private List<RaycastResult> raycastResults = new List<RaycastResult>();
    public GraphicRaycaster WorldMapCameraCanvasRaycaster;

    public void RefreshReferenceToGraphicsRaycasterAndEventSystem()
    {
        WorldMapCameraCanvasRaycaster = Camera.main.GetComponentInChildren<GraphicRaycaster>(true);
        EventSystem = FindObjectOfType<EventSystem>();
    }

    public bool CheckIfUIisHit()
    {
        bool hitUI = false;

        if (WorldMapCameraCanvasRaycaster != null)
        {
            pointerEventData = new PointerEventData(EventSystem);
            pointerEventData.position = Input.mousePosition;
            raycastResults = new List<RaycastResult>();
            WorldMapCameraCanvasRaycaster.Raycast(pointerEventData, raycastResults);

            for (int i = 0; i < raycastResults.Count; i++)
            {
                if (raycastResults[i].gameObject.layer == 5)
                {

                    hitUI = true;
                    break;
                }
            }
        }

        else
        {
            //Debug.LogError("Can't check if UI is hit, because worldmap main camera canvas is null");
        }

        if (hitUI)
        {
            //Debug.LogError("We hit UI");
        }

        else
        {
            //Debug.LogWarning("NOT hitting UI");
        }

        return hitUI;
    }

    private void Update()
    {
        CheckIfUIisHit();
    }
}
