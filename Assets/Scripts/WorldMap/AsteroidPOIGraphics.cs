using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPOIGraphics : MonoBehaviour
{
    [SerializeField]
    GameObject[] graphics;

    void Start()
    {
        EnableRandomGraphic();
    }

    void EnableRandomGraphic()
    {
        int enabledGraphics = Random.Range(0, graphics.Length);
        for (int i = 0; i < graphics.Length; i++)
        {
            if(i == enabledGraphics)
            {
                graphics[i].SetActive(true);
            } else
            {
                graphics[i].SetActive(false);
            }
        }
    }
}
