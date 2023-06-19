using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LifeSupportSystem : MonoBehaviour
{

    public GameObject OxygenHUDParent;
    public TextMeshProUGUI OxygenHUDText;

    public int AmountOfOxygenBottles;
    public float AmountOfOxygenInLastBottle;

    public bool HasSpaceSuitEquipped = true;

    public bool IsOnBreathablePlanet; // What if we try to enter a planet that is not breathable without a spacesuit on?

    public bool DoOxygenThings;

    public FirstPersonPlayerControls PlayerControls;

    public bool JustRanOutOfOxygen;

    public bool TriedLeavingTheSceneAlready;

    private float OxygenConsumptionRatePerMinute = 0.1f;

    private bool TheEventHasBeenCalled;

    public const string ranOutOfOxygenText = "You ran out of oxygen";
    public const string removedSpaceSuitText = "You removed your spacesuit. Can't survive in space without it.";
    public const string OxygenString = "OXYGEN IN BOTTLES: ";
    public const string MgLString = " mg/L";

    public ItemSO EquippedSpaceSuit;

    public void Init()
    {


        OxygenHUDParent.SetActive(false);

        AmountOfOxygenBottles = 0;
        AmountOfOxygenInLastBottle = 0;

        if (GameManager.Instance.InventoryController.Equipment.equippedSpacesuit != null) 
        {
            HasSpaceSuitEquipped = true;
        }

        else
        {
            HasSpaceSuitEquipped = false;
        }

        JustRanOutOfOxygen = false;
        //Debug.Log("Initting life support system. Do this after initial savedata loading!");
    }

    public void Start()
    {
        int amountOfBottles = 0;
        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(13);

        if (inventoryItemScript != null)
        {
            amountOfBottles = GameManager.Instance.InventoryController.Inventory.GetItemScript(13).currentItemAmount;
        }

        UpdateOxygenBottles(amountOfBottles);

        if (GameManager.LaunchType == GameManager.TypeOfLaunch.LoadedGame)
        {
            float amountOfOxygen = GameManager.Instance.SaverLoader.LoadAmountOfOxygenInLastBottle();

            if (amountOfOxygen > 0) 
            {
                AmountOfOxygenInLastBottle = amountOfOxygen;
            }
        }
    }

    public void OnExitAsteroid(MineableAsteroidTrigger asteroid)
    {
        OnExitUnbreathablePlace();
    }

    public void UpdateOxygenBottles(int amount)
    {
        if (AmountOfOxygenBottles <= 0
            && amount > 0)
        {
            AmountOfOxygenInLastBottle = 1.0f;
        }

        AmountOfOxygenBottles = amount;

        if (AmountOfOxygenBottles <= 0)
        {
            AmountOfOxygenInLastBottle = 0.0f;
        }
    }

    public void OnSpaceSuitEquipped(ItemSO suit)
    {
        EquippedSpaceSuit = suit;
        HasSpaceSuitEquipped = true;
    }

    public void OnSpaceSuitUnequipped()
    {
        EquippedSpaceSuit = null;
        HasSpaceSuitEquipped = false;

        PlayerRemovedSpacesuit();
    }

    public void OnEnterUnbreathablePlace()
    {
        // Just in case removal if there is some duplicity thing going on
        GameEvents.Instance.EventPlayerLeftAsteroid.RemoveListener(OnExitAsteroid);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnExitAsteroid);

        OxygenHUDParent.SetActive(true);
        OxygenHUDParent.transform.SetParent(Camera.main.transform);
        TheEventHasBeenCalled = false;
        DoOxygenThings = true;
        TriedLeavingTheSceneAlready = false;

        int amountOfBottles = 0;
        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(13);

        if (inventoryItemScript != null)
        {
            amountOfBottles = GameManager.Instance.InventoryController.Inventory.GetItemScript(13).currentItemAmount;
        }



        UpdateOxygenBottles(amountOfBottles);
        //Debug.Log("Enter unbreathable place");
    }

    public void OnExitUnbreathablePlace()
    {
        OxygenHUDParent.transform.SetParent(GameManager.Instance.gameObject.transform);
        OxygenHUDParent.SetActive(false);
        DoOxygenThings = false;
        SaveRelevantData();
        GameManager.Instance.InventoryController.SaveInventory(); // Save invenotry too due, to reduced oxygen bottles
        //Debug.Log("On exit unbreathable place" + Time.time);
    }

    public void Update()
    {
        if (DoOxygenThings)
        {
            //Debug.Log("Doing oxygen things");
            ReduceOxygen();
        }

        else
        {
            //Debug.Log("Don't do oxygen things");
        }
    }

    public void ClearJustRanOutOfOxygenBool()
    {
        JustRanOutOfOxygen=false;
    }

    public void ReduceOxygen()
    {
        float deltaTime = Time.deltaTime;

        if (deltaTime != 0f)
        {
            float consumptionPerSecond = OxygenConsumptionRatePerMinute / 60.0f;

            AmountOfOxygenInLastBottle -= deltaTime * consumptionPerSecond;

            if (AmountOfOxygenInLastBottle <= 0
                && AmountOfOxygenBottles <= 1
                && !JustRanOutOfOxygen)
            {
                //Debug.Log("We ran out of oxygen. Die!!! or leave asteroid, if that's the better solution? " + Time.time);
                
                JustRanOutOfOxygen = true;
                TriedLeavingTheSceneAlready = true;
                DoOxygenThings = false;

                GameManager.Instance.InventoryController.Inventory.RemoveItem(13, 1);

                if (!TheEventHasBeenCalled) 
                {
                    //Debug.Log("A call to the event is up next " + Time.time);
                    OnExitUnbreathablePlace();
                    GameEvents.Instance.CallEventPlayerRanOutOfOxygen();
                    GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(ranOutOfOxygenText);
                    TheEventHasBeenCalled = true;
                }


            }

            else if(!JustRanOutOfOxygen)
            {
                if (AmountOfOxygenInLastBottle <= 0)
                {
                    AmountOfOxygenBottles--;
                    GameManager.Instance.InventoryController.Inventory.RemoveItem(13, 1);
                    AmountOfOxygenInLastBottle = 1.0f;

                    SaveRelevantData();

                    if (AmountOfOxygenBottles <= 0)
                    {
                        Debug.LogError("Problem with code, this shouldn't happen");
                    }
                }

               // Debug.Log("amount of oxygen in last tank is " + AmountOfOxygenInLastTank + " amount of bottles is " + AmountOfOxygenTanks);
            }

            OxygenHUDText.text = OxygenString + "\n" + (AmountOfOxygenInLastBottle * 1000.0f + (AmountOfOxygenBottles - 1) * 1000.0f).ToString("0") + MgLString;

            //AmountOfOxygenInLastTank -= deltaTime / 120.0f;

        }

        else
        {
            //Debug.Log("Timescale is zero. Do not reduce oxygen");
        }


    }        
    
    public bool CheckIfWeCanEnterUnbreathableArea()
    {
        if (!HasSpaceSuitEquipped
            || AmountOfOxygenBottles <= 0)
        {
            //Debug.LogWarning("Can't enter unbreathable area");
            return false;
        }

        else
        {
            //Debug.LogWarning("CAN enter unbreathable area");
            return true;
        }
    }

    public void PlayerRemovedSpacesuit()
    {
        if (DoOxygenThings) 
        {
            GameManager.Instance.InventoryController.OnInventoryHide(true);
            OnExitUnbreathablePlace();
            GameEvents.Instance.CallEventPlayerRanOutOfOxygen();
            GameEvents.Instance.CallEventPlayerEnteredPromptTrigger(removedSpaceSuitText);
            //TheEventHasBeenCalled = true;
        }
    }

    public void SaveRelevantData()
    {
        GameManager.Instance.SaverLoader.SaveAmountOfOxygenInLastBottle(AmountOfOxygenInLastBottle);
        //Debug.Log("Saving data");
    }

    public float GetAmountOfOxygenInLastBottle()
    {
        return AmountOfOxygenInLastBottle;
    }
}
