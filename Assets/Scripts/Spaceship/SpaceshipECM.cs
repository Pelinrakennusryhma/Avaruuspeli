using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipECM : MonoBehaviour, IUseable
{
    public bool Active { get; set; }
    public float Duration { get; set; }
    public float Cooldown { get; set; }
    private float timer;


    public void Init(float duration, float cooldown)
    {
        Duration = duration;
        Cooldown = cooldown;
        timer = cooldown;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (Active)
        {
            if(timer > Cooldown)
            {
                Debug.Log("use ECM");
                timer = 0f;
            }
        }
    }

}
