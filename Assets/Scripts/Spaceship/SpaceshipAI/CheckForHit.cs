using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckForHit : Node
{
    SpaceshipEvents _spaceshipEvents;
    int hitsBeforeEvading = 3;
    float hitsOverSeconds = 1f;
    int hits = 0;
    float hitTimer = 0f;
    public CheckForHit(SpaceshipEvents spaceshipEvents)
    {
        _spaceshipEvents = spaceshipEvents;
        _spaceshipEvents.EventSpaceshipHealthChanged.AddListener(OnHealthChanged);
    }

    void OnHealthChanged()
    {
        hits++;
    }

    public override NodeState Evaluate()
    {
        // run timer once we've been hit
        if (hits > 0)
        {
            hitTimer += Time.deltaTime;
        }

        // hit enough over the duration, set bool for evade task
        if (hits > hitsBeforeEvading)
        {
            parent.SetData("evading", true);
        }

        // not hit enough times during the duration, reset
        if (hitTimer > hitsOverSeconds)
        {
            hitTimer = 0f;
            hits = 0;
        }

        return NodeState.RUNNING;
    }
}
