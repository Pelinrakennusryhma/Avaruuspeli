using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipHealth : UITrackable
{
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    int currentHealth = 100;

    public override int MaxValue
    {
        get
        {
            return maxHealth;
        }
    }

    public override int CurrentValue
    {
        get
        {
            return currentHealth;
        }
    }

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int value)
    {
        SetHealth(currentHealth - value);
    }

    void SetHealth(int newValue)
    {
        if(newValue <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead");
        } else if(newValue > maxHealth) {
            currentHealth = newValue;
        } else
        {
            currentHealth = newValue;
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            TakeDamage(5);
        }
    }
}
