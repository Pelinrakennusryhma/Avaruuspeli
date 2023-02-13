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
        object data = GetData("obstacle");
        RaycastHit hitData = (RaycastHit)data;
        _enemyControls.Avoid(hitData.collider);

        state = NodeState.RUNNING;
        return state;
        //object data = GetData("avoidHit");
        //RaycastHit avoidPosition = (RaycastHit)data;
        //Vector3 hitRelative = _shipTransform.InverseTransformPoint(avoidPosition.point);

        //Vector3 moveTowards = new Vector3(-hitRelative.x * 2f, -hitRelative.y * 2f, hitRelative.z);

        //_enemyControls.MoveTowards(moveTowards);




        //Debug.Log(avoidPosition);
        //Debug.Log(hitRelative);
        //Debug.Log(moveTowards);



        //_avoidCounter += Time.deltaTime;
        //if (_avoidCounter >= _avoidTime)
        //{
        //    Debug.Log("no longer avoiding");
        //    _avoiding = false;
        //}
        //else
        //{
        //    //Debug.DrawRay(_shipTransform.position, avoidPosition, Color.magenta);
        //    //Debug.DrawRay(_shipTransform.position, moveTowards, Color.cyan);
        //    _enemyControls.MoveTowards(moveTowards, false);
        //    Debug.Log("avoiding");
        //}




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
    }
}