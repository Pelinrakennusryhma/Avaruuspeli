using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveSpaceshipSceneHandler : UITrackable
{
    bool leavingScene = false;
    float buttonHeldFor = 0f;
    void Awake()
    {
        GameEvents.Instance.EventLeavingSceneStarted.AddListener(OnEventLeavingSceneStarted);
        GameEvents.Instance.EventLeavingSceneCancelled.AddListener(OnEventLeavingSceneCancelled);
        gameObject.SetActive(false);
    }

    void OnEventLeavingSceneStarted()
    {
        leavingScene = true;
    }

    void OnEventLeavingSceneCancelled()
    {
        leavingScene = false;
        buttonHeldFor = 0f;
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
