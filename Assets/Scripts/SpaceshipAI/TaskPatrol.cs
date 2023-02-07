using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private float _waitTime = 0.1f;
    private float _waitCounter = 0f;
    private bool _waiting = true;

    Transform _targetTransform;
    EnemyControls _enemyControls;

    public TaskPatrol(EnemyControls enemyControls, Transform targetTransform) 
    {
        _targetTransform = targetTransform;
        _enemyControls = enemyControls;
    }

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
            Debug.Log("idling");
            // movement logic
            //if (_enemyControls.MoveTowards(_targetTransform.position))
            //{
            //    Debug.Log("destination reached");
            //    _waitCounter = 0f;
            //    _waiting = true;
            //}

        }

        state = NodeState.RUNNING;
        return state;
    }

    //void MoveTowards(Vector3 position)
    //{
    //    _enemyControls.OnThrust(1f);
    //}

    //void Stop()
    //{
    //    //Debug.Log("stop");
    //    _enemyControls.OnThrust(0f);
    //}

    //bool ReachedDestination()
    //{
    //    if(Vector3.Distance(shipTransform.position, destination) < 0.1f)
    //    {
    //        //Debug.Log("reached destination" + Vector3.Distance(shipTransform.position, destination));
    //        return true;
    //    }
    //    return false;
    //}
}
