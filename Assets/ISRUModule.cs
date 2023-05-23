using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ISRUModule : MonoBehaviour
{
    public bool ConvertButtonsAreInteractable;

    public TextMeshProUGUI TestText;
    public TMP_InputField InputField;

    //private float TimeUntilFlash = 1.0f;

    public TextMeshProUGUI ResultText;
    public TextMeshProUGUI InfoText;

    public TextMeshProUGUI WeightText;

    public ItemSO InputItem;
    public ItemSO OutputItem;

    public TextMeshProUGUI ButtonOneText;
    public TextMeshProUGUI ButtonTenText;
    public TextMeshProUGUI ButtonHundredText;
    public TextMeshProUGUI ButtonXAmountText;

    public Image ButtonOneImage;
    public Image ButtonTenImage;
    public Image ButtonHundredImage;
    public Image ButtonXAmountImage;

    public Color GrayedOutTextColor;
    public Color InteractableTextColor;

    public Color GrayedOutButtonColor;
    public Color InteractableButtonColor;

    public float TimeUntilBecomesInteractable;

    public Scrollbar PrecursorScrollbar;
    public Scrollbar TargetScrollbar;

    public ISRUItemPlayer ISRUItemPlayerPrefab;
    public ISRUItemTarget ISRUItemTargetPrefab;

    public GameObject InputScrollableContent;
    public GameObject OutputScrollableContent;

    public ISRUItemPlayer[] InputItems;
    public ISRUItemTarget[] OutputItems;

    public ISRUItemPlayer SelectedPrecursor;
    public ISRUItemTarget SelectedTarget;

    private float resultTextTimer;

    public void Init()
    {
        //TestText = GetComponent<TextMeshProUGUI>();

        InputItems = new ISRUItemPlayer[3];
        OutputItems = new ISRUItemTarget[5];

        InputItems[0] = Instantiate(ISRUItemPlayerPrefab, InputScrollableContent.transform);
        InputItems[0].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(27), 0, this);
        InputItems[1] = Instantiate(ISRUItemPlayerPrefab, InputScrollableContent.transform);
        InputItems[1].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(28), 0, this);
        InputItems[2] = Instantiate(ISRUItemPlayerPrefab, InputScrollableContent.transform);
        InputItems[2].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(30), 0, this);

        OutputItems[0] = Instantiate(ISRUItemTargetPrefab, OutputScrollableContent.transform);
        OutputItems[0].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(13), 0, this);
        OutputItems[1] = Instantiate(ISRUItemTargetPrefab, OutputScrollableContent.transform);
        OutputItems[1].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(31), 0, this);
        OutputItems[2] = Instantiate(ISRUItemTargetPrefab, OutputScrollableContent.transform);
        OutputItems[2].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(30), 0, this);
        OutputItems[3] = Instantiate(ISRUItemTargetPrefab, OutputScrollableContent.transform);
        OutputItems[3].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(15), 0, this);
        OutputItems[4] = Instantiate(ISRUItemTargetPrefab, OutputScrollableContent.transform);
        OutputItems[4].SetupItem(GameManager.Instance.InventoryController.ItemDataBaseWithScriptables.ItemDataBaseSO.GetItem(6), 0, this);


    }

    public void OnViewOpened()
    {
        InfoText.text = "";
        ResultText.text = "";
        SelectedPrecursor = null;
        SelectedTarget = null;
        InputField.text = 1.ToString();

        UpdateLists();

        PrecursorScrollbar.value = 1.0f;
        TargetScrollbar.value = 1.0f;
        MakeButtonsUninteractable();
        //MakeButtonsInteractable();
        TimeUntilBecomesInteractable = 3.0f;

        for (int i = 0; i < InputItems.Length; i++)
        {
            InputItems[i].OnDeselected();
        }

        for (int i = 0; i < OutputItems.Length; i++)
        {
            OutputItems[i].OnDeselected();
        }
    }

    public void UpdateLists()
    {
        for (int i = 0; i < InputItems.Length; i++)
        {
            ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(InputItems[i].Item.id);

            int amount = 0;

            if (inventoryItemScript != null)
            {
                amount = inventoryItemScript.currentItemAmount;
            }

            InputItems[i].UpdateAmount(amount);

            if (amount <= 0)             
            {
                InputItems[i].MakeGrayScaled();
            }

            else
            {
                InputItems[i].MakeNotGrayScaled();
            }
        }

        for (int i = 0; i < OutputItems.Length; i++)
        {
            ItemScript inventoryItemScript = GameManager.Instance.InventoryController.Inventory.GetItemScript(OutputItems[i].Item.id);

            int amount = 0;

            if (inventoryItemScript != null)
            {
                amount = inventoryItemScript.currentItemAmount;
            }

            int precursorAmount = 0;
            int itemId = -1;

            itemId = OutputItems[i].Item.id;

            precursorAmount = CheckForPrecursors(precursorAmount, itemId);

            if (precursorAmount <= 0)
            {
                OutputItems[i].MakeGrayScaled();
            }

            else
            {
                OutputItems[i].MakeNotGrayScaled();
            }

            OutputItems[i].UpdateAmount(amount);
        }

        double maxWeight = GameManager.Instance.InventoryController.Inventory.maxWeight;
        double currentWeight = GameManager.Instance.InventoryController.Inventory.currentWeight;

        WeightText.text = "Inventory weight:\n" + currentWeight.ToString("0.0") + "/" + maxWeight.ToString("0.0");

        //Debug.Log("Updating list amounts");
    }

    private int CheckForPrecursors(int precursorAmount, int itemId)
    {
        // Oxygen bottle
        if (itemId == 13)
        {
            // Check for ice
            precursorAmount = InputItems[0].Amount;
        }

        // Water bottle
        else if (itemId == 31)
        {
            // Check for ice
            precursorAmount = InputItems[0].Amount;
        }

        // Carbon
        else if (itemId == 30)
        {
            // Check for silicates
            precursorAmount = InputItems[1].Amount;
        }

        // Rocket fuel
        else if (itemId == 15)
        {
            // Check for silicates
            precursorAmount = InputItems[1].Amount;
        }

        // Diamond
        else if (itemId == 6)
        {
            // Check for carbon
            precursorAmount = InputItems[2].Amount;
        }

        return precursorAmount;
    }

    // Update is called once per frame
    void Update()
    {
        //TimeUntilFlash -= Time.unscaledDeltaTime;
        
        //TimeUntilBecomesInteractable -= Time.unscaledDeltaTime;

        //if (!ConvertButtonsAreInteractable
        //    && TimeUntilBecomesInteractable <= 0)
        //{
        //    MakeButtonsInteractable();
        //}

        //if (TimeUntilFlash <= 0)
        //{

        //    if (TestText.gameObject.activeSelf)
        //    {
        //        TestText.gameObject.SetActive(false);
        //        TimeUntilFlash = 0.3f;
        //    }

        //    else
        //    {
        //        TestText.gameObject.SetActive(true);
        //        TimeUntilFlash = 1.0f;
        //    }
        //}

        if (resultTextTimer > 0.0f)
        {
            resultTextTimer -= Time.unscaledDeltaTime;

            if (resultTextTimer <= 0.0f)
            {
                ResultText.text = "";
            }
        }
    }

    public void OnCloseWindow()
    {
        GameManager.Instance.InventoryController.OnISRUHide();
    }

    private bool CheckIfCanInteractWithButtons()
    {
        bool canInteract;
        if (!ConvertButtonsAreInteractable)
        {
            canInteract = false;
            Debug.Log("Convert button is not interactable");
        }

        else
        {
            canInteract = true;
        }

        return canInteract;
    }



    public void OnConvertOnePressed()
    {
        bool canInteract = false;

        canInteract = CheckIfCanInteractWithButtons();

        if (canInteract)
        {
            Debug.Log("Should convert one");
            DoTheConversion(1);
        }
    }

    public void OnConvertTenPressed()
    {
        bool canInteract = false;

        canInteract = CheckIfCanInteractWithButtons();

        if (canInteract)
        {
            Debug.Log("Should convert ten");
            DoTheConversion(10);
        }
    }

    public void OnConvertHundredPressed()
    {
        bool canInteract = false;

        canInteract = CheckIfCanInteractWithButtons();

        if (canInteract)
        {
            Debug.Log("Should convert hundred");
            DoTheConversion(100);
        }
    }

    public void OnConvertXAmountPressed()
    {
        bool canInteract = false;

        canInteract = CheckIfCanInteractWithButtons();

        if (canInteract)
        {
            int amount = int.Parse(InputField.text);
            //Debug.Log("Should convert an x amount. Amount is " + int.Parse(InputField.text));
            DoTheConversion(amount);
        }
    }

    public void MakeButtonsUninteractable()
    {
        ConvertButtonsAreInteractable = false;

        ButtonOneText.color = GrayedOutTextColor;
        ButtonTenText.color = GrayedOutTextColor;
        ButtonHundredText.color = GrayedOutTextColor;
        ButtonXAmountText.color = GrayedOutTextColor;

        ButtonOneImage.color = GrayedOutButtonColor;
        ButtonTenImage.color = GrayedOutButtonColor;
        ButtonHundredImage.color = GrayedOutButtonColor;
        ButtonXAmountImage.color = GrayedOutButtonColor;

        //Debug.Log("Making buttons uninteractable");
    }

    public void MakeButtonsInteractable()
    {
        ConvertButtonsAreInteractable = true;

        ButtonOneText.color = InteractableTextColor;
        ButtonTenText.color = InteractableTextColor;
        ButtonHundredText.color = InteractableTextColor;
        ButtonXAmountText.color = InteractableTextColor;

        ButtonOneImage.color = InteractableButtonColor;
        ButtonTenImage.color = InteractableButtonColor;
        ButtonHundredImage.color = InteractableButtonColor;
        ButtonXAmountImage.color = InteractableButtonColor;
        //Debug.Log("Making buttons interactable");
    }

    public void OnPrecursorPressed(ISRUItemPlayer item)
    {
        if (SelectedPrecursor != null)
        {
            SelectedPrecursor.OnDeselected();
        }

        SelectedPrecursor = item;

        if (SelectedPrecursor.Amount <= 0)
        {
            SelectedPrecursor.OnInvalidConversionSelected();
        }

        else
        {
            SelectedPrecursor.OnSelected();
        }

        //Debug.Log("On precursor pressed. Item is " + item.Item.itemName);

        CheckIfConversionPrecursorAndTargetAreValid();
    }

    public void OnTargetPressed(ISRUItemTarget item)
    {
        if (SelectedTarget != null)
        {
            SelectedTarget.OnDeselected();
        }

        SelectedTarget = item;

        int precursorAmount = CheckForPrecursors(0, SelectedTarget.Item.id);

        if (precursorAmount > 0) 
        {
            SelectedTarget.OnSelected();
        }

        else
        {
            SelectedTarget.OnInvalidConversionSelected();
        }

        //Debug.Log("On target pressed. Item is " + item.Item.itemName);

        CheckIfConversionPrecursorAndTargetAreValid();
    }

    public void OnInputFieldValueChanged()
    {
        int parsed = 0;

        int.TryParse(InputField.text, out parsed);

        if (parsed < 0)
        {
            InputField.text = 0.ToString();
        }

        if (parsed <= 0)
        {
            MakeButtonsUninteractable();
        }

        else
        {
            CheckIfConversionPrecursorAndTargetAreValid();
        }
    }

    public void CheckIfConversionPrecursorAndTargetAreValid()
    {
        bool foundAValidConversion = false;
        string text = "";

        if (SelectedPrecursor == null
            || SelectedTarget == null)
        {
            // Just skip the if clauses.
        }

        else
        {
            // ICE
            if (SelectedPrecursor.Item.id == 27)
            {
                // OXYGEN BOTTLE
                if (SelectedTarget.Item.id == 13)
                {
                    text = "Convert 2 Ice to \n 1 Oxygen Bottle";
                    foundAValidConversion = true;
                }

                // WATER BOTTLE
                else if (SelectedTarget.Item.id == 31)
                {
                    text = "Convert 2 Ice to \n 1 Water Bottle";
                    foundAValidConversion = true;
                }

                // CARBON
                else if (SelectedTarget.Item.id == 30)
                {

                }

                // ROCKET FUEL
                else if (SelectedTarget.Item.id == 15)
                {

                }

                // DIAMOND
                else if (SelectedTarget.Item.id == 6)
                {

                }
            }

            // SILICATE
            else if (SelectedPrecursor.Item.id == 28)
            {
                // OXYGEN BOTTLE
                if (SelectedTarget.Item.id == 13)
                {

                }

                // WATER BOTTLE
                else if (SelectedTarget.Item.id == 31)
                {

                }

                // CARBON
                else if (SelectedTarget.Item.id == 30)
                {
                    text = "Convert 1 silicate to \n 1 Carbon";
                    foundAValidConversion = true;
                }

                // ROCKET FUEL
                else if (SelectedTarget.Item.id == 15)
                {
                    text = "Convert 2 Silicates to \n 1 unit of Rocket Fuel";
                    foundAValidConversion = true;
                }

                // DIAMOND
                else if (SelectedTarget.Item.id == 6)
                {

                }
            }

            // CARBON
            else if (SelectedPrecursor.Item.id == 30)
            {
                // OXYGEN BOTTLE
                if (SelectedTarget.Item.id == 13)
                {

                }

                // WATER BOTTLE
                else if (SelectedTarget.Item.id == 31)
                {

                }

                // CARBON
                else if (SelectedTarget.Item.id == 30)
                {

                }

                // ROCKET FUEL
                else if (SelectedTarget.Item.id == 15)
                {

                }

                // DIAMOND
                else if (SelectedTarget.Item.id == 6)
                {
                    text = "Convert 10 Carbon to \n 1 Diamond";
                    foundAValidConversion = true;
                }
            }

            if (SelectedPrecursor.Amount <= 0)
            {
                foundAValidConversion = false;
            }

            if (!foundAValidConversion)
            {
                text = "Can not convert";
                
                SelectedPrecursor.OnInvalidConversionSelected();
                SelectedTarget.OnInvalidConversionSelected();
            }

            if (SelectedPrecursor == null
                || SelectedTarget == null)
            {
                text = "";
                foundAValidConversion = false;
            }
        }

        if (SelectedPrecursor != null
            && SelectedPrecursor.Amount <= 0)
        {
            foundAValidConversion = false;
        }


        if (!foundAValidConversion)
        {
            MakeButtonsUninteractable();
            InfoText.color = Color.red;
        }

        else
        {
            InfoText.color = Color.white;
            SelectedPrecursor.OnSelected();
            SelectedTarget.OnSelected();
            MakeButtonsInteractable();
        }

        int parsed = 0;

        int.TryParse(InputField.text, out parsed);

        if (parsed < 0)
        {
            InputField.text = 0.ToString();
        }

        if (parsed <= 0)
        {
            MakeButtonsUninteractable();
        }

        InfoText.text = text;
    }

    private void UpdateInventoryAmounts(int precursorSubstractAmount)
    {
        GameManager.Instance.InventoryController.Inventory.RemoveItem(SelectedPrecursor.Item.id, 
                                                                      precursorSubstractAmount);

        if (ResourceInventory.Instance != null)
        {
            ResourceInventory.Instance.SetResourceAmounts(GameManager.Instance.InventoryController.Inventory);
        }
        
        UpdateLists();
        //Debug.Log("Updating inventory amounts");
    }

    private void DoTheConversion(int targetAmount)
    {
        bool foundAValidConversion = true;

        int precursorSubstractAmount = 0;
        int amountRequiredForOneUnit = 0;
        int amountOfSuccesfullConversion = 0;

        bool weHaveRoomForItem = true;


        if (SelectedPrecursor != null
            && SelectedTarget != null)
        {
            //Debug.LogWarning("We can try conversion.");
        }

        else
        {
            //Debug.LogError("No selected precursor or target. Don't convert");
            return;
        }

        // OXYGEN BOTTLE
        if (SelectedTarget.Item.id == 13)
        {
            // Required ice
            amountRequiredForOneUnit = 2;
            //text = "Convert 2 ice to 1 oxygen bottle";

            CalculateSubstractAndResultAmounts(targetAmount, 
                                               out precursorSubstractAmount, 
                                               amountRequiredForOneUnit, 
                                               out amountOfSuccesfullConversion);

            if (CheckIfWeHaveRoom(precursorSubstractAmount, amountOfSuccesfullConversion))
            {
                GameManager.Instance.InventoryController.Inventory.AddItem(13, amountOfSuccesfullConversion);
                foundAValidConversion = true;
            }

            else
            {
                weHaveRoomForItem = false;
            }

        }

        // WATER BOTTLE
        else if (SelectedTarget.Item.id == 31)
        {
            // Required ice
            amountRequiredForOneUnit = 2;

            CalculateSubstractAndResultAmounts(targetAmount,
                                               out precursorSubstractAmount,
                                               amountRequiredForOneUnit,
                                               out amountOfSuccesfullConversion);

            //text = "Convert 1 ice to 1 water bottle";

            if (CheckIfWeHaveRoom(precursorSubstractAmount, amountOfSuccesfullConversion))
            {
                GameManager.Instance.InventoryController.Inventory.AddItem(31, amountOfSuccesfullConversion);
                foundAValidConversion = true;
            }

            else
            {
                weHaveRoomForItem = false;
            }


        }

        // CARBON
        else if (SelectedTarget.Item.id == 30)
        {
            // Required silicates
            amountRequiredForOneUnit = 1;

            CalculateSubstractAndResultAmounts(targetAmount,
                                               out precursorSubstractAmount,
                                               amountRequiredForOneUnit,
                                               out amountOfSuccesfullConversion);

            if (CheckIfWeHaveRoom(precursorSubstractAmount, amountOfSuccesfullConversion))
            {
                GameManager.Instance.InventoryController.Inventory.AddItem(30, amountOfSuccesfullConversion);
                foundAValidConversion = true;
            }

            else
            {
                weHaveRoomForItem = false;
            }


        }

        // ROCKET FUEL
        else if (SelectedTarget.Item.id == 15)
        {
            // Required silicated
            amountRequiredForOneUnit = 2;

            CalculateSubstractAndResultAmounts(targetAmount,
                                               out precursorSubstractAmount,
                                               amountRequiredForOneUnit,
                                               out amountOfSuccesfullConversion);

            if (CheckIfWeHaveRoom(precursorSubstractAmount, amountOfSuccesfullConversion))
            {
                GameManager.Instance.InventoryController.Inventory.AddItem(15, amountOfSuccesfullConversion);
                foundAValidConversion = true;
            }

            else
            {
                weHaveRoomForItem = false;
            }


        }

        // DIAMOND
        else if (SelectedTarget.Item.id == 6)
        {
            // Required carbon
            amountRequiredForOneUnit = 10;

            CalculateSubstractAndResultAmounts(targetAmount,
                                               out precursorSubstractAmount,
                                               amountRequiredForOneUnit,
                                               out amountOfSuccesfullConversion);

            if (CheckIfWeHaveRoom(precursorSubstractAmount, amountOfSuccesfullConversion))
            {
                GameManager.Instance.InventoryController.Inventory.AddItem(6, amountOfSuccesfullConversion);
                foundAValidConversion = true;
            }

            else
            {
                weHaveRoomForItem = false;
            }
        }


        if (!foundAValidConversion)
        {
            Debug.LogError("Incorrect conversion, didn't do anything.");
        }

        if (weHaveRoomForItem) 
        {
            UpdateInventoryAmounts(precursorSubstractAmount);
        }

        else
        {
            ResultText.text = "No room for item in inventory"; // Add amounts too, so we know how much room we have
            ResultText.color = Color.red;
            resultTextTimer = 4.0f;
            Debug.LogError("Put an error text out there");
        }

        CheckIfConversionPrecursorAndTargetAreValid(); // Once more check this, so it's updated for the next round of conversions.
    }

    private bool CheckIfWeHaveRoom(int precursorSubstractAmount,
                                   int targetAddAmount)
    {
        if (SelectedPrecursor == null
            || SelectedTarget == null)
        {
            Debug.LogError("We don't have a valid precursor or target");
            return false;
        }

        double weightToAdd = (SelectedTarget.Item.weight * targetAddAmount) - (SelectedPrecursor.Item.weight * precursorSubstractAmount);

        if (GameManager.Instance.InventoryController.Inventory.CheckForRoomWithWeight(weightToAdd))
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    private void CalculateSubstractAndResultAmounts(int targetAmount, 
                                                    out int precursorSubstractAmount, 
                                                    int amountRequiredForOneUnit, 
                                                    out int amountOfSuccesfullConversion)
    {            

        if (targetAmount >= SelectedPrecursor.Amount / amountRequiredForOneUnit)
        {
            //Debug.Log("Was more");
            precursorSubstractAmount = SelectedPrecursor.Amount;
            
        }

        else
        {
            //Debug.Log("Wasn't more");
            precursorSubstractAmount = targetAmount * amountRequiredForOneUnit;
        }

        amountOfSuccesfullConversion = precursorSubstractAmount / amountRequiredForOneUnit;

        string precursorText;
        string targetText;

        if (precursorSubstractAmount > 1) 
        {
            precursorText = SelectedPrecursor.Item.plural;
        }

        else
        {
            precursorText = SelectedPrecursor.Item.itemName;
        }

        if (amountOfSuccesfullConversion > 1)
        {
            targetText = SelectedTarget.Item.plural;
        }

        else
        {
            targetText = SelectedTarget.Item.itemName;
        }

            
        ResultText.text = "Converted " + precursorSubstractAmount + " " + precursorText + " to " + amountOfSuccesfullConversion + " " + targetText;
        ResultText.color = Color.white;
        resultTextTimer = 4.0f;
        //Debug.Log("Precursor substract amount is " + precursorSubstractAmount);
        //Debug.Log("Amount of successfull conversion is " + amountOfSuccesfullConversion);
    }


}
