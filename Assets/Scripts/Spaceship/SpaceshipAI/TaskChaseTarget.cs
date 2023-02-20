using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskChaseTarget : Node
{
    //private Transform _transform;
    EnemyControls _enemyControls;

    public TaskChaseTarget(EnemyControls enemyControls)
    {
        _enemyControls = enemyControls;
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject)GetData("target");
        if(target != null)
        {
            _enemyControls.MoveToPosition(target.transform.position, 50f);
        } else
        {
            _enemyControls.Stop();
        }


        state = NodeState.RUNNING;
        return state;
    }
}
