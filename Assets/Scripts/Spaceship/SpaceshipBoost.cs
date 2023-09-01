using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpaceshipMovement))]
public class SpaceshipBoost : UITrackable
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

    private SpaceshipEvents spaceshipEvents;

    Rigidbody rb;

    // Audio
    private EventInstance boostSFX;

    public override float MaxValue
    {
        get
        {
            return (int)maxBoostAmount;
        }
    }

    public override float CurrentValue
    {
        get
        {
            return (int)currentBoostAmount;
        }
    }

    void Start()
    {
        spaceshipMovement = GetComponent<SpaceshipMovement>();
        currentBoostAmount = maxBoostAmount;

        rb = GetComponent<Rigidbody>();
        spaceshipEvents = GetComponent<SpaceshipEvents>();
        spaceshipEvents.EventSpaceshipDied.AddListener(OnSpaceshipDied);
        boostSFX = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.ShipBoost);
        RuntimeManager.AttachInstanceToGameObject(boostSFX, transform, rb);
    }

    void FixedUpdate()
    {
        HandleBoosting();
        UpdateSound();
    }

    void OnSpaceshipDied()
    {
        boostSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
            spaceshipMovement.speedMultiplier = boostMultiplier;
        } else
        {
            spaceshipMovement.speedMultiplier = 1f;
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

    void UpdateSound()
    {
        if (boosting)
        {
            PLAYBACK_STATE playbackState;
            boostSFX.getPlaybackState(out playbackState);
            RuntimeManager.AttachInstanceToGameObject(boostSFX, transform, rb);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                boostSFX.start();
            }
        }
        else
        {
            boostSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }
}
