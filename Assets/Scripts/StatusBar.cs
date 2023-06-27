using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusBar : MonoBehaviour
{
    [SerializeField]
    UITrackable trackedScript;
    [SerializeField]
    Image bar;
    [SerializeField]
    TMP_Text text;
    [SerializeField]
    Color fullColor;
    [SerializeField]
    Color emptyColor;
    [SerializeField]
    float lerpSpeed = 10f;
    [SerializeField]
    bool lerpedBar = true;

    float currentValue = 0f;
    float maxValue = 0f;

    void Update()
    {
        if(maxValue <= 0f)
        {
            if(trackedScript != null)
            {
                maxValue = trackedScript.MaxValue;
            }
        }

        UpdateText();
        UpdateBar();
    }

    public void SetTrackable(UITrackable tracked)
    {
        trackedScript = tracked;
        currentValue = trackedScript.CurrentValue;
        maxValue = trackedScript.MaxValue;
    }

    void UpdateText()
    {
        text.text = $"{currentValue} / {maxValue}";
    }

    void UpdateBar()
    {
        currentValue = trackedScript != null ? trackedScript.CurrentValue : currentValue;
        float ratio = currentValue / maxValue;
        float lerpedAmount = Mathf.Lerp(bar.fillAmount, ratio, Time.deltaTime * lerpSpeed);
        bar.fillAmount = lerpedBar ? lerpedAmount: ratio;
        bar.color = Color.Lerp(emptyColor, fullColor, ratio);
    }
}
