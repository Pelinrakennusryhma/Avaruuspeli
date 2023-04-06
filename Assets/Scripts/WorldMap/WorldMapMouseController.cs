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

    public static RaycastHit[] Hits;

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
    public void Start()
    {
        GameManager.Instance.OnEnterWorldMap();  
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

        //DoSingleHit(ray);
        //DoMultiHit(ray);
    }

    private static void DoMultiHit(Ray ray)
    {
        Hits = Physics.RaycastAll(ray,
                                  10000.0f);

        // Try to find other than asteroid field.
        // If no other is found, try to detect if asteroid field hit falls between acceptable radius?

        bool hitSomethingOtherThanAsteroidField = false;


        WorldMapClickDetector detector1 = null;

        if (Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < Hits.Length; i++) 
            {

                WorldMapClickDetector detector = Hits[i].collider.GetComponent<WorldMapClickDetector>();

                if (detector != null
                    && detector.type != WorldMapClickDetector.ClickableObjectType.AsteroidField)
                {                
                    detector1 = detector;
                    //detector.OnClick();
                    hitSomethingOtherThanAsteroidField = true;
                    break;
                }

                //Debug.Log("Clicked an object that a raycast hits " + Time.time);
            }
        }

        if (hitSomethingOtherThanAsteroidField)
        {
            //Debug.LogWarning("We hit other than asteroid field " + detector1.gameObject.name);
        }

        else if(Input.GetMouseButtonDown(0))
        {
            for (int i = 0; i < Hits.Length; i++)
            {
                WorldMapClickDetector detector = Hits[i].collider.GetComponent<WorldMapClickDetector>();

                if (detector != null
                    && detector.type == WorldMapClickDetector.ClickableObjectType.AsteroidField)
                {
                    // Get asteroid field component and compare reference point distance

                    bool canClickAsteroidField = false;

                    AsteroidFieldOnWorldMap asteroidField = detector.GetComponent<AsteroidFieldOnWorldMap>();

                    Vector3 yZeroedHitPoint = new Vector3(Hits[i].point.x, 0, Hits[i].point.z);
                    Vector3 yZeroedOriginPos = new Vector3(asteroidField.transform.position.x, 0, asteroidField.transform.position.z);

                    Vector3 yZeroedReferenceObjectPos = asteroidField.ReferenceObject.transform.position;
                    yZeroedReferenceObjectPos = new Vector3(yZeroedReferenceObjectPos.x, 0, yZeroedReferenceObjectPos.z);

                    float distanceToClick = (yZeroedOriginPos - yZeroedHitPoint).magnitude;
                    float distanceToReferenceObjectPos = (yZeroedOriginPos - yZeroedReferenceObjectPos).magnitude;

                    //Debug.Log("Magnitude to click pos is " + distanceToClick + " magnitude to reference object pos is " + distanceToReferenceObjectPos);

                    if ((distanceToClick -distanceToReferenceObjectPos) >= -0.1f
                        && (distanceToClick - distanceToReferenceObjectPos) <= 0.2f)
                    {
                        canClickAsteroidField = true;
                        //Debug.LogError("FALLS WITHIN TOLERANCE");
                    }

                    if (canClickAsteroidField && !GameManager.Instance.Helpers.CheckIfUIisHit()) 
                    {
                        Vector3 hitPoint = new Vector3(Hits[i].point.x, 0, Hits[i].point.z);
                        MotherShipOnWorldMapController.Instance.SetCurrentTargetClickableObjectAndPosOnAsteroidField(detector, hitPoint);
                        //detector.OnClick();
                        //Debug.LogWarning("We CLICKED ASTEROID FIELD");
                        break;
                    }
                }

                //Debug.Log("Clicked an object that a raycast hits " + Time.time);
            }
        }

    }

    private static void DoSingleHit(Ray ray)
    {
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
                    //detector.OnClick();
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
                zoomScale = 1500.0f;
                cameraSpeed = 100.0f * Time.deltaTime;
                break;

            case ZoomLevel.Galaxy:
                zoomScale = 150.0f;
                cameraSpeed = 10.0f * Time.deltaTime;
                break;

            case ZoomLevel.StarSystem:
                zoomScale = 15.0f;
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
            //Debug.LogError("Don't zoom in anymore");
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
                scale = 0.02f; // Maybe too small
                break;

            default:
                break;
        }

        SetZoomOrigin(originPos);
        Camera.main.transform.position = new Vector3(originPos.x, originPos.y + 138.0f * scale, originPos.z);

        if (CurrentZoomLevel == ZoomLevel.Universe)
        {
            ZoomOutButton.HideButton();
            UniverseController.Instance.ShowGalaxies();
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
                CurrentStarSystem.OnZoomOutToStarSystems();
                origin = CurrentGalaxyPos;
                break;

            default:
                break;
        }

        if (CurrentZoomLevel == ZoomLevel.Universe)
        {
            ZoomOutButton.HideButton();
            UniverseController.Instance.ShowGalaxies();
        }

        else
        {
            ZoomOutButton.ShowButton();
            ZoomOutButton.SetZoomText();
        }

        MotherShipController.OnZoom(CurrentZoomLevel, origin);

        //Debug.Log("Should zoom out");
    }
}
