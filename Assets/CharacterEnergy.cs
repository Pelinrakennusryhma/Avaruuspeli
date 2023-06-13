using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEnergy : UITrackable
{
    public override float MaxValue
    {
        get 
        { 
            return maxEnergy; 
        }
    }

    public override float CurrentValue
    {
        get 
        { 
            return currentEnergy; 
        }
    }

    private float maxEnergy = 100;
    private float currentEnergy;

    // Start is called before the first frame update
    private void Awake()
    {
        maxEnergy = GameManager.Instance.HungerTracker.MaxFullness;
        //Debug.Log("We have energy HUD activated");
    }

    // Update is called once per frame
    void Update()
    {
        currentEnergy = Mathf.RoundToInt(GameManager.Instance.HungerTracker.CurrentFullness);
        //currentEnergy = Random.Range(0.0f, maxEnergy);
        //Debug.Log("Current energy is " + currentEnergy);
    }
}
