using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITrackable : MonoBehaviour
{
    public abstract float MaxValue
    {
        get;
    }

    public abstract float CurrentValue
    {
        get;
    }
}
