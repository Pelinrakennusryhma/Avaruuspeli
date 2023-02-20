using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipOnWorldMapController : MonoBehaviour
{
    public Vector3 CurrentUniversePos;
    public Vector3 CurrentGalaxyPos;
    public Vector3 CurrentStarSystemPos;

    public static MotherShipOnWorldMapController Instance;

    public WorldMapClickDetector CurrentTargetClickableObject;

    public bool IsOnCurrentClickableObject;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("We got two instances of mothership. This is not good and should be fixed probably");
        }
        Instance = this;
    }

    public void OnTriggered(Collider other)
    {
        Debug.LogError("Other is " + other.gameObject.name);
    }

    public void OnZoom(WorldMapMouseController.ZoomLevel newZoomLevel,
                       Vector3 originPos)
    {
        float localPosY = 0;

        switch (newZoomLevel)
        {
            case WorldMapMouseController.ZoomLevel.None:
                break;
            case WorldMapMouseController.ZoomLevel.Universe:
                transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
                localPosY = 0f;
                Debug.LogWarning("Set Ship to universe scale");
                break;
            case WorldMapMouseController.ZoomLevel.Galaxy:
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                localPosY = 0f;
                Debug.LogWarning("Set Ship to galaxy scale");
                break;
            case WorldMapMouseController.ZoomLevel.StarSystem:
                transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                localPosY = 0.05f;
                Debug.LogWarning("Set Ship to star system scale");
                break;
            default:
                break;
        }

        transform.position = new Vector3(originPos.x, localPosY, originPos.z);

        //Debug.LogWarning("Ship should react to zoom level");
    }

    public void SetPosOnUniverse(Vector3 universePos)
    {
        CurrentUniversePos = universePos;
    }

    public void SetPosOnCurrentGalaxy(Vector3 currentGalaxyPos)
    {
        CurrentGalaxyPos = currentGalaxyPos;
        //transform.position = new Vector3(currentGalaxy.x, 0, currentGalaxy.z);
    }

    public void SetPosOnCurrentStarSystem(Vector3 currentStarPos)
    {
        CurrentStarSystemPos = currentStarPos;
        //transform.position = new Vector3(currentStar.x, 0, currentStar.z);
    }

    public void MoveToUniversePos()
    {
        transform.position = new Vector3(CurrentUniversePos.x, 0, CurrentUniversePos.z);
    }

    public void MoveToGalaxyPos()
    {
        transform.position = new Vector3(CurrentGalaxyPos.x, 0, CurrentGalaxyPos.z);
    }

    public void MoveToStarSystemPos()
    {
        transform.position = new Vector3(CurrentStarSystemPos.x, 0, CurrentStarSystemPos.z);
    }

    public void SetCurrentTargetClickableObject(WorldMapClickDetector clickableObject)
    {
        CurrentTargetClickableObject = clickableObject;
    }

    public void Update()
    {
        if (CurrentTargetClickableObject != null)
        {
            Vector3 toDestinationNormalized = CurrentTargetClickableObject.transform.position -transform.position;

            toDestinationNormalized = new Vector3(toDestinationNormalized.x, 0, toDestinationNormalized.z);

            Vector3 targetPos = Vector3.Lerp(transform.position, CurrentTargetClickableObject.transform.position, Time.deltaTime * 5.0f);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);

            Vector3 toTarget2D = CurrentTargetClickableObject.transform.position - transform.position;
            toTarget2D = new Vector3(toTarget2D.x, 0, toTarget2D.z);

            if ((toTarget2D.magnitude <= 1.0f
                && WorldMapMouseController.Instance.CurrentZoomLevel
                == WorldMapMouseController.ZoomLevel.Universe)
                ||
                (toTarget2D.magnitude <= 0.2f
                && WorldMapMouseController.Instance.CurrentZoomLevel
                == WorldMapMouseController.ZoomLevel.Galaxy)
                ||
                (toTarget2D.magnitude <= 0.1f
                && WorldMapMouseController.Instance.CurrentZoomLevel
                == WorldMapMouseController.ZoomLevel.StarSystem))
            {
                IsOnCurrentClickableObject = true;
            }

            else
            {
                if (toDestinationNormalized != Vector3.zero)
                {
                    transform.rotation = Quaternion.LookRotation(-toDestinationNormalized, Vector3.up);
                }
            }

        }

        else
        {
            IsOnCurrentClickableObject = false;
        }

        if (WorldMapMouseController.Instance.CurrentZoomLevel == WorldMapMouseController.ZoomLevel.StarSystem)
        {
            transform.position = new Vector3(transform.position.x, 0.05f, transform.position.z);
        }
    }
}
