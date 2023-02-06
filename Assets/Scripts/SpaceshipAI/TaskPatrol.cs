using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private float _waitTime = 1f;
    private float _waitCounter = 0f;
    private bool _waiting = false;

    public TaskPatrol() { }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            _waitCounter += Time.deltaTime;
            if(_waitCounter >= _waitTime)
            {
                _waiting = false;
            }
        } else
        {
            // movement logic
            Debug.Log("next");
            _waitCounter = 0f;
            _waiting = true;
        }

        state = NodeState.RUNNING;
        return state;
    }
}
