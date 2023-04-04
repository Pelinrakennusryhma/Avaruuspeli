using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveSpaceshipSceneHandler : UITrackable
{
    [SerializeField]
    ActorManager actorManager;
    [SerializeField]
    MothershipHangar mothershipHangar;
    [SerializeField]
    GameObject successObject;
    [SerializeField]
    GameObject failureObject;
    bool leavingScene = false;
    float buttonHeldFor = 0f;
    void Awake()
    {
        GameEvents.Instance.EventLeavingSceneStarted.AddListener(OnEventLeavingSceneStarted);
        GameEvents.Instance.EventLeavingSceneCancelled.AddListener(OnEventLeavingSceneCancelled);
        successObject.SetActive(false);
    }

    void OnEventLeavingSceneStarted()
    {
        leavingScene = true;

        if (actorManager.SceneCleared || mothershipHangar.PlayerShipInHangar)
        {
            successObject.SetActive(true);
        } else
        {
            failureObject.SetActive(true);
        }
    }

    void OnEventLeavingSceneCancelled()
    {
        leavingScene = false;
        buttonHeldFor = 0f;

        successObject.SetActive(false);
        failureObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (leavingScene)
        {
            buttonHeldFor += Time.deltaTime;
        }
    }

    public override float MaxValue
    {
        get
        {
            return Globals.Instance.leaveSpaceshipSceneDelay;
        }
    }

    public override float CurrentValue
    {
        get
        {
            return buttonHeldFor;
        }
    }
}
