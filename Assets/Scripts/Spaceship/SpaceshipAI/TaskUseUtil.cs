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

    float _minReactionTime;
    float _maxReactionTime;
    float reactionTimer;
    bool reacting = false;

    public TaskUseUtil(EnemyControls enemyControls, float minCooldown, float maxCooldown, float minReactionTime, float maxReactionTime)
    {
        _enemyControls = enemyControls;
        _minCooldown = minCooldown;
        _maxCooldown = maxCooldown;
        _minReactionTime = minReactionTime;
        _maxReactionTime = maxReactionTime;
    }

    void GetNewCooldown()
    {
        cooldown = Random.Range(_minCooldown, _maxCooldown);
    }

    void GetNewReactionTime()
    {
        reactionTimer = Random.Range(_minReactionTime, _maxReactionTime);
    }

    public override NodeState Evaluate()
    {
        cooldown -= Time.deltaTime;
        reactionTimer -= Time.deltaTime;

        if (cooldown < 0f)
        {
            if (_enemyControls.InDanger && _enemyControls.ActiveUtils.Count <= 0 && !reacting)
            {
                reacting = true;
                GetNewReactionTime();
                //Debug.Log("New Reaction: " + reactionTimer);
            }

            if (reacting && reactionTimer < 0f)
            {
                reacting = false;
                _enemyControls.OnRandomUtility();
                GetNewCooldown();
                //Debug.Log("Acting!");
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
