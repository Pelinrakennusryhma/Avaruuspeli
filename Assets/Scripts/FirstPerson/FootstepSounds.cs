using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepSounds : MonoBehaviour
{
    [Tooltip("Value under which character is considered stopped and no footstep sounds should play.")]
    [SerializeField] float minSoundTriggerVelocity = 0.05f;
    [SerializeField] AnimationCurve intervalCurve;
    float interval = Mathf.Infinity;
    float timer = 0f;
    Rigidbody rb;
    IFPController firstPersonController;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        firstPersonController = GetComponent<IFPController>();
    }

    void Update()
    {
        CalculateInterval();
        HandleInterval();
    }

    void CalculateInterval()
    {
        float vel = rb.velocity.sqrMagnitude;

        if (vel > minSoundTriggerVelocity)
        {
            interval = intervalCurve.Evaluate(vel);
        } else
        {
            interval = Mathf.Infinity;
        }

    }

    void HandleInterval()
    {
        if(timer >= interval)
        {
            timer = 0f;
            if (firstPersonController.IsGrounded)
            {
                AudioManager.Instance.PlayOneShot(FMODEvents.Instance.Footsteps, transform.position);
            }
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
}
