using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : ActorSpaceship
{
    Transform shipTransform;
    float breakThreshold = 8f;
    float rotationDotThreshold = 0.999f;
    float angleThreshold = 3f;
    void Start()
    {
        shipTransform = transform.GetChild(0).GetComponent<Transform>();
    }

    void Update()
    {
        
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }

    // returns true when destination is reached
    public bool MoveTowards(Vector3 destination, float minDistance)
    {

        Vector3 destinationRelative = shipTransform.InverseTransformPoint(destination);
        Vector3 destinationNormal = destinationRelative.normalized;
        Vector2 rotationNormal = (Vector2)destinationRelative.normalized;
        float dotProduct = Vector3.Dot(shipTransform.forward, (destination - shipTransform.position).normalized);
        Vector3 targetDir = destination - shipTransform.position;
        float angle = Vector3.Angle(targetDir, shipTransform.forward);

        // rolling

        Debug.Log(destinationNormal);

        // rotation
        float turnSpeedFactor = 1f;

        if (dotProduct < rotationDotThreshold)
        {
            // when targetting precisely enough, gradually slow down the rotation
            if(dotProduct < 0.9)
            {
                turnSpeedFactor = Mathf.Lerp(0.1f, 1f, 1 - dotProduct);
            }

            OnPitchYaw(rotationNormal * turnSpeedFactor);
        } 
        else
        {
            OnPitchYaw(Vector2.zero);
        } 


        // movement
        if (destinationRelative.z > minDistance)
        {
            OnThrust(1f);
        } else if(destinationRelative.z < breakThreshold)
        {
            OnThrust(-1f);
        } else
        {
            OnThrust(0f);
        }

        return false;

    }

    public void Stop()
    {
        OnPitchYaw(Vector2.zero);
        OnThrust(0f);
        OnStrafe(0f);
        OnUpDown(0f);
        OnRoll(0f);
        OnBoost(false);
    }

    float Normalize1D(float value1D)
    {
        return Mathf.Clamp(value1D, -1f, 1f);
    }

    void OnThrust(float thrust1D)
    {
        spaceshipMovement.thrust1D = Normalize1D(thrust1D);
    }

    void OnStrafe(float strafe1D)
    {
        spaceshipMovement.strafe1D = Normalize1D(strafe1D);
    }

    void OnUpDown(float upDown1D)
    {
        spaceshipMovement.upDown1D = Normalize1D(upDown1D);
    }

    void OnRoll(float roll1D)
    {
        spaceshipMovement.roll1D = Normalize1D(roll1D);
    }

    void OnPitchYaw(Vector2 delta)
    {
        spaceshipMovement.pitchYaw = delta.normalized;
    }

    void OnBoost(bool boosting)
    {
        spaceshipBoost.boosting = boosting;
    }

    public void OnShoot(bool shooting)
    {
        spaceshipShoot.shooting = shooting;
    }
}
