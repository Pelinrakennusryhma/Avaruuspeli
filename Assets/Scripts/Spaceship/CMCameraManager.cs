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

    void Awake()
    {
        GameEvents.instance.EventPlayerLanded.AddListener(OnPlayerLanded);
        GameEvents.instance.EventPlayerLeftAsteroid.AddListener(OnPlayerLeftAsteroid);
    }
    void Start()
    {
        SetActiveCamera();
    }

    void OnPlayerLanded()
    {
        cachedShipCamID = currentCameraID;
        currentCameraID = 2;
        SetActiveCamera();
    }

    void OnPlayerLeftAsteroid()
    {
        currentCameraID = cachedShipCamID;
        SetActiveCamera();
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
    }
}
