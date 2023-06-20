using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungerTracker : MonoBehaviour
{
    public float CurrentFullness = 100;
    public float MaxFullness = 100;

    private float SandwichConsumptionRatePerMinute = 33.0f;
    //private float SandwichConsumptionRatePerMinute = 100.0f;

    public bool IsOnFirstPersonScene;

    private const string hungerTextShip = "You we're too hungry to continue expedition. \nReturned back to ship for nourishment.";
    private const string hungerTextMothership = "You we're too hungry to continue expedition. \nReturned back to mothership for nourishment.";

    private bool WaitingForADelayedMessagePrompt;

    public void Awake()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnEnterWorldMap -= OnEnterWorldMap;
            GameManager.Instance.OnEnterWorldMap += OnEnterWorldMap;
        }

        WaitingForADelayedMessagePrompt = false;
    }

    public void OnEnterFirstPersonScene()
    {
        IsOnFirstPersonScene = true;
        CurrentFullness = 100;
        //Debug.Log("We entered first person scene");
    }

    public void OnLeaveFirstPersonScene()
    {
        IsOnFirstPersonScene = false;
        //Debug.Log("We exited first person scene");
    }

    public bool CheckIfCanEatASandwich()
    {
        bool canEatASandwich = false;

        if (CurrentFullness + 25.0f < 124.0f)
        {
            canEatASandwich = true;
            Debug.Log("We have room. We can eat a sandwich");
        }

        else
        {
            canEatASandwich = false;
            Debug.Log("We are too full to eat a sandwich");
        }

        return canEatASandwich;
    }

    public void OnEatSandwich()
    {
        GameManager.Instance.InventoryController.Inventory.RemoveItem(20, 1);

        CurrentFullness += 25.0f;

        CurrentFullness = Mathf.Clamp(CurrentFullness, 0, 100.0f);

        Debug.Log("We ate a sandwich");
    }

    public void Update()
    {
        if (IsOnFirstPersonScene) 
        {
            CurrentFullness -= Time.deltaTime * SandwichConsumptionRatePerMinute / 60.0f;

            if (CurrentFullness <= 0)
            {
                OnDieOfHunger();
                Debug.LogError("We pretty much died of hunger");
            }

            CurrentFullness = Mathf.Clamp(CurrentFullness, 0, MaxFullness);
        }

    }

    public void OnDieOfHunger()
    {
        // return back to mothership
        // Display a message about being too hungry to continue

        // We are on asteroid surface
        if (GameEvents.Instance != null) 
        {
            GameEvents.Instance.CallEventPlayerWasTooHungyToContinue();
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(hungerTextShip);
        }

        else
        {
            WaitingForADelayedMessagePrompt = true;
            GameManager.Instance.GoBackToWorldMap();
        }
    }

    public void OnEnterWorldMap()
    {
        if (WaitingForADelayedMessagePrompt)
        {
            GameManager.Instance.WorldMapMessagePrompt.DisplayMessage(hungerTextMothership, 6.0f);
        }

        WaitingForADelayedMessagePrompt = false;
    }
}
