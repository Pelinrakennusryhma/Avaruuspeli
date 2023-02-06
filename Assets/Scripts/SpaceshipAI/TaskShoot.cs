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
        object obj = GetData("shouldShoot");

        bool shouldShoot = (bool)obj;

       _spaceshipShoot.shooting = shouldShoot;


        state = NodeState.RUNNING;
        return state;
    }
}
