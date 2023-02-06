using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForTarget : Node
{
    private Transform _transform;
    private List<GameObject> _possibleTargets;

    public CheckForTarget(Transform transform, List<GameObject> possibleTargets)
    {
        _transform = transform;
        _possibleTargets = possibleTargets;
    }

    public override NodeState Evaluate()
    {
        GameObject t = (GameObject)GetData("target");
        if(t == null)
        {
            foreach (GameObject possibleTarget in _possibleTargets)
            {
                //Debug.Log(Vector3.Distance(_transform.position, possibleTarget.transform.position));
                if(Vector3.Distance(_transform.position, possibleTarget.transform.position) < SpaceshipBT.detectTargetRange)
                {
                    parent.parent.SetData("target", possibleTarget);
                    Debug.Log("found target");
                    state = NodeState.SUCCESS;
                    return state;
       
                }
            }
            state = NodeState.FAILURE;
            return state;

        } else if (Vector3.Distance(_transform.position, t.transform.position) > SpaceshipBT.detectTargetRange)
        {
            ClearData("target");
            Debug.Log("cleared target");
            state = NodeState.FAILURE;
            return state;
        }

        state = NodeState.SUCCESS;
        return state;
    }
}
