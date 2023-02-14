using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;
public class CheckForObstacle : Node
{
    Transform _shipTransform;
    EnemyControls _enemyControls;
    Rigidbody _rb;
    float _minRayLength = 50f;
    float _maxRayLength = 150f;
    float _rayLength = 50f;

    float shipWidth;
    float shipHeight;

    bool hasHit = false;
    float hitCooldown = 0.2f;
    float hitTimer;

    float rayRadius = 4f;

    public CheckForObstacle(EnemyControls enemyControls, Transform shipTransform)
    {
        _enemyControls = enemyControls;
        _shipTransform = shipTransform;
        _rb = _shipTransform.GetComponent<Rigidbody>();

        MeshCollider shipCollider = shipTransform.GetComponent<MeshCollider>();
        shipWidth = shipCollider.bounds.extents.x;
        shipHeight = shipCollider.bounds.extents.y;

        hitTimer = hitCooldown;
        rayRadius = Mathf.Max(shipHeight, shipWidth) + 4f;
        Debug.Log("rayRadius: " + rayRadius);
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
        hitTimer += Time.deltaTime;
        if (hasHit && hitTimer < hitCooldown)
        {
            state = NodeState.SUCCESS;
            return state;
        }

        float rayLengthRatio = 10f / _rb.velocity.magnitude;
        _rayLength = Mathf.Lerp(_maxRayLength, _minRayLength, rayLengthRatio);
        //Debug.Log(_rayLength);

        RaycastHit hit;

        if(Physics.SphereCast(_shipTransform.position, rayRadius, _shipTransform.forward, out hit, _rayLength))
        {
            Debug.Log("hit");
            parent.SetData("obstacle", hit);
            hitTimer = 0f;
            hasHit = true;
            state = NodeState.SUCCESS;
            return state;
        } else
        {
            _enemyControls.Stop();
            ClearData("obstacle");
            hasHit = false;
            state = NodeState.FAILURE;
            return state;
        }
    }
}