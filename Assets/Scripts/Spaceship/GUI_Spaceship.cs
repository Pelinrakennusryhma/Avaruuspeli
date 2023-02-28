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
    TMP_Text promptText;
    public string landKey = "";
    void Start()
    {
        promptText.text = "";
        GameEvents.instance.EventPlayerEnteredPromptTrigger.AddListener(OnPromptTriggerEnter);
        GameEvents.instance.EventPlayerExitedPromptTrigger.AddListener(OnPromptTriggerExit);
        GameEvents.instance.EventPlayerSpaceshipDied.AddListener(OnPlayerSpaceshipDeath);
        GameEvents.instance.EventPlayerLanded.AddListener(OnPlayerLanded);
        ShowControls();
    }

    void OnPromptTriggerEnter(string text)
    {
        promptText.gameObject.SetActive(true);
        promptText.text = text.Replace("%landKey%", landKey);
    }

    void OnPromptTriggerExit()
    {
        promptText.gameObject.SetActive(false);
        promptText.text = "";
    }

    void OnPlayerLanded()
    {
        promptText.gameObject.SetActive(false);
        promptText.text = "";
    }

    void OnPlayerSpaceshipDeath()
    {
        Cursor.lockState = CursorLockMode.None;
        restartButton.gameObject.SetActive(true);
    }

    public void OnRestartClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
                    landKey = button;
                }
            }
        }

        string textToShow = "";

        int i = 0;

        foreach (KeyValuePair<string, string> bindData in bindings)
        {
            // NOTE: WE don't want to show first person controls right now.
            // A more robust solution would be fine here.
            if (i < 9)
            {
                textToShow += $"{bindData.Key}{bindData.Value}\n";
            }

            i++;

        }

        helpText.text = textToShow;
    }

    public void OnToggleCursor()
    {
        if (Cursor.lockState == CursorLockMode.None)
        {
            Cursor.lockState = CursorLockMode.Locked;
        } else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
