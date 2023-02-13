using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : ActorSpaceship
{
    Transform shipTransform;
    float breakThreshold = 8f;
    float rotationDotThreshold = 0.999f;
    float minRollThreshold = 0.05f;
    float maxRollThreshold = 0.5f;

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
    public bool MoveToPosition(Vector3 destination, float minDistance, bool useBoost = true)
    {

        Vector3 destinationRelative = shipTransform.InverseTransformPoint(destination);
        Vector3 destinationNormal = destinationRelative.normalized;
        Vector2 rotationNormal = (Vector2)destinationRelative.normalized;
        float dotProduct = Vector3.Dot(shipTransform.forward, (destination - shipTransform.position).normalized);
        Vector3 targetDir = destination - shipTransform.position;

        // rolling

        // roll more responsively the closer we are to target
        float rollDistanceThreshold = minDistance * 2;
        float rollThreshold = destinationRelative.z > rollDistanceThreshold ? maxRollThreshold : minRollThreshold;
        if (destinationNormal.x > rollThreshold)
        {
            OnRoll(1f);
        }
        else if (destinationNormal.x < -rollThreshold)
        {
            OnRoll(-1f);
        }
        else
        {
            OnRoll(0f);
        }



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

        //boost
        if (useBoost)
        {
            float boostThreshold = minDistance * 2;
            if (destinationRelative.z > boostThreshold)
            {
                OnBoost(true);
            }
            else
            {
                OnBoost(false);
            }
        }

        // destination reached
        if (Vector3.Distance(shipTransform.position, destination) < minDistance)
        {
            return true;
        } else
        {
            return false;
        }
    }

    public void Avoid(Collider obstacle)
    {
        Vector3 relativeNormal = shipTransform.InverseTransformPoint(obstacle.transform.position).normalized;
        
        //Debug.Log(relativeNormal);
        float moveX = -relativeNormal.x;
        float moveY = -relativeNormal.y;
        OnPitchYaw(new Vector2(moveX, moveY));

        //Vector3 closestHit = obstacle.ClosestPoint(shipTransform.position);
        //float distance = Vector3.Distance(closestHit, shipTransform.position);
        //Debug.Log("distance: " + distance);

        float rollThreshold = (minRollThreshold + maxRollThreshold) / 2;

        if (relativeNormal.y < -minRollThreshold)
        {
            OnRoll(moveX);
        } else if(relativeNormal.y > minRollThreshold)
        {
            OnRoll(-moveX);
        }

    }

    public void MoveSmoothly(Vector3 destination)
    {

        float smoothAmount = 0.01f;
        //float thrust1DSmoothed = Mathf.Lerp(destination.x, spaceshipMovement.thrust1D, smoothAmount);
        //float upDown1DSmoothed = Mathf.Lerp(destination.x, spaceshipMovement.thrust1D, smoothAmount);
        //float strafe1DSmoothed;
        //float roll1DSmoothed = Mathf.Lerp(destination.x, spaceshipMovement.thrust1D, smoothAmount);

        Vector2 pitchYawSmoothed = Vector2.Lerp(destination, spaceshipMovement.pitchYaw, smoothAmount);

        float rollDirection = 0f;
        if (destination.x < minRollThreshold)
        {
            rollDirection = 0.1f;
        }
        else if (destination.x > -minRollThreshold)
        {
            rollDirection = -0.1f;
        }

        float rollSmoothed = Mathf.Lerp(rollDirection, spaceshipMovement.roll1D, smoothAmount);

        //OnRoll(rollSmoothed);
        OnPitchYaw(pitchYawSmoothed);

    }

    public void MoveTowards(Vector3 delta, bool stopBoost = true)
    {
        OnBoost(!stopBoost);

        // rolling
        float rollTarget = 0f;
        if (delta.x < maxRollThreshold)
        {
            //rollTarget = Mathf.Lerp(spaceshipMovement.roll1D, 1f, 0.1f);
            rollTarget = 0.1f;
        }
        else if (delta.x > -maxRollThreshold)
        {
            //rollTarget = Mathf.Lerp(spaceshipMovement.roll1D, -1f, 0.1f);
            rollTarget = -0.1f;
        } else
        {
            rollTarget = 0f;
        }

        //if(delta.y < 0)
        //{
        //    rollTarget = -rollTarget;
        //}
        
        //OnRoll(rollTarget);

        // rotation
        OnPitchYaw(delta);


        //float distance = Vector3.Distance

        // movement
        //if (destination.z > breakThreshold * 2)
        //{
        //    OnThrust(1f);
        //}
        //else if (destination.z < breakThreshold)
        //{
        //    OnThrust(-1f);
        //}
        //else
        //{
        //    OnThrust(0f);
        //}

        ////boost
        //if (useBoost)
        //{
        //    float boostThreshold = minDistance * 2;
        //    if (destinationRelative.z > boostThreshold)
        //    {
        //        OnBoost(true);
        //    }
        //    else
        //    {
        //        OnBoost(false);
        //    }
        //}
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
