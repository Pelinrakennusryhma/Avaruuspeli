using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskPatrol : Node
{
    private float _waitTime = 3f;
    private float _waitCounter = 0f;
    private bool _waiting = true;
    private float _patrolArea = 0f;

    EnemyControls _enemyControls;
    Vector3 startPos;
    Vector3 destination = Vector3.zero;
    bool needsNewDestination = true;

    public TaskPatrol(EnemyControls enemyControls, float patrolArea) 
    {
        _enemyControls = enemyControls;
        startPos = enemyControls.transform.position;
        _patrolArea = patrolArea;
    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            //Debug.Log("waiting");
            _waitCounter += Time.deltaTime;
            if(_waitCounter >= _waitTime)
            {
                _waiting = false;
            }
        } else
        {
            if(needsNewDestination)
            {
                //Debug.Log("new destination");
                destination = GetNewPatrolPosition();
                needsNewDestination = false;
            }

            if (_enemyControls.MoveToPosition(destination, 50f, true))
            {
                //Debug.Log("reached destination");
                needsNewDestination = true;
                _waitCounter = 0f;
                _waiting = true;
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }

    Vector3 GetNewPatrolPosition()
    {
        return startPos + Random.insideUnitSphere * _patrolArea;
    }
}
