using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class TaskAvoidObstacle : Node
{

    Transform _shipTransform;
    EnemyControls _enemyControls;
    float _rayLength;
    float _avoidTime = 5f;
    float _avoidCounter = 0f;
    bool _avoiding = true;

    public TaskAvoidObstacle(EnemyControls enemyControls, Transform shipTransform, float rayLength = 50f)
    {
        _enemyControls = enemyControls;
        _shipTransform = shipTransform;
        _rayLength = rayLength;
    }

    public override NodeState Evaluate()
    {
        object data = GetData("avoidPosition");
        Vector3 avoidPosition = (Vector3)data;
        Vector3 hitRelative = _shipTransform.InverseTransformPoint(avoidPosition);
        Vector3 moveTo = new Vector3(
            avoidPosition.x + hitRelative.x,
            avoidPosition.y + hitRelative.y,
            avoidPosition.z);
        //Debug.Log(avoidPosition);

        _avoidCounter += Time.deltaTime;
        if (_avoidCounter >= _avoidTime)
        {
            Debug.Log("no longer avoiding");
            _avoiding = false;
            state = NodeState.FAILURE;
        } else
        {
            _enemyControls.MoveTowards(moveTo, 5f, false);
            Debug.Log("avoiding");
            state = NodeState.SUCCESS;
        }




        //// find a non hitting side ray
        //Vector3 dir = Quaternion.Euler(0, 30, 0) * _shipTransform.forward;
        //RaycastHit hit;
        //if (Physics.Raycast(_shipTransform.position + new Vector3(0, 10, 0), dir, out hit, _rayLength))
        //{
        //    Debug.DrawRay(_shipTransform.position, dir * hit.distance, Color.red);
        //    // start a timer to move to a free position
        //    state = NodeState.FAILURE;
        //}
        //else
        //{
        //    Debug.DrawRay(_shipTransform.position, dir * 1000, Color.green);
        //    Debug.Log("Did not Hit");
        //    state = NodeState.FAILURE;

        //}

        return state;
    }
}