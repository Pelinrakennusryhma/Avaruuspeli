using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskChaseTarget : Node
{
    private Transform _transform;

    public TaskChaseTarget(Transform transform)
    {
        _transform = transform;
    }

    public override NodeState Evaluate()
    {
        // chase target
        GameObject target = (GameObject)GetData("target");
        if (target)
        {
            Debug.Log("Chasing");
        } else
        {
            Debug.Log("No target to chase");
        }
        

        state = NodeState.RUNNING;
        return state;
    }
}
