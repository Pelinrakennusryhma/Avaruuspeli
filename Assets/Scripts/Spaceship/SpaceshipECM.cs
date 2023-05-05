using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipECM : Useable
{
    protected override IEnumerator Activate()
    {
        Actor.ActivateUtil(this);
        Actor.ClearLockedMissiles();
        yield return new WaitForSeconds(Duration);
        Actor.DeactivateUtil(this);
    }

}
