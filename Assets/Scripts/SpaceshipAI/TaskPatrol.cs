using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private float _waitTime = 5f;
    private float _waitCounter = 0f;
    private bool _waiting = true;

    Transform shipTransform;
    EnemyControls _enemyControls;

    Vector3 destination;

    public TaskPatrol(EnemyControls enemyControls) 
    {
        _enemyControls = enemyControls;
        shipTransform = enemyControls.transform.GetChild(0);
        destination = shipTransform.position + shipTransform.forward * 50f;
        Debug.DrawLine(shipTransform.position, destination, Color.blue, 10f);
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
            // movement logic
            if (_enemyControls.MoveTowards(destination))
            {
                Debug.Log("destination reached");
                _waitCounter = 0f;
                _waiting = true;
            }

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
