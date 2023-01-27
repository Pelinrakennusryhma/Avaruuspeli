using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SpaceshipHealth : UITrackable
{
    [SerializeField]
    int maxHealth = 100;
    [SerializeField]
    int currentHealth = 100;

    public UnityEvent shipHealthChangedEvent;

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
        int cachedCurrentHealth = currentHealth;

        if(newValue <= 0)
        {
            currentHealth = 0;
            Debug.Log("Dead");
        } else if(newValue > maxHealth) {
            currentHealth = maxHealth;
        } else
        {
            currentHealth = newValue;
        }

        if(currentHealth != cachedCurrentHealth)
        {
            shipHealthChangedEvent.Invoke();
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Alpha0))
        {
            IncreaseHealth(5);
        }

        if (Input.GetKeyUp(KeyCode.Alpha9))
        {
            DecreaseHealth(5);
        }

        if (Input.GetKeyUp(KeyCode.Alpha7))
        {
            IncreaseHealth(100);
        }

        if (Input.GetKeyUp(KeyCode.Alpha8))
        {
            DecreaseHealth(100);
        }
    }
}
