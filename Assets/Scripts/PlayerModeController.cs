using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerModeController : MonoBehaviour
{
    [SerializeField]
    float landShipDistance = 5f;
    [SerializeField]
    PlayerControls playerControls;
    [SerializeField]
    GameObject firstPersonControls;
    [SerializeField] private FirstPersonPlayerControllerWithCentreOfGravity fpGravity;

    PlayerInput playerInput;
    void Awake()
    {
        GameEvents.Instance.EventPlayerLanded.AddListener(OnLand);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
        //Debug.Log("Listener added to on leftasteroid" + Time.time);
    }

    private void Start()
    {
        playerInput = playerControls.GetComponent<PlayerInput>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void OnLand(MineableAsteroidTrigger asteroid)
    {
        Transform ship = playerControls.ship.transform;     
        LandShipOnAsteroid(ship, asteroid);
        asteroid.shipPosition.position = ship.position;

        playerControls.spaceshipMovement.Freeze();
        playerInput.actions.FindActionMap("ShipControls").Disable();

        fpGravity.CenterOfGravity = asteroid.CenterOfGravity;
        Vector3 posUnderShip = ship.position + (-ship.up * 1.5f);
        firstPersonControls.transform.position = posUnderShip;
        fpGravity.LookAt(asteroid.transform.position, ship);
        //Debug.Log("rot: " + firstPersonControls.transform.rotation);
        //firstPersonControls.transform.LookAt(asteroid.transform.position, firstPersonControls.transform.up);
        //Debug.Log("rot2: " + firstPersonControls.transform.rotation);
        //firstPersonControls.transform.SetPositionAndRotation(asteroid.CharacterPosition.position, asteroid.CharacterPosition.rotation);
        firstPersonControls.SetActive(true);
        playerInput.actions.FindActionMap("FirstPersonControls").Enable();

        GameManager.Instance.LifeSupportSystem.OnEnterUnbreathablePlace();
    }

    void LandShipOnAsteroid(Transform ship, MineableAsteroidTrigger asteroid)
    {
        Vector3 closestPoint = asteroid.Collider.ClosestPoint(ship.position);
        Vector3 dirNormal = (ship.position - closestPoint).normalized;
        Vector3 landPos = closestPoint + (dirNormal * landShipDistance);
        ship.position = landPos;
        ship.up = dirNormal;
    }

    void OnLeaveAsteroid(MineableAsteroidTrigger asteroid)
    {
        fpGravity.CenterOfGravity = null;
        firstPersonControls.SetActive(false);
        playerControls.spaceshipMovement.UnFreeze();
        playerInput.actions.FindActionMap("FirstPersonControls").Disable();
        playerInput.actions.FindActionMap("ShipControls").Enable();

        //Debug.Log("On leave asteroid called" + Time.time +" asteroid is "+ asteroid.gameObject.name + " gameobject is " + gameObject.name);
        //GameManager.Instance.LifeSupportSystem.OnExitUnbreathablePlace();
    }
}
