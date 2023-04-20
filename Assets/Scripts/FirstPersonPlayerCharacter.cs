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

        CheckIfWeAreOnBreathableScene();
    }

    public void CheckIfWeAreOnBreathableScene()
    {
        if (GameManager.Instance.CurrentSceneType == GameManager.TypeOfScene.AsteroidField)
        {
            GameManager.Instance.LifeSupportSystem.OnEnterUnbreathablePlace();
        }
    }
}
