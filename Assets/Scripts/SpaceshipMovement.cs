using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SpaceshipMovement : MonoBehaviour
{
    [SerializeField]
    private float yawTorque = 500f;
    [SerializeField]
    private float pitchTorque = 1000f;
    [SerializeField]
    private float rollTorque = 1000f;
    [SerializeField]
    private float thrust = 100f;
    [SerializeField]
    private float upThrust = 50f;
    [SerializeField]
    private float strafeThrust = 50f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float thrustGlideReduction = 0.999f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float upDownGlideReduction = 0.111f;
    [SerializeField, Range(0.001f, 0.999f)]
    private float leftRightGlideReduction = 0.111f;

    public float boostMultiplier = 1f;

    private float glide = 0f;
    private float verticalGlide = 0f;
    private float horizontalGlide = 0f;

    Rigidbody rb;
    float mass;

    // Input Values
    public float thrust1D;
    public float upDown1D;
    public float strafe1D;
    public float roll1D;
    public Vector2 pitchYaw;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Roll
        rb.AddRelativeTorque(mass * Vector3.back * roll1D * rollTorque * Time.deltaTime);
        // Pitch
        rb.AddRelativeTorque(mass * Vector3.right * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Time.fixedDeltaTime);
        // Yaw
        rb.AddRelativeTorque(mass * Vector3.up * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Time.fixedDeltaTime);

        // Thrust
        if(thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currentThrust = thrust * boostMultiplier;

            // disable boost when going backwards
            if (thrust1D < -0.1f)
            {
                currentThrust = thrust;
            }

            rb.AddRelativeForce(mass * Vector3.forward * thrust1D * currentThrust * Time.fixedDeltaTime);
            glide = thrust;
        } else
        {
            rb.AddRelativeForce(mass * Vector3.forward * glide * Time.fixedDeltaTime);
            glide *= thrustGlideReduction;
        }

        // Up/Down
        if(upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(mass * Vector3.up * upDown1D * upThrust * Time.fixedDeltaTime);
            verticalGlide = upDown1D * upThrust;
        } else
        {
            rb.AddRelativeForce(mass * Vector3.up * verticalGlide * Time.fixedDeltaTime);
            verticalGlide *= upDownGlideReduction;
        }

        // Strafing

        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(mass * Vector3.right * strafe1D * strafeThrust * Time.fixedDeltaTime);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddRelativeForce(mass * Vector3.right * horizontalGlide * Time.fixedDeltaTime);
            horizontalGlide *= leftRightGlideReduction;
        }
    }
}
