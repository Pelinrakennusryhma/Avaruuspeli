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
        object t = GetData("target");

        if(t == null)
        {
            parent.parent.SetData("shouldShoot", false);
            state = NodeState.RUNNING;
            return state;
        }

        GameObject target = (GameObject)t;

        Vector3 heading = target.transform.position - _transform.position;
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
