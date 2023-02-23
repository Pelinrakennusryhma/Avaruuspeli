using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskEvadeAttacker : Node
{
    EnemyControls _enemyControls;
    float fleeDistance = 2000f;
    float fleeDuration = 3f;
    float fleeTimer = 0f;
    Vector3 fleePosition = Vector3.zero;
    public TaskEvadeAttacker(EnemyControls enemyControls)
    {
        _enemyControls = enemyControls;
    }

    public override NodeState Evaluate()
    {
        object data = GetData("evading");
        if(data == null)
        {
            return NodeState.FAILURE;
        }

        bool evading = (bool)data;
        if(evading == false)
        {
            return NodeState.FAILURE;
        }

        // duration over, stop evasion behavior
        if (fleeTimer > fleeDuration)
        {
            parent.SetData("evading", false);
            fleeTimer = 0f;
            fleePosition = Vector3.zero;
            return NodeState.FAILURE;
        }

        // start timer and evade;
        if (fleePosition == Vector3.zero)
        {
            GetRandomPosition();
            Debug.Log("new fleePosition: " + fleePosition);
        }
        Debug.Log("evading: " + fleePosition);
        fleeTimer += Time.deltaTime;
        _enemyControls.MoveToPosition(fleePosition, 100f, true);
        return NodeState.SUCCESS;
    }

    void GetRandomPosition()
    {
        fleePosition = Random.insideUnitSphere.normalized * fleeDistance;
    }
}
