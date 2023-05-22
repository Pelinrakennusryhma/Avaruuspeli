using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    FMODBus bus;
    [SerializeField]
    Slider slider;

    private void Start()
    {
        slider.onValueChanged.AddListener(OnValueChanged);

        // load values from save?
        float startValue = 1f;
        OnValueChanged(startValue);
    }

    private void OnValueChanged(float value)
    {
        AudioManager.Instance.SetBusValue(bus, value);
    }
}
