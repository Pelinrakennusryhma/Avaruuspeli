using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipECM : Useable
{
    protected override IEnumerator Activate()
    {
        Actor.ActivateUtil(this);
        Actor.ClearLockedMissiles();
        EnableVisuals();
        yield return new WaitForSeconds(Duration);
        DisableVisuals();
        Actor.DeactivateUtil(this);
    }

    void EnableVisuals()
    {
        if(spaceshipEffects != null)
        {
            spaceshipEffects.Electrify();
        }
    }

    void DisableVisuals()
    {
        if (spaceshipEffects != null)
        {
            spaceshipEffects.UnElectrify();
        }
    }

}
