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


    void Update()
    {
        UpdateText();
        UpdateBar();
    }

    void UpdateText()
    {
        text.text = $"{trackedScript.CurrentValue} / {trackedScript.MaxValue}";
    }

    void UpdateBar()
    {
        float ratio = (float)trackedScript.CurrentValue / (float)trackedScript.MaxValue;
        float lerpedAmount = Mathf.Lerp(bar.fillAmount, ratio, Time.deltaTime * lerpSpeed);
        bar.fillAmount = lerpedAmount;
        bar.color = Color.Lerp(emptyColor, fullColor, ratio);
    }
}
