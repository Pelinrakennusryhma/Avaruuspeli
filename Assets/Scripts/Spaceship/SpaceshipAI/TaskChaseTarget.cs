using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskChaseTarget : Node
{
    EnemyControls _enemyControls;
    float _projectileSpeed;
    Transform _shipTransform;

    public TaskChaseTarget(EnemyControls enemyControls, Transform shipTransform, float projectileSpeed)
    {
        _shipTransform = shipTransform;
        _projectileSpeed = projectileSpeed;
        _projectileSpeed = projectileSpeed;
        _enemyControls = enemyControls;
    }

    public override NodeState Evaluate()
    {
        GameObject target = (GameObject)GetData("target");

        if(target != null)
        {
            TargetProjection tp = target.GetComponent<TargetProjection>();
            Vector3 pos = tp.GetPosition(_projectileSpeed, _shipTransform.position);
            parent.SetData("shootTargetPos", pos);
            _enemyControls.MoveToPosition(pos, 50f);
        } else
        {
            ClearData("shootTargetPos");
            _enemyControls.Stop();
        }


        state = NodeState.RUNNING;
        return state;
    }
}
