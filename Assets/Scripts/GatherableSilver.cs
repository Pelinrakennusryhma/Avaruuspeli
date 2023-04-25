using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableSilver : GatherableObject
{
    public void Awake()
    {
        OffsetFromGround = -0.2f;
        //ResourceType = ResourceInventory.ResourceType.Silver;
    }
}
