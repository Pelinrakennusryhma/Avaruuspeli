using BehaviorTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskEvadeAttacker : Node
{
    EnemyControls _enemyControls;
    Transform _shipTransform;
    float fleeDistance = 2000f;
    float fleeDuration = 3f;
    float fleeTimer = 0f;
    Vector3 fleePosition = Vector3.zero;
    float _shieldChance;
    Useable usedUseable;
    public TaskEvadeAttacker(EnemyControls enemyControls, Transform shipTransform, float shieldChance)
    {
        _enemyControls = enemyControls;
        _shipTransform = shipTransform;
        _shieldChance = shieldChance;
    }

    public override NodeState Evaluate()
    {
        object data = GetData("evading");
        if(data == null)
        {
            return NodeState.FAILURE;
        }

        bool evading = (bool)data;
        if(evading == false)
        {
            return NodeState.FAILURE;
        }

        // duration over, stop evasion behavior
        if (fleeTimer > fleeDuration)
        {
            parent.SetData("evading", false);
            fleeTimer = 0f;
            fleePosition = Vector3.zero;
            _enemyControls.CancelUseable(usedUseable);
            return NodeState.FAILURE;
        }

        // start timer and evade;
        if (fleePosition == Vector3.zero)
        {
            GetRandomPosition();
            Debug.Log("new fleePosition: " + fleePosition);
            if(Random.value > _shieldChance)
            {
                usedUseable = _enemyControls.UseShield();
            }
        }
        Debug.Log("evading: " + fleePosition);
        fleeTimer += Time.deltaTime;
        _enemyControls.MoveToPosition(fleePosition, 100f, true);
        return NodeState.SUCCESS;
    }

    Vector3 GetPointOnUnitSphereCap(Quaternion targetDirection, float angle)
    {
        float angleInRad = Random.Range(0.0f, angle) * Mathf.Deg2Rad;
        Vector3 PointOnCircle = (Random.insideUnitCircle.normalized) * Mathf.Sin(angleInRad);
        Vector3 v = new Vector3(PointOnCircle.x, PointOnCircle.y, Mathf.Cos(angleInRad));
        return targetDirection * v;
    }
    Vector3 GetPointOnUnitSphereCap(Vector3 targetDirection, float angle)
    {
        return GetPointOnUnitSphereCap(Quaternion.LookRotation(targetDirection), angle);
    }

    void GetRandomPosition()
    {
        fleePosition = GetPointOnUnitSphereCap(_shipTransform.forward, 30f) * fleeDistance;
        //fleePosition = Random.insideUnitSphere.normalized * fleeDistance + _shipTransform.position;
    }
}
