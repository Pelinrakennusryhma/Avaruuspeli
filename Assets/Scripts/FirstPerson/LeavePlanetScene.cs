using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeavePlanetScene : ButtonHeldAction
{
    protected override bool CanTrigger()
    {
        Debug.LogWarning("Leaving planet scene. Check here if player can leave the scene or not.. in combat, shop open etc. ?");
        return true;
    }

    protected override void OnSuccess()
    {
        GameManager.Instance.GoBackToWorldMap();
    }
}
