using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSurface : MonoBehaviour
{
    public DestroyableRock[] Rocks;

    private void Awake()
    {
        for (int i = 0; i < Rocks.Length; i++)
        {
            //Rocks[i].gameObject.SetActive(true);
        }
    }
}
