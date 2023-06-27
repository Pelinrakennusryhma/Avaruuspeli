using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveSpaceshipScene : ButtonHeldAction
{
    [SerializeField]
    ActorManager actorManager;
    [SerializeField]
    MothershipHangar mothershipHangar;

    protected override void Awake()
    {
        delay = Globals.Instance.leaveSpaceshipSceneDelay;
        base.Awake();
    }
    protected override bool CanTrigger()
    {
        return actorManager.SceneCleared || mothershipHangar.PlayerShipInHangar;
    }

    protected override void OnSuccess()
    {
        GameManager.Instance.GoBackToWorldMap();
    }
}
