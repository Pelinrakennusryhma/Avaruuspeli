using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable
{
    public bool TryingToActivate { get; set; }
    public bool Active { get; set; }
    public float Duration { get; set; }
    public float DurationTimer { get; set; }
    public float Cooldown { get; set; }
    public float CooldownTimer { get; set; }
    public ActorSpaceship Actor { get; set; }
    public ShipUtility Data { get; set; }

    public abstract void Init(ShipUtility data, ActorSpaceship actor);
}
