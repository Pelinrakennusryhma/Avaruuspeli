using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForTarget : Node
{
    private Transform _transform;
    private List<ActorSpaceship> _possibleTargets;

    public CheckForTarget(Transform transform, List<ActorSpaceship> possibleTargets)
    {
        _transform = transform;
        _possibleTargets = possibleTargets;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("_possibleTargets: " + _possibleTargets.Count);
        object t = GetData("target");
        GameObject tGO = (GameObject)t;


        // no target, try to find a new one
        if (t == null) 
        {
            foreach (ActorSpaceship possibleTarget in _possibleTargets)
            {
                if(Vector3.Distance(_transform.position, possibleTarget.ship.transform.position) < SpaceshipBT.detectTargetRange)
                {
                    Debug.Log("possibleTarget: " + possibleTarget);
                    parent.parent.SetData("target", possibleTarget.gameObject);
                    Debug.Log("found target");
                    state = NodeState.SUCCESS;
                    return state;
       
                }
            }
            state = NodeState.FAILURE;
            return state;
        // no active targets or target fled too far
        }
        else if (_possibleTargets.Count <= 0 || Vector3.Distance(_transform.position, tGO.transform.position) > SpaceshipBT.detectTargetRange)
        {
            ClearData("target");
            Debug.Log("cleared target");
            state = NodeState.RUNNING;
            return state;
        // target found
        } else
        {
            state = NodeState.SUCCESS;
            return state;
        }

    }
}
