using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDangerIndicator : MonoBehaviour
{
    [SerializeField]
    PlayerControls playerControls;
    [SerializeField]
    Image dangerImage;
    Color originalColor;
    float minAlpha = 0.02f;
    float maxAlpha = 0.30f;

    void Start()
    {
        if(playerControls == null)
        {
            playerControls = FindObjectOfType<PlayerControls>();
        }

        if(dangerImage == null)
        {
            dangerImage = GetComponent<Image>();
        }

        originalColor = dangerImage.color;
    }

    void Update()
    {
        if (playerControls.InDanger)
        {
            dangerImage.enabled = true;
            float newAlpha = Mathf.PingPong(Time.time, maxAlpha - minAlpha) + minAlpha;
            Color newColor = new Color(originalColor.r, originalColor.g, originalColor.b, newAlpha);

            dangerImage.color = newColor;
        } else
        {
            dangerImage.enabled = false;
        }
    }
}
