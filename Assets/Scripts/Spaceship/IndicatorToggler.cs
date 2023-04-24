using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Target))]
public class IndicatorToggler : MonoBehaviour
{
    [SerializeField]
    Target targetScript;

    void Awake()
    {
        if (targetScript == null)
        {
            targetScript = GetComponent<Target>();
        }

        GameEvents.Instance.EventToggleIndicators.AddListener(OnToggleIndicators);
    }

    void OnToggleIndicators(bool showIndicator)
    {
        targetScript.enabled = showIndicator;
    }
}
