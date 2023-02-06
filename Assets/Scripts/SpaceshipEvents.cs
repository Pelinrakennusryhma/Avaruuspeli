using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpaceshipEvents : MonoBehaviour
{
    public UnityEvent EventSpaceshipHealthChanged;
    public UnityEvent EventSpaceshipDied;
    public UnityEvent<float> EventSpaceshipCollided;

    public void CallEventSpaceshipHealthChanged()
    {
        EventSpaceshipHealthChanged.Invoke();
    }

    public void CallEventSpaceshipDied()
    {
        EventSpaceshipDied.Invoke();
    }

    public void CallEventSpaceshipCollided(float magnitude)
    {
        EventSpaceshipCollided.Invoke(magnitude);
    }
}
