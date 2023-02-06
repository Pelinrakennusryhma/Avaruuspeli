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
        GameObject target = (GameObject)GetData("target");
        if(target != null)
        {
            Debug.Log("Chasing");
        }


        state = NodeState.RUNNING;
        return state;
    }
}
