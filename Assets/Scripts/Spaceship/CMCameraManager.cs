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
        GameEvents.Instance.EventPlayerLanded.AddListener(OnPlayerLanded);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnPlayerLeftAsteroid);
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
