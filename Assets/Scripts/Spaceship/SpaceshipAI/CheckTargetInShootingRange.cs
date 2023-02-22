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
        object t = GetData("shootTargetPos");

        if(t == null)
        {
            parent.parent.SetData("shouldShoot", false);
            state = NodeState.RUNNING;
            return state;
        }

        Vector3 targetPos = (Vector3)t;

        Vector3 heading = targetPos - _transform.position;
        float dot = Vector3.Dot(heading.normalized, _transform.forward);

        if (dot > 0.95f)
        {
            state = NodeState.SUCCESS;
            parent.parent.SetData("shouldShoot", true);
        } 
        else
        {
            parent.parent.SetData("shouldShoot", false);
        }

        state = NodeState.SUCCESS;
        return state;
    }
}