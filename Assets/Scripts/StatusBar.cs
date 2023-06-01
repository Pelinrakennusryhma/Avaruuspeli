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


    void Update()
    {
        if(trackedScript != null)
        {
            UpdateText();
            UpdateBar();
        }
    }

    public void SetTrackable(UITrackable tracked)
    {
        trackedScript = tracked;
    }

    void UpdateText()
    {
        text.text = $"{trackedScript.CurrentValue} / {trackedScript.MaxValue}";
    }

    void UpdateBar()
    {
        float ratio = (float)trackedScript.CurrentValue / (float)trackedScript.MaxValue;
        float lerpedAmount = Mathf.Lerp(bar.fillAmount, ratio, Time.deltaTime * lerpSpeed);
        bar.fillAmount = lerpedBar ? lerpedAmount: ratio;
        bar.color = Color.Lerp(emptyColor, fullColor, ratio);
    }
}
