using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWorldMapButton : MonoBehaviour
{
    public void GoToWorldMap()
    {
        GameManager.Instance.GoBackToWorldMap();
    }
}
