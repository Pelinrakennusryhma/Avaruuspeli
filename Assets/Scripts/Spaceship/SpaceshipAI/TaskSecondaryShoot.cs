using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskSecondaryShoot : Node
{
    private EnemyControls _enemyControls;
    float _minCooldown;
    float _maxCooldown;
    float cooldown = 0f;
    public TaskSecondaryShoot(EnemyControls enemyControls, float minCooldown, float maxCooldown)
    {
        _enemyControls = enemyControls;
        _minCooldown = minCooldown;
        _maxCooldown = maxCooldown;
        GetNewCooldown();
    }

    void GetNewCooldown()
    {
        cooldown = Random.Range(_minCooldown, _maxCooldown);
    }

    public override NodeState Evaluate()
    {
        cooldown -= Time.deltaTime;

        if(cooldown < 0f)
        {
            object obj = GetData("shouldShoot");

            bool shouldShoot = (bool)obj;

            _enemyControls.OnSecondaryShoot(shouldShoot);
            GetNewCooldown();
        }
        
        state = NodeState.FAILURE;
        return state;
    }
}
