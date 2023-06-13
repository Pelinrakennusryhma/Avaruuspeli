using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapScene : MonoBehaviour
{
    public static WorldMapScene Instance;

    public Canvas UICanvas;

    public void Awake()
    {
        Instance = this;
    }

    public void HideCanvas()
    {
        UICanvas.gameObject.SetActive(false);
    }

    public void ShowCanvas()
    {
        UICanvas.gameObject.SetActive(true);
    }
}
