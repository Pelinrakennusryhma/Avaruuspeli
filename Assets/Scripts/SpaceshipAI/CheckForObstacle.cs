using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class CheckForObstacle : Node
{
    Transform _shipTransform;
    EnemyControls _enemyControls;
    float _rayLength;

    float shipWidth;
    float shipHeight;

    float hitCooldown = 0.1f;
    float hitTimer;

    public CheckForObstacle(EnemyControls enemyControls, Transform shipTransform, float rayLength = 50f)
    {
        _enemyControls = enemyControls;
        _shipTransform = shipTransform;
        _rayLength = rayLength;

        MeshCollider shipCollider = shipTransform.GetComponent<MeshCollider>();
        shipWidth = shipCollider.bounds.extents.x;
        shipHeight = shipCollider.bounds.extents.y;

        hitTimer = hitCooldown;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(_shipTransform.position, _rayLength);

    //    RaycastHit hit;
    //    float rayRadius = Mathf.Max(shipHeight, shipWidth);
    //    if (Physics.SphereCast(_shipTransform.position, rayRadius, _shipTransform.forward, out hit, _rayLength))
    //    {
    //        Gizmos.color = Color.green;
    //        Vector3 sphereCastMidpoint = _shipTransform.position + (_shipTransform.forward * hit.distance);
    //        Gizmos.DrawWireSphere(sphereCastMidpoint, rayRadius);
    //        Gizmos.DrawSphere(hit.point, 0.1f);
    //        Debug.DrawLine(_shipTransform.position, sphereCastMidpoint, Color.green);
    //    }
    //    else
    //    {
    //        Gizmos.color = Color.red;
    //        Vector3 sphereCastMidpoint = _shipTransform.position + (_shipTransform.forward * (_rayLength - rayRadius));
    //        Gizmos.DrawWireSphere(sphereCastMidpoint, rayRadius);
    //        Debug.DrawLine(_shipTransform.position, sphereCastMidpoint, Color.red);
    //    }
    //}


    public override NodeState Evaluate()
    {
        RaycastHit hit;
        float rayRadius = Mathf.Max(shipHeight, shipWidth) + 1f;
        if(Physics.SphereCast(_shipTransform.position, rayRadius, _shipTransform.forward, out hit, _rayLength))
        {
            Debug.Log("hit");
            parent.SetData("obstacle", hit);
            state = NodeState.SUCCESS;
            return state;
        } else
        {
            _enemyControls.Stop();
            ClearData("obstacle");
            state = NodeState.FAILURE;
            return state;
        }
        // failure on middle ray hit, store hit object
        // find a non hitting side ray
        // move towards end of side ray

        //// Damageable layer
        //int layerMask = 1 << 10;

        //// Invert to check everything but damageable because we might want to shoot that?
        //layerMask = ~layerMask;

        //hitTimer += Time.deltaTime;
        //if(hitTimer < hitCooldown)
        //{
        //    state = NodeState.SUCCESS;
        //    return state;
        //}

        //List<Vector3> rayOrigins = CreateRayOrigins();
        
        //foreach (Vector3 rayOrigin in rayOrigins)
        //{
        //    RaycastHit hit;
        //    if (Physics.Raycast(rayOrigin, _shipTransform.TransformDirection(Vector3.forward), out hit, _rayLength))
        //    {
        //        Debug.DrawRay(rayOrigin, _shipTransform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
        //        parent.SetData("avoidHit", hit);
        //        hitTimer = 0f;
        //        state = NodeState.SUCCESS;
        //        return state;
        //    }
        //    else
        //    {
        //        Debug.DrawRay(rayOrigin, _shipTransform.TransformDirection(Vector3.forward) * 1000, Color.white);
        //    }

        //    _enemyControls.Stop();
        //    ClearData("avoidPosition");
        //}

        //state = NodeState.FAILURE;
        //return state;
    }

    List<Vector3> CreateRayOrigins()
    {
        float xAdjust = 1.0f;
        float yAdjust = 1.0f;
        List<Vector3> rayOrigins = new List<Vector3>();
        // upper right
        rayOrigins.Add(_shipTransform.position + (_shipTransform.right * shipWidth * xAdjust) + (_shipTransform.up * shipHeight * yAdjust));
        // lower right
        rayOrigins.Add(_shipTransform.position + (_shipTransform.right * shipWidth * xAdjust) + (-_shipTransform.up * shipHeight * yAdjust));
        // upper left 
        rayOrigins.Add(_shipTransform.position - (_shipTransform.right * shipWidth * xAdjust) + (_shipTransform.up * shipHeight * yAdjust));
        // lower left
        rayOrigins.Add(_shipTransform.position - (_shipTransform.right * shipWidth * xAdjust) + (-_shipTransform.up * shipHeight * yAdjust));
        return rayOrigins;
    }
}