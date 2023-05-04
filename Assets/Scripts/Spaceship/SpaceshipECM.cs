using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipECM : MonoBehaviour, IUseable
{
    public bool TryingToActivate { get; set; }
    public bool Active { get; set; }
    public float Duration { get; set; }
    public float UseTimer { get; private set; }
    public float Cooldown { get; set; }
    public ActorSpaceship Actor { get; set; }

    public float CooldownTimer { get; private set; }


    public void Init(float duration, float cooldown, ActorSpaceship actor)
    {
        Duration = duration;
        Cooldown = cooldown;
        CooldownTimer = cooldown;
        Actor = actor;
    }

    void Update()
    {
        if (Active)
        {
            UseTimer -= Time.deltaTime;
            if(UseTimer <= 0f)
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
                    UseTimer = Duration;
                    CooldownTimer = 0f;
                }
            }
        }
    }

    IEnumerator Activate()
    {
        Debug.Log("activating ecm");
        Actor.Protected = true;
        Actor.ClearLockedMissiles();
        yield return new WaitForSeconds(Duration);
        Actor.Protected = false;
    }

}
