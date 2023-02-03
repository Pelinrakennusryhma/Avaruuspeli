using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class TaskShoot : Node
{
    private SpaceshipShoot _spaceshipShoot;
    public TaskShoot(SpaceshipShoot spaceshipShoot)
    {
        _spaceshipShoot = spaceshipShoot;
    }

    public override NodeState Evaluate()
    {
        bool shouldShoot = (bool)GetData("shouldShoot");
        _spaceshipShoot.shooting = shouldShoot;

        state = NodeState.RUNNING;
        return state;
    }
}
