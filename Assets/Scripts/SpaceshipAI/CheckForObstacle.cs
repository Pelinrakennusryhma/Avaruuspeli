using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class CheckForObstacle : Node
{
    Transform _shipTransform;
    float _rayLength;

    float shipWidth;
    float shipHeight;

    public CheckForObstacle(Transform shipTransform, float rayLength = 50f)
    {
        _shipTransform = shipTransform;
        _rayLength = rayLength;

        MeshCollider shipCollider = shipTransform.GetComponent<MeshCollider>();
        shipWidth = shipCollider.bounds.extents.x;
        shipHeight = shipCollider.bounds.extents.y;
    }

    public override NodeState Evaluate()
    {
        // failure on middle ray hit, store hit object
        // find a non hitting side ray
        // move towards end of side ray

        //// Damageable layer
        //int layerMask = 1 << 10;

        //// Invert to check everything but damageable because we might want to shoot that?
        //layerMask = ~layerMask;

        List<Vector3> rayOrigins = CreateRayOrigins();

        foreach (Vector3 rayOrigin in rayOrigins)
        {
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, _shipTransform.TransformDirection(Vector3.forward), out hit, _rayLength))
            {
                Debug.DrawRay(rayOrigin, _shipTransform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                parent.SetData("avoidPosition", hit.point);
                state = NodeState.SUCCESS;
                return state;
            }
            else
            {
                Debug.DrawRay(rayOrigin, _shipTransform.TransformDirection(Vector3.forward) * 1000, Color.white);
            }

            //ClearData("avoidPosition");
        }

        state = NodeState.FAILURE;
        return state;
    }

    List<Vector3> CreateRayOrigins()
    {
        List<Vector3> rayOrigins = new List<Vector3>();
        rayOrigins.Add(_shipTransform.position + _shipTransform.right * shipWidth);
        //rayOrigins.Add(_shipTransform.position + -_shipTransform.right * shipWidth);
        //rayOrigins.Add(_shipTransform.position + _shipTransform.up * shipHeight);
        //rayOrigins.Add(_shipTransform.position + -_shipTransform.up * shipHeight);
        return rayOrigins;
    }
}