using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterStarOnWorldMap : MonoBehaviour
{
    public StarSystemOnFocus ParentStarSystem;

    public void Init(StarSystemOnFocus parentStarSystem)
    {
        ParentStarSystem = parentStarSystem;
    }
}
