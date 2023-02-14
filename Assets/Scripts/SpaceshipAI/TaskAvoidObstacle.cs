using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskAvoidObstacle : Node
{

    Transform _shipTransform;
    EnemyControls _enemyControls;
    float _rayLength;
    float _avoidTime = 5f;
    float _avoidCounter = 0f;
    bool _avoiding = true;
    float disableBoostDistance = 30f;

    public TaskAvoidObstacle(EnemyControls enemyControls, Transform shipTransform, float rayLength = 50f)
    {
        _enemyControls = enemyControls;
        _shipTransform = shipTransform;
        _rayLength = rayLength;
    }

    public override NodeState Evaluate()
    {
        object data = GetData("obstacle");
        RaycastHit hitData = (RaycastHit)data;
        _enemyControls.Avoid(hitData.collider, disableBoostDistance);
        Debug.Log("avoiding");

        state = NodeState.RUNNING;
        return state;    
    }
}