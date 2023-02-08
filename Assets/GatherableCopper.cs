using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableCopper : GatherableObject
{
    public void Awake()
    {
        OffsetFromGround = -0.2f;
        ResourceType = ResourceInventory.ResourceType.Copper;
    }
}
