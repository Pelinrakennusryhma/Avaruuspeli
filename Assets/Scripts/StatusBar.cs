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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
        bar.fillAmount = (float)trackedScript.CurrentValue / (float)trackedScript.MaxValue;
    }
}
