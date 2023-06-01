using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUI_Spaceship : MonoBehaviour
{
    [SerializeField]
    private InputActionAsset inputActionAsset;
    [SerializeField]
    private TMP_Text helpText;
    [SerializeField]
    Button restartButton;
    [SerializeField]
    Button quitButton;
    [SerializeField]
    TMP_Text promptText;
    [SerializeField]
    GameObject spaceshipHUD;
    [SerializeField]
    StatusBar healthBar;
    [SerializeField]
    StatusBar boostBar;
    [SerializeField]
    StatusBar missileBar;

    private bool prompTextWasActiveBeforeInventoryShow;
    private bool spaceshipHUDWasActiveBeforeInventoryShow;

    void Awake()
    {
        promptText.text = "";
        GameEvents.Instance.EventPlayerEnteredPromptTrigger.AddListener(OnPromptTriggerEnter);
        GameEvents.Instance.EventPlayerExitedPromptTrigger.AddListener(OnPromptTriggerExit);
        GameEvents.Instance.EventPlayerSpaceshipDied.AddListener(OnPlayerSpaceshipDeath);
        GameEvents.Instance.EventPlayerLanded.AddListener(OnPlayerLanded);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnPlayerLeftAsteroid);
        GameEvents.Instance.EventInventoryClosed.AddListener(OnInventoryClosed);
        GameEvents.Instance.EventInventoryOpened.AddListener(OnInventoryOpened);
        GameEvents.Instance.EventSpaceshipSpawned.AddListener(OnSpaceshipSpawned);
        //Debug.Log("Listener added to on leftasteroid" + Time.time);
    }

    private void Start()
    {
        ShowControls();
    }

    void OnPromptTriggerEnter(string text)
    {
        if (GameManager.Instance.InventoryController.ShowingInventory)
        {
            Debug.LogWarning("Returned, because invenotry is showing. Don't draw a prompt on top that");
            return;
        }

        promptText.gameObject.SetActive(true);

        if(text == null)
        {
            Debug.LogError("Null text passed to prompt trigger. How could this happen?");
            
            promptText.text = "ERROR. NULL TEXT. Find out why null string is passed";
        }

        else
        {
            promptText.text = text.Replace("%landKey%", Globals.Instance.landKey);
        }



    }

    void OnPromptTriggerExit()
    {
        promptText.gameObject.SetActive(false);
        promptText.text = "";
    }

    void OnPlayerLanded(MineableAsteroidTrigger asteroid)
    {
        promptText.gameObject.SetActive(false);
        promptText.text = "";

        spaceshipHUD.SetActive(false);
    }

    void OnPlayerLeftAsteroid(MineableAsteroidTrigger asteroid)
    {
        spaceshipHUD.SetActive(true);
    }

    void OnPlayerSpaceshipDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        restartButton.gameObject.SetActive(true);
        quitButton.gameObject.SetActive(true);
    }

    void OnSpaceshipSpawned(ActorSpaceship actor)
    {
        if(actor.faction.factionName == "Player")
        {
            InitShipHUD(actor);
        }
    }

    void InitShipHUD(ActorSpaceship actor)
    {
        healthBar.SetTrackable(actor.spaceshipHealth);
        boostBar.SetTrackable(actor.spaceshipBoost);
        missileBar.SetTrackable(actor.spaceshipMissile);
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnQuitClicked()
    {
        Application.Quit();
    }

    void ShowControls()
    {
        Dictionary<string, string> bindings = new Dictionary<string, string>();

        foreach (InputBinding inputBinding in inputActionAsset.bindings)
        {
            string inputBindingString = inputBinding.ToString();
            if (inputBindingString.Contains("/"))
            {
                string action = inputBindingString.Split("<")[0] + " ";
                string button = inputBindingString.Split("/")[1];
                if (bindings.ContainsKey(action))
                {
                    bindings[action] = bindings[action] + $", {button}";
                } else
                {
                    bindings[action] = button;
                }

                if (action.StartsWith("Land"))
                {
                    Globals.Instance.landKey = button;
                }
            }
        }

        string textToShow = "";

        int i = 0;

        foreach (KeyValuePair<string, string> bindData in bindings)
        {
            // NOTE: WE don't want to show first person controls right now.
            // A more robust solution would be fine here.
            if (i < 15)
            {
                textToShow += $"{bindData.Key}{bindData.Value}\n";
            }

            i++;

        }

        helpText.text = textToShow;
    }

    public void OnInventoryOpened()
    {
        if (spaceshipHUD.activeSelf)
        {
            spaceshipHUDWasActiveBeforeInventoryShow = true;
            spaceshipHUD.SetActive(false);
        }

        else
        {
            spaceshipHUDWasActiveBeforeInventoryShow = false;
        }

        if (promptText.gameObject.activeSelf)
        {
            promptText.gameObject.SetActive(false);
            prompTextWasActiveBeforeInventoryShow = true;
        }

        else
        {
            prompTextWasActiveBeforeInventoryShow = false;
        }

        Debug.Log("Hide everything");
    }

    public void OnInventoryClosed()
    {
        if (spaceshipHUDWasActiveBeforeInventoryShow)
        {
            spaceshipHUD.SetActive(true);
        }

        if (prompTextWasActiveBeforeInventoryShow)
        {
            promptText.gameObject.SetActive(true);
        }

        Debug.Log("Show everything");
    }

    //public void OnToggleCursor()
    //{
    //    if (Cursor.lockState == CursorLockMode.None)
    //    {
    //        Cursor.lockState = CursorLockMode.Locked;
    //    } else
    //    {
    //        Cursor.lockState = CursorLockMode.None;
    //    }
    //}
}
