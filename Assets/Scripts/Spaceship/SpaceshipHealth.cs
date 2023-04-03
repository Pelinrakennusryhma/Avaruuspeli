using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipHealth : UITrackable
{
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    int currentHealth = 100;

    SpaceshipEvents spaceshipEvents;
    public override float MaxValue
    {
        get
        {
            return maxHealth;
        }
    }

    public override float CurrentValue
    {
        get
        {
            return currentHealth;
        }
    }

    void Awake()
    {
        spaceshipEvents = GetComponent<SpaceshipEvents>();
    }

    public void DecreaseHealth(int value)
    {
        SetHealth(currentHealth - value);
    }

    public void IncreaseHealth(int value)
    {
        SetHealth(currentHealth + value);
    }

    void SetHealth(int newValue)
    {
        if(currentHealth != newValue)
        {
            spaceshipEvents.CallEventSpaceshipHealthChanged();
        }

        if(newValue <= 0)
        {
            currentHealth = 0;
            spaceshipEvents.CallEventSpaceshipDied();
        } else if(newValue > maxHealth) {
            currentHealth = maxHealth;
        } else
        {
            currentHealth = newValue;
        }

    }

    //private void Update()
    //{
    //    if (Input.GetKeyUp(KeyCode.Alpha0))
    //    {
    //        IncreaseHealth(5);
    //    }

    //    if (Input.GetKeyUp(KeyCode.Alpha9))
    //    {
    //        DecreaseHealth(5);
    //    }

    //    if (Input.GetKeyUp(KeyCode.Alpha7))
    //    {
    //        IncreaseHealth(100);
    //    }

    //    if (Input.GetKeyUp(KeyCode.Alpha8))
    //    {
    //        DecreaseHealth(100);
    //    }
    //}
}
