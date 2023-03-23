using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;

public class CheckForTarget : Node
{
    private Transform _transform;
    private List<ActorSpaceship> _possibleTargets;
    private float _detectTargetRange;

    public CheckForTarget(Transform transform, List<ActorSpaceship> possibleTargets, float detectTargetRange)
    {
        _transform = transform;
        _possibleTargets = possibleTargets;
        _detectTargetRange = detectTargetRange;
    }

    public override NodeState Evaluate()
    {
        Debug.Log("_possibleTargets: " + _possibleTargets.Count);
        object t = GetData("target");
        GameObject tGO = (GameObject)t;


        // no target, try to find a new one
        if (t == null || tGO == null) 
        {
            List<ActorSpaceship> targetsInRange = new List<ActorSpaceship>();
            foreach (ActorSpaceship possibleTarget in _possibleTargets)
            {
                if(Vector3.Distance(_transform.position, possibleTarget.ship.transform.position) < _detectTargetRange)
                {
                    targetsInRange.Add(possibleTarget);
                    Debug.Log("possibleTarget: " + possibleTarget);
                    parent.parent.SetData("target", possibleTarget.gameObject);
                    Debug.Log("found target");
                    state = NodeState.SUCCESS;
                    return state;

                }
            }
            //if(targetsInRange.Count > 0)
            //{
            //    ActorSpaceship closestTarget = targetsInRange[0];
            //    float shortestDistance = Vector3.Distance(closestTarget.ship.transform.position, _transform.position);
            //    foreach (ActorSpaceship targetInRange in targetsInRange)
            //    {
            //        float distance = Vector3.Distance(targetInRange.ship.transform.position, _transform.position);
            //        if(distance < shortestDistance)
            //        {
            //            closestTarget = targetInRange;
            //            shortestDistance = distance;
            //        }
            //    }
            //    Debug.Log("closestTarget: " + closestTarget);
            //    parent.parent.SetData("target", closestTarget.gameObject);
            //    Debug.Log("found target");
            //    state = NodeState.SUCCESS;
            //    return state;
            //}
            state = NodeState.FAILURE;
            return state;
        // no active targets or target fled too far
        }
        else if ((tGO != null && Vector3.Distance(_transform.position, tGO.transform.position) > _detectTargetRange))
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
