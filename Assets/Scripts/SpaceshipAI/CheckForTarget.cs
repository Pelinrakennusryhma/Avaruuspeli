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
        object t = GetData("target");
        GameObject tGO = (GameObject)t;

        if(tGO != null)
        {
            Debug.Log("tGOparent: " + tGO.transform.parent.gameObject.activeSelf);
        }

        List<GameObject> activeTargets = _possibleTargets.FindAll(t => t.transform.parent.gameObject.activeSelf == true);

        // no target, try to find a new one
        if (t == null) 
        {
            foreach (GameObject possibleTarget in activeTargets)
            {
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
        // no active targets or target fled too far
        }
        else if (activeTargets.Count <= 0 || Vector3.Distance(_transform.position, tGO.transform.position) > SpaceshipBT.detectTargetRange)
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
