using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] vCams;
    [SerializeField]
    private int currentCameraID = 0;
    private int cachedShipCamID;
    [SerializeField]
    CinemachineVirtualCamera behindVCam;
    [SerializeField]
    CinemachineVirtualCamera cockpitVCam;

    void Awake()
    {
        GameEvents.Instance.EventPlayerLanded.AddListener(OnPlayerLanded);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnPlayerLeftAsteroid);
        GameEvents.Instance.EventSpaceshipSpawned.AddListener(OnSpaceshipSpawned);
        //Debug.Log("Listener added to on leftasteroid" + Time.time);
    }
    void Start()
    {
        SetActiveCamera();
    }

    void OnPlayerLanded(MineableAsteroidTrigger asteroid)
    {
        cachedShipCamID = currentCameraID;
        currentCameraID = 2;
        SetActiveCamera();
    }

    void OnPlayerLeftAsteroid(MineableAsteroidTrigger asteroid)
    {
        currentCameraID = cachedShipCamID;
        SetActiveCamera();
    }

    void OnSpaceshipSpawned(ActorSpaceship actor)
    {
        if(actor.faction.factionName == "Player")
        {
            Spaceship playerShip = actor.ship.GetComponent<Spaceship>();
            Debug.Log("shipname: " + playerShip.gameObject.name);
            SetCamTargets(playerShip);
        }
    }

    void SetCamTargets(Spaceship playerShip)
    {
        behindVCam.Follow = playerShip.BehindCamera;
        behindVCam.LookAt = playerShip.BehindCamera;
        cockpitVCam.Follow = playerShip.CockpitCamera;
    }

    public void OnChangeCamera()
    {
        currentCameraID++;
        if(currentCameraID > 1)
        {
            currentCameraID = 0;
        }
        SetActiveCamera();
    }

    void SetActiveCamera()
    {
        for (int i = 0; i < vCams.Length; i++)
        {
            if(i == currentCameraID)
            {
                vCams[i].SetActive(true);
            } else
            {
                vCams[i].SetActive(false);
            }
        }
        GameEvents.Instance.CallEventCameraChanged(currentCameraID);
    }
}
