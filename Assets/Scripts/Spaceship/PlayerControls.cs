using FMOD.Studio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : ActorSpaceship
{
    [SerializeField]
    ButtonHeldAction leaveSpaceshipSceneHandler;

    bool dead = false;

    // Audio
    private EventInstance alarmSFX;

    protected override void Start()
    {
        base.Start();
        alarmSFX = AudioManager.Instance.CreateEventInstance(FMODEvents.Instance.Alarm);
        ship.tag = "PlayerShip";
    }

    private void Update()
    {
        UpdateSounds();
    }

    private void LateUpdate()
    {
        if (dead)
        {
            alarmSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            Destroy(gameObject);
        }
    }

    void UpdateSounds()
    {
        if (lockedMissiles.Count > 0)
        {
            PLAYBACK_STATE playbackState;
            alarmSFX.getPlaybackState(out playbackState);
            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {
                alarmSFX.start();
            }
        }
        else
        {
            alarmSFX.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        }
    }

    protected override void OnDeath()
    {
        base.OnDeath();
        Cursor.visible = true;
        dead = true;   
    }

    public void OnThrust(InputAction.CallbackContext context)
    {
        spaceshipMovement.thrust1D = context.ReadValue<float>();
    }

    public void OnStrafe(InputAction.CallbackContext context)
    {
        spaceshipMovement.strafe1D = context.ReadValue<float>();
    }

    public void OnUpDown(InputAction.CallbackContext context)
    {
        spaceshipMovement.upDown1D = context.ReadValue<float>();
    }

    public void OnRoll(InputAction.CallbackContext context)
    {
        spaceshipMovement.roll1D = context.ReadValue<float>();
    }

    public void OnPitchYaw(InputAction.CallbackContext context)
    {
        spaceshipMovement.pitchYaw = context.ReadValue<Vector2>();
    }

    public void OnBoost(InputAction.CallbackContext context)
    {
        spaceshipBoost.boosting = context.performed;
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        spaceshipShoot.shooting = context.performed;
    }

    public void OnLand(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameEvents.Instance.CallEventPlayerTriedLanding();
        }
    }

    public void OnShowIndicators(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameEvents.Instance.CallEventToggleIndicators(true);
        } else if (context.canceled)
        {
            GameEvents.Instance.CallEventToggleIndicators(false);
        }
    }

    public void OnOptions(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.OnOptionsPressed();
        }

    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameManager.Instance.OnInventoryPressed();
            //Debug.Log("On inventory pressed");
        }


        //GameManager.Instance.OnInventoryPressed();
        //Debug.Log("On inventory pressed");

    }

    public void OnLeaveScene(InputAction.CallbackContext context)
    {
        if(leaveSpaceshipSceneHandler == null)
        {
            leaveSpaceshipSceneHandler = FindObjectOfType<ButtonHeldAction>();
        }

        if (context.started)
        {
            leaveSpaceshipSceneHandler.OnButtonPressed();
        } else if (context.canceled)
        {
            leaveSpaceshipSceneHandler.OnButtonReleased();
        }
    }

    public void OnSecondaryShoot(InputAction.CallbackContext context)
    {
        spaceshipMissile.shooting = context.performed;
    }

    protected override void InitUtilities()
    {
        base.InitUtilities();
        GameEvents.Instance.CallEventPlayerUtilitiesInited(shipUtilityScripts);
    }

    public void OnUtility1(InputAction.CallbackContext context)
    {
        if(shipUtilityScripts[0] != null)
        {
            shipUtilityScripts[0].TryingToActivate = context.performed;
        }

    }
    public void OnUtility2(InputAction.CallbackContext context)
    {
        if (shipUtilityScripts.Count > 1 && shipUtilityScripts[1] != null)
        {
            shipUtilityScripts[1].TryingToActivate = context.performed;
        }
    }
}
