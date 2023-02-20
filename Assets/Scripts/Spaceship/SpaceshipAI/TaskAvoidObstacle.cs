using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskAvoidObstacle : Node
{
    EnemyControls _enemyControls;
    float disableBoostDistance = 30f;

    public TaskAvoidObstacle(EnemyControls enemyControls, Transform shipTransform)
    {
        _enemyControls = enemyControls;
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