using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : ActorSpaceship
{
    Transform shipTransform;
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
    public bool MoveTowards(Vector3 destination)
    {
        if (Vector3.Distance(shipTransform.position, destination) < 0.1f)
        {
            // stop all movement
            return true;
        }

        Vector3 destinationRelative = shipTransform.InverseTransformPoint(destination);
        Debug.Log("relative: " + destinationRelative);
        if (Vector3.Distance(shipTransform.position, destination) > 0.1f)
        {
            OnThrust(1f);
            return false;
        } else
        {
            OnThrust(0f);
            return true;
        }

    }

    float Normalize1D(float value1D)
    {
        return Mathf.Clamp(value1D, -1f, 1f);
    }

    void OnThrust(float thrust1D)
    {
        Debug.Log("input: " + thrust1D + "normalize: " + Normalize1D(thrust1D));
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
