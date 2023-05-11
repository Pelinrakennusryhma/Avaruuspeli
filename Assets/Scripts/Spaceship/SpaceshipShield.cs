using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipShield : Useable
{
    protected override IEnumerator Activate()
    {
        Actor.ActivateUtil(this);
        spaceshipEffects.Shield();
        yield return new WaitForSeconds(Duration);
        spaceshipEffects.UnShield();
        Actor.DeactivateUtil(this);
    }

}
