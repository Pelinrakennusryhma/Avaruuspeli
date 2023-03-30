using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableGold : GatherableObject
{
    public void Awake()
    {
        OffsetFromGround = -0.2f;
        //ResourceType = ResourceInventory.ResourceType.Gold;
    }
}
