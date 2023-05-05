using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipShield : Useable
{
    protected override IEnumerator Activate()
    {
        Actor.ActivateUtil(this);
        yield return new WaitForSeconds(Duration);
        Actor.DeactivateUtil(this);
    }

}
