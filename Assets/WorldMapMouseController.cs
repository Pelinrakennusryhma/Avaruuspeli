using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapMouseController : MonoBehaviour
{
    public enum ZoomLevel
    {
        None = 0,
        Universe = 1,
        Galaxy = 2,
        StarSystem = 3
    }

    public MotherShipOnWorldMapController MotherShipController;

    public ZoomLevel CurrentZoomLevel;

    public Vector3 zoomOrigin = Vector3.zero;

    public static WorldMapMouseController Instance;

    public GalaxyOnWorldMap CurrentGalaxy;
    public StarOnWorldMap CurrentStarSystem;


    public Vector3 CurrentUniversePos;
    public Vector3 CurrentGalaxyPos;
    public Vector3 CurrentStartSystemPos;

    public ZoomOutOnWorldMapButton ZoomOutButton;

    public void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        else
        {
            Destroy(gameObject);
        }

        CurrentZoomLevel = ZoomLevel.Universe;
        ZoomOutButton.Init();
        ZoomOutButton.HideButton();
    }

    public void ChangeZoomLevel(ZoomLevel newZoomLevel,
                                Vector3 zoomOrigin)
    {
        CurrentZoomLevel = newZoomLevel;


    }

    void Update()
    {



        MoveScreen();


        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition, Camera.MonoOrStereoscopicEye.Mono);
        RaycastHit hit;

        if (Physics.Raycast(ray,
                            out hit,
                            10000.0f))
        {
            //Debug.Log("Raycast hits " + hit.collider.gameObject.name);

            if (Input.GetMouseButtonDown(0))
            {
                WorldMapClickDetector detector = hit.collider.GetComponent<WorldMapClickDetector>();

                if (detector != null)
                {
                    detector.OnClick();
                }

                Debug.Log("Clicked an object that a raycast hits " + Time.time);
            }
        }

        else
        {
            //Debug.Log("Raycast didn't hit anything");
        }
    }

    private void MoveScreen()
    {
        // Detect when mouse is on the edge of screen and move camera accordingly
        // Clamp position to a proper distance from origin
        // TODO

        bool onUpperPartOfScreen = false;
        bool onLowerPartOfScreen = false;
        bool onLeftPartOfScreen = false;
        bool onRightPartOfScreen = false;

        Vector3 mousePos = Input.mousePosition;
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;


        //Debug.Log("Mouse pos is " + mousePos + " screen width is " + screenWidth + " and height is " + screenHeight);

        float edgePercentage = 0.95f;

        if (mousePos.x >= screenWidth * edgePercentage)
        {
            onRightPartOfScreen = true;
        }

        else if(mousePos.x <= screenWidth * (1.0f - edgePercentage))
        {
            onLeftPartOfScreen = true;
        }

        if (mousePos.y >= screenHeight * edgePercentage)
        {
            onUpperPartOfScreen = true;
        }

        else if (mousePos.y <= screenHeight * (1.0f - edgePercentage))
        {
            onLowerPartOfScreen = true;
        }


        float horizontal = 0;
        float vertical = 0;

        if (onUpperPartOfScreen)
        {
            vertical = 1.0f;
            //Debug.Log("On upper part of screen " + Time.time);
        }

        else if (onLowerPartOfScreen)
        {
            vertical = -1.0f;
            //Debug.Log("On lower part of screen " + Time.time);
        }

        if (onLeftPartOfScreen)
        {
            horizontal = -1.0f;
            //Debug.Log("On left part of screen " + Time.time);
        }

        else if (onRightPartOfScreen)
        {
            horizontal = 1.0f;
            //Debug.Log("On right part of screen " + Time.time);
        }

        float cameraSpeed = 100.0f * Time.deltaTime;
        float zoomScale = 1000.0f;

        switch (CurrentZoomLevel)
        {
            case ZoomLevel.None:
                break;

            case ZoomLevel.Universe:
                zoomScale = 1000.0f;
                cameraSpeed = 100.0f * Time.deltaTime;
                break;

            case ZoomLevel.Galaxy:
                zoomScale = 100.0f;
                cameraSpeed = 10.0f * Time.deltaTime;
                break;

            case ZoomLevel.StarSystem:
                zoomScale = 10.0f;
                cameraSpeed = 1.0f * Time.deltaTime;
                break;

            default:
                break;
        }

        Camera.main.transform.position += new Vector3(horizontal * cameraSpeed, 
                                                      0, 
                                                      vertical * cameraSpeed);





        Vector3 cameraPos = Camera.main.transform.position;

        //cameraPos = new Vector3(Mathf.Clamp(cameraPos.x, -zoomScale, zoomScale),
        //                                    cameraPos.y,
        //                        Mathf.Clamp(cameraPos.z, -zoomScale, zoomScale));

        cameraPos = new Vector3(Mathf.Clamp(cameraPos.x, -zoomScale + zoomOrigin.x, zoomScale + zoomOrigin.x),
                                    cameraPos.y,
                                Mathf.Clamp(cameraPos.z, -zoomScale + zoomOrigin.z, zoomScale + zoomOrigin.z));

        Camera.main.transform.position = cameraPos;

        switch (CurrentZoomLevel)
        {
            case ZoomLevel.None:
                break;

            case ZoomLevel.Universe:
                CurrentUniversePos = cameraPos;
                break;

            case ZoomLevel.Galaxy:
                CurrentGalaxyPos = cameraPos;
                break;

            case ZoomLevel.StarSystem:
                break;

            default:
                break;
        }
    }

    public void SetZoomOrigin(Vector3 zoomOrigin)
    {
        this.zoomOrigin = zoomOrigin;
       // Debug.Log("Should set zoom origin");
    }

    public void ZoomIn(Vector3 originPos,
                       ZoomLevel newZoomLevel,
                       GalaxyOnWorldMap currentGalaxy,
                       StarOnWorldMap currentStar)
    {
        if (newZoomLevel == ZoomLevel.None)
        {
            Debug.LogError("Don't zoom in anymore");
            return;
        }

        CurrentGalaxy = currentGalaxy;
        CurrentStarSystem = currentStar;

        CurrentZoomLevel = newZoomLevel;

        float scale = 1.0f;

        switch (CurrentZoomLevel)
        {
            case ZoomLevel.None:
                break;
            case ZoomLevel.Universe:
                scale = 1.0f;
                break;

            case ZoomLevel.Galaxy:
                scale = 0.1f;
                break;

            case ZoomLevel.StarSystem:
                scale = 0.01f; // Maybe too small
                break;

            default:
                break;
        }

        SetZoomOrigin(originPos);
        Camera.main.transform.position = new Vector3(originPos.x, originPos.y + 138.0f * scale, originPos.z);

        if (CurrentZoomLevel == ZoomLevel.Universe)
        {
            ZoomOutButton.HideButton();
        }

        else
        {
            ZoomOutButton.ShowButton();
            ZoomOutButton.SetZoomText();
        }

        MotherShipController.OnZoom(CurrentZoomLevel, originPos);

        //Debug.Log("Should zoom in");
    }

    public void ZoomOut()
    {
        Vector3 origin = Vector3.zero;

        switch (CurrentZoomLevel)
        {
            case ZoomLevel.None:
                break;
            case ZoomLevel.Universe:
                Debug.Log("Hide the button on universe scale!!!");
                break;
            case ZoomLevel.Galaxy:
                Camera.main.transform.position = CurrentUniversePos;
                CurrentZoomLevel = ZoomLevel.Universe;
                SetZoomOrigin(CurrentUniversePos);
                CurrentGalaxy.OnZoomOutToGalaxy();

                origin = CurrentUniversePos;
                break;

            case ZoomLevel.StarSystem:                
                Camera.main.transform.position = CurrentGalaxyPos;
                CurrentZoomLevel = ZoomLevel.Galaxy;
                SetZoomOrigin(CurrentGalaxyPos);
                CurrentStarSystem.OnZoomOutToStarSystem();
                origin = CurrentGalaxyPos;
                break;

            default:
                break;
        }

        if (CurrentZoomLevel == ZoomLevel.Universe)
        {
            ZoomOutButton.HideButton();
        }

        else
        {
            ZoomOutButton.ShowButton();
            ZoomOutButton.SetZoomText();
        }

        MotherShipController.OnZoom(CurrentZoomLevel, origin);

        Debug.Log("Should zoom out");
    }
}
