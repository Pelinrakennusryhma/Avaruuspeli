using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonPlayerCharacter : MonoBehaviour
{
    public static FirstPersonPlayerCharacter Instance;

    public CapsuleCollider StandingCapsule;

    private void Awake()
    {
        Instance = this;
    }
}
