using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapScene : MonoBehaviour
{
    public static WorldMapScene Instance;

    public void Awake()
    {
        Instance = this;
    }
}
