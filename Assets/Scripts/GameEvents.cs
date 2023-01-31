using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    private void Awake()
    {
        instance = this;
    }

    public UnityEvent EventPlayerDied;
    public void CallEventPlayerDied()
    {
        EventPlayerDied.Invoke();
    }
}
