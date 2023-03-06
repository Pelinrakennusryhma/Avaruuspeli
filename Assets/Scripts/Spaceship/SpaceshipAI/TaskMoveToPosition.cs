using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskMoveToPosition : Node
{
    EnemyControls _enemyControls;
    List<Vector3> _destinations = new List<Vector3>();
    private float _waitTime = 2f;
    private float _waitCounter = 0f;
    private bool _waiting = true;
    bool needsNewDestination = true;
    int destinationId = 0;
    Vector3 destination;

    public TaskMoveToPosition(EnemyControls enemyControls, Transform shipTransform)
    {
        _enemyControls = enemyControls;

        _destinations.Add(shipTransform.position);
        _destinations.Add(shipTransform.position + shipTransform.forward * 200);

    }

    public override NodeState Evaluate()
    {
        if (_waiting)
        {
            Debug.Log("waiting");
            _waitCounter += Time.deltaTime;
            if (_waitCounter >= _waitTime)
            {
                _waiting = false;
            }
        }
        else
        {
            if (needsNewDestination)
            {
                Debug.Log("new destination");
                GetNextDestination();
                needsNewDestination = false;
            }

            if (_enemyControls.MoveToPosition(destination, 50f, true))
            {
                Debug.Log("reached destination");
                needsNewDestination = true;
                _waitCounter = 0f;
                _waiting = true;
            }
        }

        state = NodeState.SUCCESS;
        return state;
    }

    void GetNextDestination()
    {
        if(destinationId == _destinations.Count - 1)
        {
            destinationId = 0;
        } else
        {
            destinationId++;
        }
        destination = _destinations[destinationId];
    }
}