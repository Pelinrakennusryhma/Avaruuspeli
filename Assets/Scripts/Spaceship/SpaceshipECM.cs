using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipECM : MonoBehaviour, IUseable
{
    public bool TryingToActivate { get; set; }
    public bool Active { get; set; }
    public float Duration { get; set; }
    private float useTimer;
    public float Cooldown { get; set; }
    private float cooldownTimer;


    public void Init(float duration, float cooldown)
    {
        Duration = duration;
        Cooldown = cooldown;
        cooldownTimer = cooldown;
    }

    void Update()
    {
        if (Active)
        {
            useTimer -= Time.deltaTime;
            if(useTimer <= 0f)
            {
                Active = false;
            }
        } 
        else
        {
            cooldownTimer += Time.deltaTime;
            if (TryingToActivate)
            {
                if (cooldownTimer > Cooldown)
                {
                    Active = true;
                    useTimer = Duration;
                    cooldownTimer = 0f;
                }
            }
        }
    }

}
