using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : ActorSpaceship
{
    TargetProjectionIcon targetProjectionIcon;
    Transform shipTransform;
    Rigidbody rb;
    float breakThreshold = 8f;
    float rotationDotThreshold = 0.999f;
    float minRollThreshold = 0.05f;
    float maxRollThreshold = 0.5f;
    [SerializeField]
    GameObject missileLockIndicatorPrefab;
    GameObject missileLockIndicator;

    protected override void OnEnable()
    {
        base.OnEnable();
        shipTransform = ship.transform;
        rb = shipTransform.GetComponent<Rigidbody>();
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Destroy(gameObject);
    }

    public override void LockMissile(ActorSpaceship shooter, Missile missile)
    {
        base.LockMissile(shooter, missile);
        missileLockIndicator = Instantiate(missileLockIndicatorPrefab, shipTransform);
    }

    public override void UnlockMissile(Missile missile)
    {
        base.UnlockMissile(missile);
        Destroy(missileLockIndicator);
    }

    public override void FocusShip(ActorSpaceship shooter)
    {
        base.FocusShip(shooter);
        if (targetProjectionIcon == null)
        {
            targetProjectionIcon = ship.GetComponentInChildren<TargetProjectionIcon>();
        }
        targetProjectionIcon.pulsing = true;
    }

    public override void UnfocusShip(ActorSpaceship shooter)
    {
        base.UnfocusShip(shooter);
        if (targetProjectionIcon == null)
        {
            targetProjectionIcon = ship.GetComponentInChildren<TargetProjectionIcon>();
        }
        targetProjectionIcon.pulsing = false;
    }

    // returns true when destination is reached
    public bool MoveToPosition(Vector3 destination, float minDistance, bool useBoost = true)
    {

        Vector3 destinationRelative = shipTransform.InverseTransformPoint(destination);
        Vector3 destinationNormal = destinationRelative.normalized;
        Vector2 rotationNormal = (Vector2)destinationRelative.normalized;
        float dotProduct = Vector3.Dot(shipTransform.forward, (destination - shipTransform.position).normalized);

        // duct tape fix in case destination is exactly behind us (0, 0, -1), very unlikely to happen in normal use
        if (destinationNormal.z < -rotationDotThreshold)
        {
            destinationNormal.x = Random.Range(-1f, 1f);
        }

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
                turnSpeedFactor = Mathf.Lerp(0.05f, 1f, 1 - dotProduct);
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
            OnThrust(0.5f);
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

    public void Avoid(Collider obstacle, float brakeDistanceSpeedThreshold = 0.8f)
    {

        Vector3 closestHit = obstacle.ClosestPoint(shipTransform.position);

        float distance = Vector3.Distance(shipTransform.position, closestHit);
        float speed = rb.velocity.magnitude;
        float ratio = distance / speed;

        if(ratio < brakeDistanceSpeedThreshold)
        {
            OnThrust(-1f);
            OnBoost(false);
            //Debug.Log("braking: " + ratio);
        } else
        {
            OnThrust(1f);
        }

        Vector3 relativeNormal = shipTransform.InverseTransformPoint(closestHit).normalized;
        
        float moveX = -relativeNormal.x;
        float moveY = -relativeNormal.y;

        OnPitchYaw(new Vector2(moveX, moveY));

        // multiply the movement so it actually has an effect
        OnStrafe(Mathf.Clamp(moveX * 1000, -1f, 1f));
        OnUpDown(Mathf.Clamp(moveY * 1000, -1f, 1f));

        if (relativeNormal.y < -minRollThreshold)
        {
            OnRoll(moveX);
        } else if(relativeNormal.y > minRollThreshold)
        {
            OnRoll(-moveX);
        }

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

    public void OnSecondaryShoot(bool shooting)
    {
        spaceshipMissile.shooting = shooting;
    }
}
