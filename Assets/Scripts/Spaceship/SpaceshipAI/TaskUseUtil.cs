using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskUseUtil : Node
{
    private EnemyControls _enemyControls;
    float _minCooldown;
    float _maxCooldown;
    float cooldown;
    public TaskUseUtil(EnemyControls enemyControls, float minCooldown, float maxCooldown)
    {
        _enemyControls = enemyControls;
        _minCooldown = minCooldown;
        _maxCooldown = maxCooldown;
    }

    void GetNewCooldown()
    {
        cooldown = Random.Range(_minCooldown, _maxCooldown);
    }

    public override NodeState Evaluate()
    {
        cooldown -= Time.deltaTime;

        if (cooldown < 0f)
        {
            // Under missile lock and no utility currently active
            if (_enemyControls.InDanger && _enemyControls.ActiveUtils.Count <= 0)
            {
                _enemyControls.OnRandomUtility();
                GetNewCooldown();
            }
        }
        else
        {
            _enemyControls.OnCancelAllUtilities();
        }

        state = NodeState.RUNNING;
        return state;
    }
}
