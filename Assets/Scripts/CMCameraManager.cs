using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMCameraManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] vCams;
    [SerializeField]
    private int currentCameraID = 0;

    void Start()
    {
        SetActiveCamera();
    }

    public void OnChangeCamera()
    {
        currentCameraID++;
        if(currentCameraID >= vCams.Length)
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
