using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(SpaceshipHealth))]
public class SpaceshipHealthEffects : MonoBehaviour
{
    [SerializeField]
    int maxHealthEffects = 2;
    [SerializeField]
    Transform healthEffectsParent;
    [SerializeField]
    List<VisualEffect> disabledEffects = new List<VisualEffect>();
    [SerializeField]
    List<VisualEffect> enabledEffects = new List<VisualEffect>();
    [SerializeField]
    List<float> effectThresholds = new List<float>();
    [SerializeField]
    int currentThreshold;
    [SerializeField]
    GameObject explosionEffectPrefab;

    Color originalEffectColor;

    SpaceshipHealth spaceshipHealth;
    SpaceshipEvents spaceshipEvents;

    void Awake()
    {
        spaceshipHealth = GetComponent<SpaceshipHealth>();
        spaceshipEvents = GetComponent<SpaceshipEvents>();
        spaceshipEvents.EventSpaceshipHealthChanged.AddListener(OnShipHealthChanged);
        spaceshipEvents.EventSpaceshipDied.AddListener(OnDeath);
        GetHealthEffects();
        CreateThresholds();
        CalculateThreshold();
    }

    void OnShipHealthChanged()
    {
        if(spaceshipHealth.CurrentValue > 0)
        {
            CalculateThreshold();
        }
    }

    void OnDeath()
    {
        Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
    }

    float GetHealthRatio()
    {
        return (float)spaceshipHealth.CurrentValue / spaceshipHealth.MaxValue;
    }

    void GetHealthEffects()
    {
        for (int i = 0; i < healthEffectsParent.childCount; i++)
        {
            disabledEffects.Add(healthEffectsParent.GetChild(i).GetComponent<VisualEffect>());
        }

        // clamp the max health effects to match the actual effect gameObjects
        if(maxHealthEffects > disabledEffects.Count)
        {
            maxHealthEffects = disabledEffects.Count;
        }

        // original effect color from the first effect
        originalEffectColor = disabledEffects[0].GetVector4("Color");
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
        VisualEffect effect = disabledEffects[Random.Range(0, disabledEffects.Count)];
        disabledEffects.Remove(effect);
        enabledEffects.Add(effect);
        DarkenColor(effect);
        effect.gameObject.SetActive(true);
    }

    void DisableEffect()
    {
        VisualEffect effect = enabledEffects[Random.Range(0, enabledEffects.Count)];
        enabledEffects.Remove(effect);
        disabledEffects.Add(effect);
        ResetColor(effect);
        effect.gameObject.SetActive(false);
    }

    void DarkenColor(VisualEffect effect)
    {
        float darkenAmount = GetHealthRatio();
        Color darkenedColor = Color.Lerp(Color.black, originalEffectColor, darkenAmount);

        effect.SetVector4("Color", darkenedColor);
    }

    void ResetColor(VisualEffect effect)
    {
        effect.SetVector4("Color", originalEffectColor);
    }
}
