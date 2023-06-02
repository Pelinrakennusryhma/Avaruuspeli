using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipShield : Useable
{
    public override void Init(ShipUtility data, ActorSpaceship actor)
    {
        base.Init(data, actor);
        soundEffect = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.Shield);
        RuntimeManager.AttachInstanceToGameObject(soundEffect, transform, rb);
    }
    protected override IEnumerator Activate()
    {
        Actor.ActivateUtil(this);
        spaceshipEffects.Shield();
        yield return new WaitForSeconds(Duration);
        spaceshipEffects.UnShield();
        Actor.DeactivateUtil(this);
    }

}
