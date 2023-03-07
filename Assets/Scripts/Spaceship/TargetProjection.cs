using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjection : MonoBehaviour
{
    [SerializeField]
    GameObject targetPrefab;
    GameObject targetObject;
    Rigidbody rb;


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if(targetPrefab != null)
        {
            targetObject = Instantiate(targetPrefab, transform);
        }
    }

    public Vector3 GetPosition(float projectileSpeed, Vector3 shooterPosition)
    {
        Vector3 predictedPos = PredictedPosition(transform.position, shooterPosition, rb.velocity, projectileSpeed);
        Debug.DrawLine(shooterPosition, predictedPos, Color.yellow, 0.1f);
        Debug.DrawLine(transform.position, predictedPos, Color.cyan, 0.1f);

        Debug.Log("?!?!");
        if(targetObject != null)
        {
            Debug.Log("???" + predictedPos);
            targetObject.transform.position = predictedPos;
        }

        return predictedPos;
    }

    private Vector3 PredictedPosition(Vector3 targetPosition, Vector3 shooterPosition, Vector3 targetVelocity, float projectileSpeed)
    {
        Vector3 displacement = targetPosition - shooterPosition;
        float targetMoveAngle = Vector3.Angle(-displacement, targetVelocity) * Mathf.Deg2Rad;
        //if the target is stopping or if it is impossible for the projectile to catch up with the target (Sine Formula)
        if (targetVelocity.magnitude == 0 || targetVelocity.magnitude > projectileSpeed && Mathf.Sin(targetMoveAngle) / projectileSpeed > Mathf.Cos(targetMoveAngle) / targetVelocity.magnitude)
        {
            Debug.Log("Position prediction is not feasible.");
            return targetPosition;
        }
        //also Sine Formula
        float shootAngle = Mathf.Asin(Mathf.Sin(targetMoveAngle) * targetVelocity.magnitude / projectileSpeed);
        return targetPosition + targetVelocity * displacement.magnitude / Mathf.Sin(Mathf.PI - targetMoveAngle - shootAngle) * Mathf.Sin(shootAngle) / targetVelocity.magnitude;
    }
}
