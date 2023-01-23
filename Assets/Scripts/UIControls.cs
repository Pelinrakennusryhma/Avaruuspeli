using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System.Linq;

public class UIControls : MonoBehaviour
{
    public InputActionAsset inputActionAsset;
    public TMP_Text helpText;
    void Start()
    {
        PrintControls();
    }

    void PrintControls()
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
}
