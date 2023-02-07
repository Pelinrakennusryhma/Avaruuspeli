using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskShoot : Node
{
    private EnemyControls _enemyControls;
    public TaskShoot(EnemyControls enemyControls)
    {
        _enemyControls = enemyControls;
    }

    public override NodeState Evaluate()
    {
        object obj = GetData("shouldShoot");

        bool shouldShoot = (bool)obj;

        _enemyControls.OnShoot(shouldShoot);


        state = NodeState.RUNNING;
        return state;
    }
}
