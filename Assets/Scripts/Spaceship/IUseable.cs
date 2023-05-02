using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseable
{
    public bool Active { get; set; }
    public float Duration { get; set; }
    public float Cooldown { get; set; }

    public abstract void Init(float duration, float cooldown);
}
