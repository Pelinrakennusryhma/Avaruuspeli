using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotherShipOnWorldMapController : MonoBehaviour
{
    public Vector3 CurrentUniversePos;
    public Vector3 CurrentGalaxyPos;
    public Vector3 CurrentStarSystemPos;

    public Vector3 CurrentAsteroidFieldPos;

    public static MotherShipOnWorldMapController Instance;

    public WorldMapClickDetector CurrentTargetClickableObject;

    public bool IsOnCurrentClickableObject;

    public MotherShipFuelSystem FuelSystem;

    public void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("We got two instances of mothership. This is not good and should be fixed probably");
        }
        Instance = this;
        FuelSystem = GetComponent<MotherShipFuelSystem>();
        SetMaterialsToAlwaysRenderOnTop();
    }

    public void Start()
    {
        WorldMapCamera.Instance.SetToUniverseOffset(transform.position);
    }

    public void OnTriggered(Collider other)
    {
        //Debug.LogError("Other is " + other.gameObject.name);
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
                WorldMapCamera.Instance.SetToUniverseOffset(transform.position);
                localPosY = 0f;
                //Debug.LogWarning("Set Ship to universe scale");

                break;
            case WorldMapMouseController.ZoomLevel.Galaxy:
                transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
                WorldMapCamera.Instance.SetToGalaxyOffset(transform.position);
                localPosY = 0f;
                //Debug.LogWarning("Set Ship to galaxy scale");
                break;
            case WorldMapMouseController.ZoomLevel.StarSystem:
                transform.localScale = new Vector3(0.015f, 0.015f, 0.015f);
                WorldMapCamera.Instance.SetToStarSystemOffset(transform.position);
                localPosY = 0.05f;
                //Debug.LogWarning("Set Ship to star system scale");
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
        //Debug.Log("Setting the position on universe");
    }

    public void SetPosOnCurrentGalaxy(Vector3 currentGalaxyPos)
    {
        CurrentGalaxyPos = currentGalaxyPos;
        //transform.position = new Vector3(currentGalaxy.x, 0, currentGalaxy.z);
    }

    public void SetPosOnCurrentStarSystem(Vector3 currentStarPos)
    {
        CurrentStarSystemPos = currentStarPos;

        //Debug.Log("Setting the position on current star system");
        //transform.position = new Vector3(currentStar.x, 0, currentStar.z);
    }

    public void SetPosOnAsteroidField(Vector3 targetAsteroidFieldPos)
    {
        CurrentAsteroidFieldPos = targetAsteroidFieldPos;
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

    public void MoveToCurrentAsteroidFieldPos()
    {
        transform.position = new Vector3(CurrentAsteroidFieldPos.x, 0, CurrentAsteroidFieldPos.z);
    }

    public void SetCurrentTargetClickableObject(WorldMapClickDetector clickableObject)
    {
        CurrentTargetClickableObject = clickableObject;
        IsOnCurrentClickableObject = false;
        
        if (clickableObject != null) 
        {
            //Debug.Log("Setting the current target clickable object " + clickableObject.gameObject.name);
        }
    }

    public void SetCurrentTargetClickableObjectAndPosOnAsteroidField(WorldMapClickDetector clickableObject,
                                                                     Vector3 pos)
    {
        CurrentTargetClickableObject = clickableObject;
        CurrentAsteroidFieldPos = new Vector3(pos.x, 0, pos.z);
        //IsOnCurrentClickableObject = false;

        if (clickableObject != null)
        {
            //Debug.Log("Setting the current target clickable object " + clickableObject.gameObject.name);
        }
        //Debug.LogError("Set the pos for asteroid field " + Time.time);

    }

    public void Update()
    {
        if (CurrentTargetClickableObject != null)
        {
            Vector3 destination = CurrentTargetClickableObject.transform.position;
            
            if (CurrentTargetClickableObject.type == WorldMapClickDetector.ClickableObjectType.AsteroidField)
            {
                destination = CurrentAsteroidFieldPos;
                //Debug.Log("DESTINATION IS ASTEROID FIELD " + Time.time);
            }

            Vector3 toDestinationNormalized = destination -transform.position;

            toDestinationNormalized = new Vector3(toDestinationNormalized.x, 0, toDestinationNormalized.z);

            Vector3 targetPos = Vector3.Lerp(transform.position, destination, Time.deltaTime * 5.0f);
            transform.position = new Vector3(targetPos.x, transform.position.y, targetPos.z);

            Vector3 toTarget2D = destination - transform.position;
            toTarget2D = new Vector3(toTarget2D.x, 0, toTarget2D.z);


            if ((toTarget2D.magnitude <= 30.0f
                && WorldMapMouseController.Instance.CurrentZoomLevel
                == WorldMapMouseController.ZoomLevel.Universe)
                ||
                (toTarget2D.magnitude <= 0.6f
                && WorldMapMouseController.Instance.CurrentZoomLevel
                == WorldMapMouseController.ZoomLevel.Galaxy)
                ||
                (toTarget2D.magnitude <= 0.4f
                && WorldMapMouseController.Instance.CurrentZoomLevel
                == WorldMapMouseController.ZoomLevel.StarSystem))
            {
                IsOnCurrentClickableObject = true;
                //Debug.Log("IS ON CURRENT CLICKABLE OBJECT");
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

    public bool CheckIfAsteroidFieldPositionIsWithinTolerance()
    {
        if (GameManager.Instance.Helpers.CheckIfUIisHit())
        {
            Debug.LogError("UI is hit, not withing tolerance");
        }

        Vector3 yZeroedPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 yZeroedAsteroidFieldPos = new Vector3(CurrentAsteroidFieldPos.x, 0, CurrentAsteroidFieldPos.z);
        
        if ((yZeroedPos - yZeroedAsteroidFieldPos).magnitude <= 0.1f)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public bool CheckIfPlanetOrStarPositionIsWithinTolerance()
    {
        Vector3 yZeroedPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 yZeroedAsteroidFieldPos = new Vector3(CurrentStarSystemPos.x, 0, CurrentStarSystemPos.z);

        if ((yZeroedPos - yZeroedAsteroidFieldPos).magnitude <= 0.03f)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    public void TeleportFromWormhole(WormholeOnWorldMap wormhole)
    {
        //Debug.LogWarning("Should teleport from wormhole right about now. Start galaxy is " + wormhole.WormholeData.GalaxyId + " end galaxy is " + wormhole.WormholeData.PairWormholeGalaxyID);

        for (int i = 0; i < UniverseController.Instance.AllGalaxies.Length; i++)
        {
            if (UniverseController.Instance.AllGalaxies[i].GalaxyData.ID == wormhole.WormholeData.PairWormholeGalaxyID)
            {
                //Debug.LogError("We found the correct galaxy to teleport to. Implement logic for actually doing the teleportation");

                // Set positions and camera and all to new pos
                // Maybe an animation would be nice here?

                WorldMapMouseController.Instance.ZoomOut();

                GameManager.Instance.CurrentGalaxy = UniverseController.Instance.AllGalaxies[i];

                SetPosOnUniverse(UniverseController.Instance.AllGalaxies[i].transform.position);
                Instance.SetPosOnCurrentGalaxy(UniverseController.Instance.AllGalaxies[i].Wormhole.transform.position);
                Instance.MoveToGalaxyPos();
                Instance.SetCurrentTargetClickableObject(null);

                UniverseController.Instance.AllGalaxies[i].OnGalaxyClicked(WorldMapClickDetector.ClickableObjectType.Wormhole);

                transform.position = UniverseController.Instance.AllGalaxies[i].Wormhole.transform.position;
                SetPosOnCurrentGalaxy(UniverseController.Instance.AllGalaxies[i].Wormhole.transform.position);
                MotherShipOnWorldMapController.Instance.FuelSystem.OnTeleportStarted();
                FuelSystem.previousKnownZoomLevel = WorldMapMouseController.ZoomLevel.Galaxy;






                OnArrivalToNewTeleportPosition();
                break;
            }
        }
    }

    public void OnArrivalToNewTeleportPosition()
    {
        float distanceToClosestStar = 10000000000.0f;
        StarOnWorldMap closestStar = null;
        int eye = 0;
        bool hasFuelToGoAnywhere = false;

        for (int i = 0; i < GameManager.Instance.CurrentGalaxy.StarSystems.Length; i++)
        {
            Vector3 toSystem = GameManager.Instance.CurrentGalaxy.StarSystems[i].transform.position - transform.position;
            toSystem = new Vector3(toSystem.x, 0, toSystem.z);
            float distanceToSystem = toSystem.magnitude;

            if (distanceToClosestStar <= distanceToSystem)
            {
                closestStar = GameManager.Instance.CurrentGalaxy.StarSystems[i];
                distanceToClosestStar = distanceToSystem;
                eye = i;
            }

            if (FuelSystem.EvaluateNeededFuel(GameManager.Instance.CurrentGalaxy.StarSystems[i].transform.position))
            {
                hasFuelToGoAnywhere = true;
            }
        }

        if (!hasFuelToGoAnywhere)
        {
            Instance.SetPosOnCurrentGalaxy(GameManager.Instance.CurrentGalaxy.StarSystems[eye].transform.position);
            CurrentTargetClickableObject = GameManager.Instance.CurrentGalaxy.StarSystems[eye].GetComponent<WorldMapClickDetector>();
            //Instance.SetPosOnCurrentGalaxy(GameManager.Instance.CurrentGalaxy.StarSystems[eye].transform.position);
            //Instance.MoveToGalaxyPos();
            //closestStar.OnStarClicked(WorldMapClickDetector.ClickableObjectType.StarSystem);
            //Debug.Log("We don't have enough fuel to go anywhere after wormhole");
        }

        //Debug.LogError("CHECK IF WE HAVE ENOUGH FUEL TO ANYWHERE. OTHERWISE DO SOMETHING, BECAUSE THE GAME WOULD GET STUCK");
    }

    public void SetMaterialsToAlwaysRenderOnTop()
    {
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //Material[] allMaterials = meshRenderer.materials;

        ////for (int i = 0; i < allMaterials.Length; i++) 
        ////{
        ////    allMaterials[i].renderQueue = 4000;
        ////    allMaterials[i].SetFloat("_Ztest", 1.0f);
        ////    //allMaterials[i].
        ////    //allMaterials[i].
        ////    Debug.Log("Material on mothership is " + allMaterials[i].name);
        ////}

        //for (int i = 0; i < GetComponent<MeshRenderer>().materials.Length; i++)
        //{
        //    GetComponent<MeshRenderer>().materials[i].renderQueue = 2051;
        //    GetComponent<MeshRenderer>().materials[i].SetFloat("_Ztest", 0.0f);
        //    //allMaterials[i].
        //    //allMaterials[i].
        //    Debug.Log("Material on mothership is " + GetComponent<MeshRenderer>().materials[i].name + " render queue is " + GetComponent<MeshRenderer>().materials[i].renderQueue);
        //}



        //Debug.LogError("WE WANT TO RENDER MOTHERSHIP ON TOP OF EVERYTHING, BUT CURRENTLY HAVEN'T FOUND A WAY TO DO THIS. MAYBE SETUP TWO CAMERAS?");
    }
}
