using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldMapMessagePrompt : MonoBehaviour
{
    public TextMeshProUGUI MessagePromptText;

    public float MessageLength;

    public void Init()
    {
        MessagePromptText.text = "";
        MessageLength = -1;
        gameObject.SetActive(false);
    }

    public void DisplayMessage(string message,
                               float duration)
    {
        gameObject.transform.SetParent(Camera.main.transform);
        gameObject.SetActive(true);
        MessagePromptText.text = message;
        MessageLength = duration;
        Debug.LogError("WE should display a message on world map");
    }

    public void Update()
    {
        if (MessageLength > 0)
        {
            MessageLength -= Time.deltaTime;

            if (MessageLength <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
