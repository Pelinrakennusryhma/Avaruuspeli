using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskMoveToPosition : Node
{
    EnemyControls _enemyControls;
    bool _waiting = false;
    Vector3 _destination;

    public TaskMoveToPosition(EnemyControls enemyControls, Transform shipTransform)
    {
        _enemyControls = enemyControls;
        _destination = shipTransform.position + shipTransform.forward * 500;
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            Debug.Log("waiting");
        }
        else
        {
            Debug.Log("moving");
            if (_enemyControls.MoveToPosition(_destination, 50f, true))
            {
                Debug.Log("reached destination");
                _waiting = true;
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }
}