using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HydroponicsBay : MonoBehaviour
{
    public float TimeInMinutesThatHydroponicsBayWillRemainSetupAfterRunningOutOfPrecursors = 15;

    public TextMeshProUGUI Status1Text;
    public TextMeshProUGUI Status2Text;

    public bool HasBeenSetup;
    public bool IsProducingOxygen;

    public ISRUItemPlayer PrecursorWater;
    public ISRUItemPlayer PrecursorCarbon;

    public Color GrayedOutButtonBGColor;
    public Color GrayedOutButtonTextColor;

    public Color NonGrayedOutButtonBGColor;
    public Color NonGrayedOutButtonTextColor;

    public Button SetupButton;
    public Button SandwichButton;
    public Button OxygenButton;

    public TextMeshProUGUI SetupButtonText;
    public TextMeshProUGUI SandwichButtonText;
    public TextMeshProUGUI OxygenButtonText;

    private float SetupFlashTimer;
    private float SetupWaitTimer;
    private int SetupCount;
    public bool UpAndRunning;

    private float SandwichFlashTimer;
    private float SandwichWaitTimer;
    private int SandwichCount;
    private float SandwichTimer;
    private float SandwichSetupCount;
    private bool MakingSandwiches;

    private bool SetuppingOxygenProduction;
    private float OxygenWaitTimer;
    private float OxygenFlashTimer;
    private int OxygenSetupCount;

    private bool IsStoppingOxygenProduction;
    private float OxygenStopWaitTimer;
    private float OxygenStopFlashTimer;
    private float OxygenStopSetupCount;

    public const string UnfunctionalRequiresSetupString = "Unfunctional. Requires Setup";
    public const string UpAndRunningString = "Up and Running";
    public const string StartString = "START";
    public const string StopString = "STOP";

    public const string PerformingSetupString = "Performing Setup";

    public const string ProducingOxygenForShipString = "Producing Oxygen for Ship";
    public const string StartingOxygenProductionString = "Starting Oxygen Production";
    public const string StoppingOxygenProductionString = "Stopping Oxygen Production";

    public const string MakingSandwichesString = "Making Sandwiches";
    public const string ProgressString = "Progress [";
    public const string OutOfTenString = "/10]";

    public const string OneDotString = ".";
    public const string TwoDotsString = "..";
    public const string ThreeDotsString = "...";

    private float UpAndRunningFlashTimer;
    private int UpAndRunningCount;

    private float ProducingOxygenFlashTimer;
    private int ProducingOxygenCount;

    // public float TimeThatRanOutOfSetup; // Won't work, if we have save games and all that jazz...

    public void Init()
    {

        HasBeenSetup = false;
        Status1Text.text = UnfunctionalRequiresSetupString;
        Status2Text.text = "";



        //Debug.Log("Initialize hydroponics bay. Maybe we have to implement a save system here about the statuses?");
    }

    public void OnViewOpened()
    {
        if (!IsProducingOxygen)
        {
            OxygenButtonText.text = StartString;
        }

        SetupButton.image.color = NonGrayedOutButtonBGColor;
        SetupButtonText.color = NonGrayedOutButtonTextColor;
        SandwichButton.image.color = NonGrayedOutButtonBGColor;
        SandwichButtonText.color = NonGrayedOutButtonTextColor;
        OxygenButton.image.color = NonGrayedOutButtonBGColor;
        OxygenButtonText.color = NonGrayedOutButtonTextColor;

        ItemSO item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(31);  // Water bottle
        int amount = 0;
        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(31);

        if (inventoryItemScript != null)
        {
            amount = inventoryItemScript.currentItemAmount;
        }

        PrecursorWater.SetupItem(item, amount, this);
        PrecursorWater.MakeNotGrayScaled();

        item = GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(30);  // Carbon
        amount = 0;
        inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(30);

        if (inventoryItemScript != null)
        {
            amount = inventoryItemScript.currentItemAmount;
        }

        PrecursorCarbon.SetupItem(item, amount, this);
        PrecursorCarbon.MakeNotGrayScaled();

        CheckWhichButtonsShouldBeGrayedOut();
        //Debug.Log("Opened hydroponics bay");
    }

    public void CheckWhichButtonsShouldBeGrayedOut()
    {
        int waterAmount = 0;

        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(31); // Water

        if (inventoryItemScript != null)
        {
            waterAmount = inventoryItemScript.currentItemAmount;
        }

        int carbonAmount = 0;

        inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(30); // Carbon

        if (inventoryItemScript != null)
        {
            carbonAmount = inventoryItemScript.currentItemAmount;
        }

        if (waterAmount < 5
            || carbonAmount < 5
            || HasBeenSetup)
        {
            SetupButton.image.color = GrayedOutButtonBGColor;
            SetupButtonText.color = GrayedOutButtonTextColor;
        }

        else
        {
            SetupButton.image.color = NonGrayedOutButtonBGColor;
            SetupButtonText.color = NonGrayedOutButtonTextColor;
        }

        if (waterAmount < 1
            || carbonAmount < 2
            || !HasBeenSetup
            || MakingSandwiches
            || SetuppingOxygenProduction
            || IsStoppingOxygenProduction)
        {
            SandwichButton.image.color = GrayedOutButtonBGColor;
            SandwichButtonText.color = GrayedOutButtonTextColor;
        }

        else
        {
            SandwichButton.image.color = NonGrayedOutButtonBGColor;
            SandwichButtonText.color = NonGrayedOutButtonTextColor;
        }

        if (waterAmount < 1
            || carbonAmount < 1
            || !HasBeenSetup
            || MakingSandwiches
            || SetuppingOxygenProduction
            || IsStoppingOxygenProduction)
        {
            OxygenButton.image.color = GrayedOutButtonBGColor;
            OxygenButtonText.color = GrayedOutButtonTextColor;
        }

        else
        {
            OxygenButton.image.color = NonGrayedOutButtonBGColor;
            OxygenButtonText.color = NonGrayedOutButtonTextColor;
        }

        if (waterAmount <= 0)
        {
            PrecursorWater.MakeGrayScaled();
        }

        else
        {
            PrecursorWater.MakeNotGrayScaled();
        }

        if (carbonAmount <= 0)
        {
            PrecursorCarbon.MakeGrayScaled();
        }

        else
        {
            PrecursorCarbon.MakeNotGrayScaled();
        }
    }

    public void OnCloseWindow()
    {
        PrecursorWater.MakeNotGrayScaled();
        PrecursorCarbon.MakeNotGrayScaled();
        GameManager.Instance.InventoryController.OnHydroponicsBayHide();
        //Debug.Log("Closed hydroponics bay");
    }

    public void OnSetupButtonPressed()
    {
        if (!HasBeenSetup
            && CheckIfWeHaveEnoughPrecursors(5, 5)) 
        {
            UpAndRunning = false;
            HasBeenSetup = true;
            Status1Text.gameObject.SetActive(true);
            Status1Text.text = PerformingSetupString;
            SetupWaitTimer = 4.0f;
            SetupCount = 0;
            //SetupTimer = 10.0f;

            SetupButton.image.color = GrayedOutButtonBGColor;
            SetupButtonText.color = GrayedOutButtonTextColor;

            RemoveFromInventory(5, 5);

            //CheckWhichButtonsShouldBeGrayedOut();
            //Debug.Log("Pressed setup button");
        }

        else
        {
            //Debug.Log("Already setup or not enough precursors to setup hydroponics bay");
        }
    }

    public void OnStartOxygenProductionButtonPressed()
    {
        if (HasBeenSetup 
            && SetupWaitTimer <= 0
            && !MakingSandwiches
            && !SetuppingOxygenProduction
            && !IsStoppingOxygenProduction)
        {
            if (!IsProducingOxygen 
                && CheckIfWeHaveEnoughPrecursors(1, 1)) 
            {
                OxygenWaitTimer = 4.0f;
                OxygenFlashTimer = 1.0f;
                OxygenSetupCount = 0;
                SetuppingOxygenProduction = true;
                Status2Text.text = StartingOxygenProductionString;

                //RemoveFromInventory(1, 1);

                CheckWhichButtonsShouldBeGrayedOut();
                //Debug.Log("Pressed oxygen production button");
            }

            else if (IsProducingOxygen)
            {
                IsStoppingOxygenProduction = true;
                OxygenStopWaitTimer = 4.0f;
                OxygenStopFlashTimer = 1.0f;
                OxygenStopSetupCount = 0;

                Status2Text.text = StoppingOxygenProductionString;

                CheckWhichButtonsShouldBeGrayedOut();

                //Debug.Log("Should stop oxygen production just about now");
            }

            else
            {
                //Debug.Log("We are already producing oxygen or we don't have enough precursors to start oxygen production");
            }
        }

        else
        {
            //Debug.Log("Hydroponics bay requires setup before producing oxygen");
        }
    }

    public void OnMakeSandwichesButtonPressed()
    {
        if (HasBeenSetup 
            && SetupWaitTimer <= 0
            && !MakingSandwiches
            && !SetuppingOxygenProduction
            && !IsStoppingOxygenProduction) 
        {
            //Debug.Log("Pressed sandwiches button");

            if (CheckIfWeHaveEnoughPrecursors(1, 2))
            {                
                MakingSandwiches = true;
                SandwichWaitTimer = 4.0f;
                SandwichCount = 1;
                Status1Text.gameObject.SetActive(true);
                Status1Text.text = MakingSandwichesString;
                Status2Text.gameObject.SetActive(true);
                Status2Text.text = ProgressString + SandwichCount + OutOfTenString;
                SandwichFlashTimer = 1.0f;
                SandwichTimer = 0.3636f;
                SandwichSetupCount = 0;


                RemoveFromInventory(1, 2);

                CheckWhichButtonsShouldBeGrayedOut();


                //Debug.Log("We have enough precursors to make sandwiches");
            }

            else
            {
                //Debug.Log("Not enough precursors to make sandwiches");
            }
        }

        else
        {
            //Debug.Log("Hydroponics bay requires setup before sandiwches can be made");
        }
    }

    public void OnPrecursorPressed(ISRUItemPlayer item)
    {
        //Debug.Log("Pressed precursor, but should anything even happen?");
    }

    public bool CheckIfWeHaveEnoughPrecursors(int requiredWater,
                                              int requiredCarbon)
    {
        bool enoughWater = false;
        bool enoughCarbon = false;

        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(31); // Water

        int amount = 0;

        if (inventoryItemScript != null)
        {
            amount = inventoryItemScript.currentItemAmount;
        }

        if (amount >= requiredWater)
        {
            enoughWater = true;
        }

        inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(30); // Water

        amount = 0;

        if (inventoryItemScript != null)
        {
            amount = inventoryItemScript.currentItemAmount;
        }

        if (amount >= requiredCarbon)
        {
            enoughCarbon = true;
        }

        if (enoughWater && enoughCarbon)
        {
            return true;
        }

        else
        {
            return false;
        }
    }



    private void Update()
    {
        //Debug.Log("Setup timer value is " + SetupTimer);

        if (SetupWaitTimer > 0)
        {
            SetupFlashTimer -= Time.unscaledDeltaTime;

            if (SetupFlashTimer <= 0)
            {
                if (Status1Text.gameObject.activeSelf)
                {
                    Status1Text.gameObject.SetActive(false);
                    SetupFlashTimer = 0.25f;
                }

                else
                {
                    Status1Text.gameObject.SetActive(true);
                    SetupFlashTimer = 1.0f;
                    SetupCount++;

                    if (SetupCount == 1)
                    {
                        Status1Text.text = PerformingSetupString + OneDotString;
                    }

                    else if (SetupCount == 2)
                    {
                        Status1Text.text = PerformingSetupString + TwoDotsString;
                    }

                    else
                    {
                        Status1Text.text = PerformingSetupString + ThreeDotsString;
                    }
                }
            }

            SetupWaitTimer -= Time.unscaledDeltaTime;

            if (SetupWaitTimer <= 0)
            {
                OnSetupComplete();
            }
        }

        if (SandwichWaitTimer > 0)
        {
            SandwichWaitTimer -= Time.unscaledDeltaTime;

            SandwichFlashTimer -= Time.unscaledDeltaTime;

            if(SandwichFlashTimer <= 0)
            {
                if (Status1Text.gameObject.activeSelf)
                {
                    Status1Text.gameObject.SetActive(false);
                    SandwichFlashTimer = 0.25f;
                }

                else
                {
                    Status1Text.gameObject.SetActive(true);
                    SandwichFlashTimer = 1.0f;                
                    SandwichSetupCount++;

                    if (SandwichSetupCount == 1)
                    {
                        Status1Text.text = MakingSandwichesString + OneDotString;
                    }

                    else if (SandwichSetupCount == 2)
                    {
                        Status1Text.text = MakingSandwichesString + TwoDotsString;
                    }

                    else
                    {
                        Status1Text.text = MakingSandwichesString + ThreeDotsString;
                    }
                }
            }

            SandwichTimer -= Time.unscaledDeltaTime;

            if (SandwichTimer <= 0)
            {
                SandwichTimer = 0.3636f;
                SandwichCount++;

                if (SandwichCount >= 10)
                {
                    SandwichCount = 10;
                }

                Status2Text.text = ProgressString + SandwichCount + OutOfTenString;
            }

            if (SandwichWaitTimer <= 0)
            {
                OnMakingSandwichesComplete();
            }
        }

        if (SetuppingOxygenProduction)
        {
            OxygenWaitTimer -= Time.unscaledDeltaTime;

            OxygenFlashTimer -= Time.unscaledDeltaTime;

            if (OxygenFlashTimer <= 0)
            {
                if (Status2Text.gameObject.activeSelf)
                {
                    Status2Text.gameObject.SetActive(false);
                    OxygenFlashTimer = 0.25f;
                }

                else
                {
                    Status2Text.gameObject.SetActive(true);
                    OxygenFlashTimer = 1.0f;

                    OxygenSetupCount++;

                    if (OxygenSetupCount == 1)
                    {
                        Status2Text.text = StartingOxygenProductionString + OneDotString;
                    }

                    else if (OxygenSetupCount == 2)
                    {
                        Status2Text.text = StartingOxygenProductionString + TwoDotsString;
                    }

                    else
                    {
                        Status2Text.text = StartingOxygenProductionString + ThreeDotsString;
                    }
                }
            }

            if (OxygenWaitTimer <= 0)
            {
                OnOxygenProcutionSetupComplete();
            }
        }

        if (IsStoppingOxygenProduction)
        {
            OxygenStopWaitTimer -= Time.unscaledDeltaTime;

            OxygenStopFlashTimer -= Time.unscaledDeltaTime;

            if (OxygenStopFlashTimer <= 0)
            {
                if (Status2Text.gameObject.activeSelf)
                {
                    Status2Text.gameObject.SetActive(false);
                    OxygenStopFlashTimer = 0.25f;
                }

                else
                {
                    Status2Text.gameObject.SetActive(true);
                    OxygenStopFlashTimer = 1.0f;

                    OxygenStopSetupCount++;

                    if (OxygenStopSetupCount == 1)
                    {
                        Status2Text.text = StoppingOxygenProductionString + OneDotString;
                    }

                    else if(OxygenStopSetupCount == 2)
                    {
                        Status2Text.text = StoppingOxygenProductionString + TwoDotsString;
                    }

                    else
                    {
                        Status2Text.text = StoppingOxygenProductionString + ThreeDotsString;
                    }
                }
            }

            if (OxygenStopWaitTimer <= 0)
            {
                OnStopOxygenProductionComplete();
            }
        }

        if (!MakingSandwiches
            && HasBeenSetup
            && UpAndRunning)
        {
            UpAndRunningFlashTimer -= Time.unscaledDeltaTime;

            if (UpAndRunningFlashTimer <= 0)
            {
                UpAndRunningFlashTimer = 0.8f;

                if (UpAndRunningCount == 0)
                {
                    Status1Text.text = UpAndRunningString;
                }

                else if (UpAndRunningCount == 1)
                {
                    Status1Text.text = UpAndRunningString + OneDotString;
                }

                else if (UpAndRunningCount == 2)
                {
                    Status1Text.text = UpAndRunningString + TwoDotsString;
                }

                else
                {
                    Status1Text.text = UpAndRunningString + ThreeDotsString;
                }

                UpAndRunningCount++;

                if (UpAndRunningCount > 3)
                {
                    UpAndRunningCount = 0;
                }
            }
        }


        if (!IsStoppingOxygenProduction
            && !SetuppingOxygenProduction
            && !MakingSandwiches
            && IsProducingOxygen)
        {
            ProducingOxygenFlashTimer -= Time.unscaledDeltaTime;

            if (ProducingOxygenFlashTimer <= 0) 
            {
                ProducingOxygenFlashTimer = 1.0f;

                if (ProducingOxygenCount == 0)
                {
                    Status2Text.text = ProducingOxygenForShipString;
                }

                else if (ProducingOxygenCount == 1)
                {
                    Status2Text.text = ProducingOxygenForShipString + OneDotString;
                }

                else if (ProducingOxygenCount == 2)
                {
                    Status2Text.text = ProducingOxygenForShipString + TwoDotsString;
                }

                else
                {
                    Status2Text.text = ProducingOxygenForShipString + ThreeDotsString;
                }

                ProducingOxygenCount++;

                if (ProducingOxygenCount > 3)
                {
                    ProducingOxygenCount = 0;
                }
            }
        }
        //if (SetupTimer > 0)
        //{
        //    SetupTimer -= Time.unscaledDeltaTime;

        //    if (SetupTimer <= 0)
        //    {
        //        OnRanOutOfSetup();
        //    }
        //}
    }

    private void OnSetupComplete()
    {
        UpAndRunning = true;
        Status1Text.gameObject.SetActive(true);
        Status1Text.text = UpAndRunningString;
        UpAndRunningFlashTimer = 0.8f;
        UpAndRunningCount = 0;

        GameManager.Instance.ShipLifeSupportSystem.OnHydroponicsBaySetup();

        CheckWhichButtonsShouldBeGrayedOut();
    }

    private void OnOxygenProcutionSetupComplete()
    {
        SetuppingOxygenProduction = false;
        IsProducingOxygen = true;
        Status2Text.gameObject.SetActive(true);
        Status2Text.text = ProducingOxygenForShipString;
        OxygenFlashTimer = 1.0f;

        OxygenButtonText.text = StopString;

        GameManager.Instance.ShipLifeSupportSystem.OnOxygenProductionStarted();

        CheckWhichButtonsShouldBeGrayedOut();
    }

    private void OnMakingSandwichesComplete()
    {
        MakingSandwiches = false;
        GameManager.Instance.InventoryController.Inventory.AddItem(20, 10);

        Status2Text.gameObject.SetActive(true);

        if (IsProducingOxygen)
        {
            Status2Text.text = ProducingOxygenForShipString;
        }

        else
        {
            Status2Text.text = "";
        }

        Status1Text.gameObject.SetActive(true);
        Status1Text.text = UpAndRunningString;

        CheckWhichButtonsShouldBeGrayedOut();
    }

    public void OnStopOxygenProductionComplete()
    {
        IsProducingOxygen = false;
        IsStoppingOxygenProduction = false;

        Status2Text.gameObject.SetActive(true);
        Status2Text.text = "";

        OxygenButtonText.text = StartString;

        CheckWhichButtonsShouldBeGrayedOut();
    }

    public void OnRanOutOfSetup()
    {
        UpAndRunning = false;
        HasBeenSetup = false;
        IsProducingOxygen = false;

        Status1Text.text = UnfunctionalRequiresSetupString;
        Status2Text.text = "";

        //Debug.Log("On ran out of setup");
        CheckWhichButtonsShouldBeGrayedOut();
    }

    public void OnRanOutOfOxygenProduction()
    {
        IsProducingOxygen = false;
        Status2Text.text = "";
        //Debug.Log("Hydroponics bay knows we ran out of oxygen production materials");
    }

    public void RemoveFromInventory(int amountOfWaterToRemove,
                                    int amountOfCarbonToRemove)
    {
        GameManager.Instance.InventoryController.Inventory.RemoveItem(31, amountOfWaterToRemove);
        GameManager.Instance.InventoryController.Inventory.RemoveItem(30, amountOfCarbonToRemove);

        ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(31);
        
        int newAmountOfWater = 0;

        if (inventoryItemScript != null)
        {
            newAmountOfWater = inventoryItemScript.currentItemAmount;
        }

        int newAmountOfCarbon = 0;

        inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(30);

        if (inventoryItemScript != null)
        {
            newAmountOfCarbon = inventoryItemScript.currentItemAmount;
        }

        UpdateInventoryAmounts(newAmountOfWater, 
                               newAmountOfCarbon);
    }

    public void UpdateInventoryAmounts(int newAmountOfWater,
                                       int newAmountOfCarbon)
    {

        PrecursorWater.UpdateAmount(newAmountOfWater);
        PrecursorCarbon.UpdateAmount(newAmountOfCarbon);

        if (ResourceInventory.Instance != null)
        {
            ResourceInventory.Instance.SetResourceAmounts(GameManager.Instance.InventoryController.Inventory);
        }
    }
}
