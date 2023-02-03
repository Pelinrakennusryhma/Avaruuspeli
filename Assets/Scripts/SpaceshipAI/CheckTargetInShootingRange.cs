using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class CheckTargetInShootingRange : Node
{
    private Transform _transform;

    public CheckTargetInShootingRange(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        // check if should fire

        object t = GetData("target");
        if (t == null)
        {
            parent.parent.SetData("shouldShoot", false);
            state = NodeState.FAILURE;
            return state;
        }

        GameObject target = (GameObject)t;

        Vector3 heading = target.transform.position - _transform.position;
        float dot = Vector3.Dot(heading, _transform.forward);

        if (dot > 0.8f)
        {
            parent.parent.SetData("shouldShoot", true);
            state = NodeState.SUCCESS;
            return state;
        }

        parent.parent.SetData("shouldShoot", false);
        state = NodeState.FAILURE;
        return state;
    }
}
