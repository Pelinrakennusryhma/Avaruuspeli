using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceshipHealth))]
public class SpaceshipHealthEffects : MonoBehaviour
{
    [SerializeField]
    int maxHealthEffects = 2;
    [SerializeField]
    Transform healthEffectsParent;
    [SerializeField]
    List<GameObject> disabledEffects = new List<GameObject>();
    [SerializeField]
    List<GameObject> enabledEffects = new List<GameObject>();
    [SerializeField]
    List<float> effectThresholds = new List<float>();
    [SerializeField]
    int currentThreshold;

    SpaceshipHealth spaceshipHealth;

    void Awake()
    {
        spaceshipHealth = GetComponent<SpaceshipHealth>();
        spaceshipHealth.shipHealthChangedEvent.AddListener(OnShipHealthChanged);
        GetHealthEffects();
        CreateThresholds();
        CalculateThreshold();
    }

    void OnShipHealthChanged()
    {
        Debug.Log(GetHealthRatio());
        CalculateThreshold();
    }

    float GetHealthRatio()
    {
        return (float)spaceshipHealth.CurrentValue / spaceshipHealth.MaxValue;
    }

    void GetHealthEffects()
    {
        for (int i = 0; i < healthEffectsParent.childCount; i++)
        {
            disabledEffects.Add(healthEffectsParent.GetChild(i).gameObject);
        }

        // clamp the max health effects to match the actual effect gameObjects
        if(maxHealthEffects > disabledEffects.Count)
        {
            maxHealthEffects = disabledEffects.Count;
        }
    }

    void CreateThresholds()
    {
        float firstThreshold = (float)spaceshipHealth.MaxValue / (maxHealthEffects + 1) / spaceshipHealth.MaxValue;

        for (int i = 1; i <= maxHealthEffects; i++)
        {
            effectThresholds.Add(firstThreshold * i);
        }

        currentThreshold = effectThresholds.Count;
    }

    void CalculateThreshold()
    {
        float healthRatio = GetHealthRatio();
        int newThreshold = effectThresholds.Count;
        // Above highest effect threshold, no effects added
        if (healthRatio > effectThresholds[effectThresholds.Count - 1])
        {
            newThreshold = effectThresholds.Count;
        } else
        {
            for (int i = 0; i < effectThresholds.Count; i++)
            {
                if (healthRatio < effectThresholds[i])
                {
                    newThreshold = i;
                    break;
                }
            }
        }
        Debug.Log("newThreshold: " + newThreshold);
        SetEffects(newThreshold);
    }

    void SetEffects(int newThreshold)
    {
        Debug.Log(newThreshold + " " + currentThreshold);
        if(newThreshold > currentThreshold)
        {
            int difference = newThreshold - currentThreshold;

            currentThreshold = newThreshold;

            for (int i = 0; i < difference; i++)
            {
                DisableEffect();
            }

        } else if(newThreshold < currentThreshold)
        {
            int difference = currentThreshold - newThreshold;

            currentThreshold = newThreshold;

            for (int i = 0; i < difference; i++)
            {
                EnableEffect();
            }

        }
    }

    void EnableEffect()
    {
        GameObject effect = disabledEffects[Random.Range(0, disabledEffects.Count)];
        disabledEffects.Remove(effect);
        enabledEffects.Add(effect);
        effect.SetActive(true);
    }

    void DisableEffect()
    {
        GameObject effect = enabledEffects[Random.Range(0, enabledEffects.Count)];
        enabledEffects.Remove(effect);
        disabledEffects.Add(effect);
        effect.SetActive(false);
    }
}
