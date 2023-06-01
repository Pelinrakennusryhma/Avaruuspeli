using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShipLifeSupportSystem : MonoBehaviour
{
    public GameObject HUDParent;
    public TextMeshProUGUI HUDText1;
    public TextMeshProUGUI HUDText2;

    public float AmountOfWaterInLastBottle;
    public float AmountOfCarbonInLastUnit;
    public float AmountOfOxygenInLastStorage;

    public int AmountOfCarbon;
    public int AmountOfWaterBottles;
    public int AmountOfOxygenStorages;

    public bool DoOxygenThings;

    private float WaterAndCarbonConsumptionRatePerMinute = 0.01f;
    //private float WaterAndCarbonConsumptionRatePerMinute = 60.0f;
    //private float OxygenStorageConsumptionRatePerMinute = 30.0f;
    private float OxygenStorageConsumptionRatePerMinute = 0.1f;

    public GameObject NoOxygenPromptParent;
    public TextMeshProUGUI NoOxygenPrompt;
    public const string NoOxygenPromptString = "No Oxygen resources available. \nCan't enter ship. \nGet some Oxygen storages or activate Hydroponics Bay's Oxygen Production";
    public const string RanOutOfOxygenPromptString = "Ran out of Oxygen for ship. \nReturning to mothership.";

    public const string HydroponicsBayIsActiveString = "HYDROPONICS BAY IS ACTIVE \nAND PRODUCING OXYGEN FOR: ";
    public const string HydroPonicsBayMinutesString = "MIN.";
    public const string HydroPonicsBaySecondsString = "SECONDS";

    public const string OxygenString ="OXYGEN IN STORAGES: ";
    public const string MgLString = " mg/L";

    private float RanOutOfOxygenPromptTimer;
    private float RanOutOfOxygenPromptTimerLength = 6.0f;

    private float NoOxygenPromptTimer;
    private float NoOxygenPromptTimerLenght = 6.0f;

    private bool RanOutOfOxygenOnHydroponicsBay;
    private bool RanOutOfOxygenInTanks;

    private float TimeThatHydroponicsBayHasBeenOutOfResources;

    public bool WaitingToShowRunningOutOfOxygenPrompt;

    public void Init()
    {
        //Debug.LogWarning("Initting ship life support system");
        HUDParent.SetActive(false);
        NoOxygenPromptParent.SetActive(false);
        AmountOfCarbonInLastUnit = 1.0f;
        AmountOfWaterInLastBottle = 1.0f;
        AmountOfOxygenInLastStorage = 1.0f;
    }

    public bool CheckIfWeCanEnterShip()
    {
        UpdateWaterBottlesAndCarbon();
        UpdateOxygenStorages();

        if (GameManager.Instance.InventoryController.HydroponicsBay.IsProducingOxygen
            || AmountOfOxygenStorages > 0)
        {
            //Debug.Log("We can enter ship. We have oxygen to do things");
            return true;
        }

        else
        {
            //Debug.Log("We can't enter ship, because we don't have oxygen storages or hydroponics bay active");
            return false;
        }
    }

    public void DisplayPromptAboutNotBeingAbleToEnterShip()
    {
        // Where do we parent the prompt and which prompt? Main camera probably.
        // Probably we show it just for a set period of time, so it is not left on.

        NoOxygenPromptParent.transform.SetParent(Camera.main.transform);
        NoOxygenPromptParent.SetActive(true);
        NoOxygenPrompt.text = NoOxygenPromptString;
        NoOxygenPromptTimer = NoOxygenPromptTimerLenght;
        //Debug.Log("we can't enter ship, because we don't have oxygen. Display a prompt about this.");
    }

    public void DisablePromptAboutNotBeingAbleToEnterShip()
    {
        NoOxygenPromptParent.transform.SetParent(GameManager.Instance.gameObject.transform);
        NoOxygenPromptParent.SetActive(false);
    }

    public void OnEnterShip()
    {
        UpdateWaterBottlesAndCarbon();
        UpdateOxygenStorages();

        DoOxygenThings = true;
        HUDParent.SetActive(true);
        HUDParent.transform.SetParent(Camera.main.transform);

        if (GameManager.Instance.InventoryController.HydroponicsBay.IsProducingOxygen)
        {
            ActivateHydroponicsBayHUD();
        }

        else
        {
            ActivateOxygenStorageHUD();
        }

        //Debug.Log("Entered ship");
    }

    // Enter planet, asteroid or world map
    public void OnExitShip()
    {
        DoOxygenThings = false;
        HUDParent.transform.SetParent(GameManager.Instance.gameObject.transform);
        HUDParent.SetActive(false);
        //Debug.Log("Exited ship");
    }

    public void ActivateHydroponicsBayHUD()
    {
        float totalAmountOfCarbon, totalAmountOfWater, consumptionPerSecond, minutesLeftToProduceOxygen;
        CalculateHowLongHydroponicsBayProducesOxygen(out totalAmountOfCarbon,
                                                     out totalAmountOfWater,
                                                     out consumptionPerSecond,
                                                     out minutesLeftToProduceOxygen);

        HUDText1.gameObject.SetActive(false);
        HUDText2.gameObject.SetActive(true);
        HUDText2.text = HydroponicsBayIsActiveString + " \n" + minutesLeftToProduceOxygen.ToString("0") + " "+ HydroPonicsBayMinutesString;
        //Debug.Log("Activate hydroponics bay hud");
    }

    public void ActivateOxygenStorageHUD()
    {
        HUDText1.gameObject.SetActive(true);
        HUDText2.gameObject.SetActive(false);
        HUDText1.text = OxygenString + "\n" + (AmountOfOxygenInLastStorage * 1000 + (AmountOfOxygenStorages - 1) * 1000).ToString("0.0") + MgLString;
        //Debug.Log("Activate oxygen storage hud");
    }

    // Implement a system for oxygen storages
    // Also, make the system compatible and work in unison with hydroponics bay
    // When Hydroponics bay is producing oxygen, there should be no consumption of oxygen storages
    // However, we have to reduce water and carbon at correct intervalls because they are consumed by the hydroponics bay
    // Also, if we run out of carbon or water, we need to turn the hydroponics bay off after a while. How long of a while? I do not know.

    public void OnOxygenProductionStarted()
    {        
        UpdateWaterBottlesAndCarbon();
        AmountOfWaterInLastBottle = 1.0f;
        AmountOfCarbonInLastUnit = 1.0f;

        if (DoOxygenThings)
        {
            ActivateHydroponicsBayHUD();
        }

        //Debug.Log("Started oxygen production");
    }

    public void OnHydroponicsBaySetup()
    {
        TimeThatHydroponicsBayHasBeenOutOfResources = 0;
        UpdateWaterBottlesAndCarbon();
    }

    public void UpdateWaterBottlesAndCarbon()
    {
        int previousAmountOfWaterBottles = AmountOfWaterBottles;
        int previousAmountOfCarbon = AmountOfCarbon;

        int amountOfWaterBottles = 0;
        int amountOfCarbon = 0;

        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(31); // Water bottle

        if (inventoryItemScript != null)
        {
            amountOfWaterBottles = inventoryItemScript.currentItemAmount;
        }

        inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(30); // Carbon

        if (inventoryItemScript != null)
        {
            amountOfCarbon = inventoryItemScript.currentItemAmount;
        }

        AmountOfWaterBottles = amountOfWaterBottles;
        AmountOfCarbon = amountOfCarbon;

        if (previousAmountOfCarbon <= 0
            && AmountOfCarbon > 0)
        {
            AmountOfCarbonInLastUnit = 1.0f;
        }

        if (previousAmountOfWaterBottles <= 0
            && AmountOfWaterBottles > 0)
        {
            AmountOfWaterInLastBottle = 1.0f;
        }

        if (AmountOfCarbon <= 0)
        {
            AmountOfCarbonInLastUnit = 0;
        }

        if (AmountOfWaterBottles <= 0)
        {
            AmountOfWaterInLastBottle = 0;
        }

        if (AmountOfCarbon >= 1
            && AmountOfWaterBottles >= 1)
        {
            RanOutOfOxygenOnHydroponicsBay = false;
        }
    }

    public void UpdateOxygenStorages()
    {
        int previousAmountOfOxygenStorages = AmountOfOxygenStorages;

        int amountOfOxygenStorages = 0;

        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(16); // Oxygen storage

        if (inventoryItemScript != null)
        {
            amountOfOxygenStorages = inventoryItemScript.currentItemAmount;
        }

        if (previousAmountOfOxygenStorages <= 0
            && amountOfOxygenStorages > 0)
        {
            AmountOfOxygenInLastStorage = 1.0f;
        }

        AmountOfOxygenStorages = amountOfOxygenStorages;

        if (AmountOfOxygenStorages <= 0)
        {
            AmountOfOxygenInLastStorage = 0;
        }

        if (amountOfOxygenStorages >= 1)
        {
            RanOutOfOxygenInTanks = false;
        }
    }

    public void ZeroOutCarbon()
    {
        AmountOfCarbon = 0;
        AmountOfCarbonInLastUnit = 0;
    }

    public void ZeroOutWaterBottles()
    {
        AmountOfWaterBottles = 0;
        AmountOfWaterInLastBottle = 0;
    }

    public void ZeroOutOxygenStorages()
    {
        AmountOfOxygenInLastStorage = 0;
        AmountOfOxygenStorages = 0;
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    GameManager.Instance.InventoryController.Inventory.AddItem(30, 100);
        //    GameManager.Instance.InventoryController.Inventory.AddItem(31, 100);
        //    GameManager.Instance.InventoryController.Inventory.AddItem(16, 100);

        //    UpdateOxygenStorages();
        //    UpdateWaterBottlesAndCarbon();

        //    Debug.Log("Adding water and carbon and oxygen storages");
        //}

        if (RanOutOfOxygenPromptTimer > 0)
        {
            RanOutOfOxygenPromptTimer -= Time.deltaTime;

            if (RanOutOfOxygenPromptTimer <= 0)
            {
                DisableRunningOutOfOxygenPrompt();
            }
        }

        if (NoOxygenPromptTimer > 0)
        {
            NoOxygenPromptTimer -= Time.unscaledDeltaTime;
            
            //Debug.Log("Substracting from no oxygen prompt timer. Timer value is " + NoOxygenPromptTimer);

            if (NoOxygenPromptTimer <= 0)
            {
                DisablePromptAboutNotBeingAbleToEnterShip();
            }
        }

        if (DoOxygenThings) 
        {
            if (GameManager.Instance.InventoryController.HydroponicsBay.IsProducingOxygen)
            {
                TrackHydroponicsBayResources();
            }

            else
            {
                TrackOxygenStorages();
            }
        }

        if ((AmountOfCarbon <= 0
            || AmountOfWaterBottles <= 0)
            && GameManager.Instance.InventoryController.HydroponicsBay.HasBeenSetup
            && GameManager.Instance.InventoryController.HydroponicsBay.UpAndRunning)
        {
            TimeThatHydroponicsBayHasBeenOutOfResources += Time.deltaTime;

            if ((TimeThatHydroponicsBayHasBeenOutOfResources / 60)
                >= GameManager.Instance.InventoryController.HydroponicsBay.TimeInMinutesThatHydroponicsBayWillRemainSetupAfterRunningOutOfPrecursors)
            {
                GameManager.Instance.InventoryController.HydroponicsBay.OnRanOutOfSetup();
            }
            //Debug.Log("We are tracking time that hydroponics bay has been out of resources " + TimeThatHydroponicsBayHasBeenOutOfResources);
        }
    }

    private void TrackHydroponicsBayResources()
    {
        // Show how many minutes we are going to produce oxygen

        if (!RanOutOfOxygenOnHydroponicsBay
            && Time.deltaTime != 0)
        {
            float totalAmountOfCarbon, totalAmountOfWater, consumptionPerSecond, minutesLeftToProduceOxygen;
            CalculateHowLongHydroponicsBayProducesOxygen(out totalAmountOfCarbon, 
                                                         out totalAmountOfWater, 
                                                         out consumptionPerSecond, 
                                                         out minutesLeftToProduceOxygen);

            AmountOfCarbonInLastUnit -= consumptionPerSecond * Time.deltaTime;
            AmountOfWaterInLastBottle -= consumptionPerSecond * Time.deltaTime;

            bool ranOutOfWaterOrCarbon = false;

            if (AmountOfCarbonInLastUnit <= 0)
            {
                //Debug.Log("Substracting carbon " );
                AmountOfCarbon--;
                GameManager.Instance.InventoryController.Inventory.RemoveItem(30, 1);

                if (AmountOfCarbon <= 0)
                {
                    AmountOfCarbonInLastUnit = 0;
                    ranOutOfWaterOrCarbon = true;
                }

                else
                {
                    AmountOfCarbonInLastUnit = 1.0f;
                }



            }

            if (AmountOfWaterInLastBottle <= 0)
            {
                //Debug.Log("Substracting water");
                AmountOfWaterBottles--;
                GameManager.Instance.InventoryController.Inventory.RemoveItem(31, 1);

                if (AmountOfWaterBottles <= 0)
                {
                    AmountOfWaterInLastBottle = 0;
                    ranOutOfWaterOrCarbon = true;
                }

                else
                {
                    AmountOfWaterInLastBottle = 1.0f;
                }



            }

            if (ranOutOfWaterOrCarbon)
            {
                OnRanOutOfOxygenOnHydroponicsBay();
                //Debug.LogError("Ran out of water or carbon " + Time.time);
            }

            //totalAmountOfCarbon = (AmountOfCarbon - 1) + AmountOfCarbonInLastUnit;
            //totalAmountOfWater = (AmountOfWaterBottles - 1) + AmountOfWaterInLastBottle;
            
            if (minutesLeftToProduceOxygen > 1.0f) 
            {
                HUDText2.text = HydroponicsBayIsActiveString + " \n" + minutesLeftToProduceOxygen.ToString("0") + " " + HydroPonicsBayMinutesString;
            }

            else
            {
                float seconds = minutesLeftToProduceOxygen * 60;
                //Debug.Log("sceonds is " + seconds);
                HUDText2.text = HydroponicsBayIsActiveString + " \n" + seconds.ToString("0") + " " + HydroPonicsBaySecondsString;
            }

            //Debug.Log("Tracking hydroponics bay resources " + Time.time + " minutes left to produce oxygen is " + minutesLeftToProduceOxygen + " total amount of water is " + totalAmountOfWater + " total amount of carbon is " + totalAmountOfCarbon);
            //Debug.Log("Tracking hydroponics bay resources " + Time.time + " amount of carbon in last unit is " + AmountOfCarbonInLastUnit+ " amount of water in last bottle is " + AmountOfWaterInLastBottle);
        }

    }

    private void CalculateHowLongHydroponicsBayProducesOxygen(out float totalAmountOfCarbon, 
                                                              out float totalAmountOfWater, 
                                                              out float consumptionPerSecond, 
                                                              out float minutesLeftToProduceOxygen)
    {
        totalAmountOfCarbon = (AmountOfCarbon - 1) + AmountOfCarbonInLastUnit;
        totalAmountOfWater = (AmountOfWaterBottles - 1) + AmountOfWaterInLastBottle;
        float smallerOfTheTwo;

        if (totalAmountOfCarbon < totalAmountOfWater)
        {
            smallerOfTheTwo = totalAmountOfCarbon;
        }

        else
        {
            smallerOfTheTwo = totalAmountOfWater;
        }

        consumptionPerSecond = WaterAndCarbonConsumptionRatePerMinute / 60.0f;
        float secondsLeftToProduceOxygen = smallerOfTheTwo / consumptionPerSecond;
        minutesLeftToProduceOxygen = secondsLeftToProduceOxygen / 60.0f;
    }

    private void TrackOxygenStorages()
    {

        if (!RanOutOfOxygenInTanks
            && Time.deltaTime != 0)
        {
            float consumptionPerSecond = OxygenStorageConsumptionRatePerMinute / 60.0f;

            AmountOfOxygenInLastStorage -= Time.deltaTime * consumptionPerSecond; // Is this calculation even correct?

            if (AmountOfOxygenInLastStorage <= 0
                && AmountOfOxygenStorages <= 1)
            {
                OnRanOutOfOxygenInTanks();
            }

            else if (AmountOfOxygenInLastStorage <= 0)
            {
                AmountOfOxygenStorages--;
                GameManager.Instance.InventoryController.Inventory.RemoveItem(16, 1);
                AmountOfOxygenInLastStorage = 1.0f;

                if (AmountOfOxygenStorages <= 0)
                {
                    Debug.LogError("How did we get here?");
                }
            }

            // Put this thing on hud

            if (AmountOfOxygenStorages > 0) 
            {
                HUDText1.text = OxygenString + "\n" + (AmountOfOxygenInLastStorage * 1000 + (AmountOfOxygenStorages - 1) * 1000).ToString("0") + MgLString;
            }

            else
            {
                HUDText1.text = OxygenString + "\n" + 0.ToString() + MgLString;
            }


        }

        // Show some sort of amount of oxygen left
        //Debug.Log("Tracking oxygen storages " + Time.time);
    }

    public void OnRanOutOfOxygenInTanks()
    {
        GameManager.Instance.InventoryController.Inventory.RemoveItem(16, 1);
        AmountOfOxygenInLastStorage = 0;
        AmountOfOxygenStorages = 0;
        RanOutOfOxygenInTanks = true;

        //Debug.LogWarning("Ran out of oxygen in tanks");
        ThrowBackToWorldMap();


    }

    public void OnRanOutOfOxygenOnHydroponicsBay()
    {
        RanOutOfOxygenOnHydroponicsBay = true;
        GameManager.Instance.InventoryController.HydroponicsBay.OnRanOutOfOxygenProduction();

        if (AmountOfOxygenStorages <= 0)
        {
            ThrowBackToWorldMap();
        }

        else
        {
            ActivateOxygenStorageHUD();
        }
    }

    public void ThrowBackToWorldMap()
    {
        WaitingToShowRunningOutOfOxygenPrompt = true;
        OnExitShip();
        GameManager.Instance.GoBackToWorldMap();


        //Debug.LogError("Ran out of oxygen, we should probably return to mothership. And display a prompt about running out of oxygen");

    }

    public void ActivateRunninOutOfOxygenPrompt()
    {
        NoOxygenPromptParent.gameObject.transform.SetParent(Camera.main.transform);
        NoOxygenPrompt.text = RanOutOfOxygenPromptString;
        NoOxygenPromptParent.gameObject.SetActive(true);
        WaitingToShowRunningOutOfOxygenPrompt = false;
        RanOutOfOxygenPromptTimer = RanOutOfOxygenPromptTimerLength;
        //Debug.LogError("Should show a prompt about running out of oxygen");
    }

    public void DisableRunningOutOfOxygenPrompt()
    {
        NoOxygenPromptParent.gameObject.transform.SetParent(Camera.main.transform);
        NoOxygenPromptParent.gameObject.SetActive(false);
        WaitingToShowRunningOutOfOxygenPrompt = false;
        //Debug.LogError("Should hide a prompt about running out of oxygen");
    }
}
