using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherbaleDiamond : GatherableObject
{
    public void Awake()
    {
        OffsetFromGround = -0.2f;
        //ResourceType = ResourceInventory.ResourceType.Diamond;
    }
}
