using FMOD.Studio;
using FMODUnity;
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

    public float speedMultiplier = 1f;

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

    // Audio
    private EventInstance engineSFX;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mass = rb.mass;
        //rb.inertiaTensor = rb.inertiaTensor;
        engineSFX = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.ShipEngine);
        RuntimeManager.AttachInstanceToGameObject(engineSFX, transform, rb);
    }

    void FixedUpdate()
    {
        HandleMovement();
        UpdateSound();
    }

    void HandleMovement()
    {
        // Roll
        rb.AddRelativeTorque(mass * roll1D * rollTorque * Vector3.back);
        // Pitch
        rb.AddRelativeTorque(mass * Mathf.Clamp(-pitchYaw.y, -1f, 1f) * pitchTorque * Vector3.right);
        // Yaw
        rb.AddRelativeTorque(mass * Mathf.Clamp(pitchYaw.x, -1f, 1f) * yawTorque * Vector3.up);

        // Thrust
        if(thrust1D > 0.1f || thrust1D < -0.1f)
        {
            float currentThrust = thrust * speedMultiplier;

            // disable boost when going backwards
            if (thrust1D < -0.1f)
            {
                currentThrust = thrust;
            }

            rb.AddRelativeForce(currentThrust * mass * thrust1D * Vector3.forward);
            glide = thrust;
        } else
        {
            rb.AddRelativeForce(glide * mass * Vector3.forward);
            glide *= thrustGlideReduction;
        }

        // Up/Down
        if(upDown1D > 0.1f || upDown1D < -0.1f)
        {
            rb.AddRelativeForce(mass * upDown1D * upThrust * Vector3.up);
            verticalGlide = upDown1D * upThrust;
        } else
        {
            rb.AddRelativeForce(mass * verticalGlide * Vector3.up);
            verticalGlide *= upDownGlideReduction;
        }

        // Strafing

        if (strafe1D > 0.1f || strafe1D < -0.1f)
        {
            rb.AddRelativeForce(mass * strafe1D * strafeThrust * Vector3.right);
            horizontalGlide = strafe1D * strafeThrust;
        }
        else
        {
            rb.AddRelativeForce(horizontalGlide * mass * Vector3.right);
            horizontalGlide *= leftRightGlideReduction;
        }
    }

    public void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public void UnFreeze()
    {
        rb.constraints = RigidbodyConstraints.None;
    }

    void UpdateSound()
    {
        // play engine sound when 'forward' is held down.. or something else?
        if (thrust1D > 0.1f)
        {
            Debug.Log("play engine?");
            PLAYBACK_STATE playbackState;
            engineSFX.getPlaybackState(out playbackState);
            RuntimeManager.AttachInstanceToGameObject(engineSFX, transform, rb);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                engineSFX.start();
            }
        } else {
            Debug.Log("stop engine?");
            engineSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
