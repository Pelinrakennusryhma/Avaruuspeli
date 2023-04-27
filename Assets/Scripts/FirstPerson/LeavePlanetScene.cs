using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavePlanetScene : ButtonHeldAction
{
    protected override bool CanTrigger()
    {
        // Check if player can leave the scene. In combat, shop open etc. ?
        return true;
    }

    protected override void OnSuccess()
    {
        GameManager.Instance.GoBackToWorldMap();
    }
}
