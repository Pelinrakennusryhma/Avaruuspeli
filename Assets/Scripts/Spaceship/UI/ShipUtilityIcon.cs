using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUtilityIcon : MonoBehaviour
{
    [SerializeField]
    TMP_Text timerText;
    [SerializeField]
    Image timerImage;
    Useable _util;
    [SerializeField]
    Color cooldownFillColor;
    [SerializeField]
    Color useFillColor;
    [SerializeField]
    TMP_Text nameText;
    [SerializeField]
    Image icon;

    public void Init(Useable util, int utilIndex)
    {
        _util = util;

        nameText.text = $"{util.Data.itemName} ({utilIndex})";

        icon.sprite = util.Data.itemIcon;
    }
    void Update()
    {
        if (_util.Active)
        {
            timerText.gameObject.SetActive(false);
            timerImage.gameObject.SetActive(true);
            timerImage.fillAmount = GetDurationFill();
            timerImage.color = useFillColor;
        } 
        else if (_util.CooldownTimer < _util.Cooldown)
        {
            timerText.gameObject.SetActive(true);
            timerImage.gameObject.SetActive(true);
            timerText.text = GetRemainingCooldown();
            timerImage.fillAmount = GetCooldownFill();
            timerImage.color = cooldownFillColor;
        } 
        else
        {
            timerText.gameObject.SetActive(false);
            timerImage.gameObject.SetActive(false);
        }
    }

    string GetRemainingCooldown()
    {
        return (_util.Cooldown - _util.CooldownTimer).ToString("F1");
    }

    float GetCooldownFill()
    {
        return 1 - _util.CooldownTimer / _util.Cooldown;
    }

    float GetDurationFill()
    {
        return _util.DurationTimer / _util.Duration;
    }
}