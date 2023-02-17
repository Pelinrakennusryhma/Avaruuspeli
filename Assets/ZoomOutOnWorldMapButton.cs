using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoomOutOnWorldMapButton : MonoBehaviour
{
    public TextMeshProUGUI textMeshProUGUI;

    public void Init()
    {
        textMeshProUGUI = GetComponentInChildren<TextMeshProUGUI>();       
    }

    public void HideButton()
    {
        gameObject.SetActive(false);
    }

    public void ShowButton()
    {
        gameObject.SetActive(true);
    }

    public void OnZoomOut()
    {
        //Debug.Log("Should zoom out");
        WorldMapMouseController.Instance.ZoomOut();

        if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
        {
            MotherShipOnWorldMapController.Instance.MoveToGalaxyPos();
        }

        else if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.Universe)
        {
            MotherShipOnWorldMapController.Instance.MoveToUniversePos();
        }
    }


    public void SetZoomText()
    {
        if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.Galaxy)
        {
            textMeshProUGUI.text = "To Universe";
        }

        else if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.StarSystem)
        {
            textMeshProUGUI.text = "To Galaxy";
        }
    }
}
