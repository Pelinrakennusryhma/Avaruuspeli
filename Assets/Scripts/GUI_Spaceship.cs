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
    void Start()
    {
        GameEvents.instance.EventPlayerSpaceshipDied.AddListener(OnPlayerSpaceshipDeath);
        ShowControls();
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
            }
        }

        string textToShow = "";

        foreach (KeyValuePair<string, string> bindData in bindings)
        {
            textToShow += $"{bindData.Key}{bindData.Value}\n";
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
