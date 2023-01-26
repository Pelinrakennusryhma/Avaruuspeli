using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UITrackable : MonoBehaviour
{
    public abstract int MaxValue
    {
        get;
    }

    public abstract int CurrentValue
    {
        get;
    }
}
