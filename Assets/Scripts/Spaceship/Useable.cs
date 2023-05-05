using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Useable : MonoBehaviour
{
    public bool TryingToActivate { get; set; }
    public bool Active { get; set; }
    public float Duration { get; set; }
    public float DurationTimer { get; set; }
    public float Cooldown { get; set; }
    public float CooldownTimer { get; set; }
    public ActorSpaceship Actor { get; set; }
    public ShipUtility Data { get; set; }

    public virtual void Init(ShipUtility data, ActorSpaceship actor)
    {
        Duration = data.effectDuration;
        Cooldown = data.cooldown;
        CooldownTimer = data.cooldown;
        Actor = actor;
        Data = data;
    }

    protected abstract IEnumerator Activate();

    void Update()
    {
        if (Active)
        {
            DurationTimer -= Time.deltaTime;
            if (DurationTimer <= 0f)
            {
                Active = false;
            }
        }
        else
        {
            CooldownTimer += Time.deltaTime;
            if (TryingToActivate)
            {
                if (CooldownTimer > Cooldown)
                {
                    StartCoroutine(Activate());
                    Active = true;
                    DurationTimer = Duration;
                    CooldownTimer = 0f;
                }
            }
        }
    }
}
