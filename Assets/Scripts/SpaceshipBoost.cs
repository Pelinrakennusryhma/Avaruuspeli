using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceshipMovement))]
public class SpaceshipBoost : MonoBehaviour
{
    private SpaceshipMovement spaceshipMovement;

    [SerializeField]
    private float boostMultiplier = 20f;
    [SerializeField]
    private float maxBoostAmount = 2f;
    [SerializeField]
    private float boostDeprecationRate = 0.25f;
    [SerializeField]
    private float boostRechargeRate = 0.5f;
    [SerializeField]
    private GameObject[] boostEffects;

    public bool boosting = false;
    private float currentBoostAmount;

    void Start()
    {
        spaceshipMovement = GetComponent<SpaceshipMovement>();
        currentBoostAmount = maxBoostAmount;
    }

    void FixedUpdate()
    {
        HandleBoosting();
    }

    void HandleBoosting()
    {
        if (boosting && currentBoostAmount > 0f)
        {
            currentBoostAmount -= boostDeprecationRate;
            if (currentBoostAmount <= 0f)
            {
                boosting = false;
            }
        }
        else
        {
            if (currentBoostAmount < maxBoostAmount)
            {
                currentBoostAmount += boostRechargeRate;
            }
        }

        if (boosting)
        {
            spaceshipMovement.boostMultiplier = boostMultiplier;
        } else
        {
            spaceshipMovement.boostMultiplier = 1f;
        }

        ToggleEffects(boosting);
    }

    void ToggleEffects(bool active)
    {
        foreach (GameObject boostEffect in boostEffects)
        {
            boostEffect.SetActive(active);
        }
    }
}
