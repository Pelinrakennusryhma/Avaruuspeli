using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;
    public bool IsPaused;
    public float NormalTimeScale;
    public Options OptionsScreen;

    public bool IsOnWorldMap;

    public Scene ActiveStackedScene;

    public int FramesPassedTillLoadScenes;
    public bool WaitingForSceneLoad;

    public Camera TransitionalCamera;

    public SaverLoader SaverLoader;
    public Helpers Helpers;
    public InventoryController InventoryController;

    public LifeSupportSystem LifeSupportSystem;
    public ShipLifeSupportSystem ShipLifeSupportSystem;

    public GalaxyOnWorldMap CurrentGalaxy;
    public StarSystemOnFocus CurrentStarSystem;
    public PlanetOnWorldMap CurrentPlanet;
    public AsteroidFieldOnWorldMap CurrentAsteroidField;
    public PointOfInterest currentPOI;


    public GalaxyData CurrentGalaxyData;
    public StarSystemData CurrentStarSystemData;
    public PlanetData CurrentPlanetData;
    public AsteroidFieldData CurrentAsteroidFieldData;

    private bool inventoryToggleQueued = false;

    private CursorLockMode cachedCursorLockMode = CursorLockMode.None;
    public bool cachedCursorHideMode = true;

    public TypeOfScene CurrentSceneType;

    public delegate void EnterWorldMap();
    public EnterWorldMap OnEnterWorldMap;

    public string ActiveStackedString;
    public bool UseStringWithActiveStackedScene;

    public HungerTracker HungerTracker;

    public WorldMapMessagePrompt WorldMapMessagePrompt;


    public enum TypeOfLaunch
    {
        None = 0,
        NewGame = 1,
        LoadedGame = 2,
        DevGame = 3
    }

    [field: SerializeField]
    public ShipEquipment ShipEquipment { get; private set; }


    public enum TypeOfScene
    {
        None = 0,
        WorldMap = 1,
        Planet = 2,
        AsteroidField = 3,
        POI = 4
    }

    public TypeOfScene IncomingSceneType;

    public static TypeOfLaunch LaunchType;

    public void Awake()
    {
        //LaunchType = TypeOfLaunch.LoadedGame; // Set this at the start screen

        DevCheck devCheck = FindObjectOfType<DevCheck>();
        DevListener devListener = FindObjectOfType<DevListener>();

        if (!devCheck
            && devListener)
        {
            return;
        }

        IncomingSceneType = TypeOfScene.None;
        if (Instance == null)
        {
            Instance = this;
            Helpers = GetComponentInChildren<Helpers>(true);
            SaverLoader = GetComponentInChildren<SaverLoader>(true);
            InventoryController = GetComponentInChildren<InventoryController>(true);
            HungerTracker = GetComponentInChildren<HungerTracker>(true);
            WorldMapMessagePrompt = GetComponentInChildren<WorldMapMessagePrompt>(true);
            LifeSupportSystem = GetComponentInChildren<LifeSupportSystem>(true);
            ShipLifeSupportSystem = GetComponentInChildren<ShipLifeSupportSystem>(true);

            SaverLoader.OnInitialStartUp();
            InventoryController.Init();
            WorldMapMessagePrompt.Init();
            LifeSupportSystem.Init();
            ShipLifeSupportSystem.Init();


            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            Options.OnLaunch();

            if (OptionsScreen != null) 
            {
                OptionsScreen.gameObject.SetActive(false);
                OptionsScreen.OnGameStarted();
            }
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;        
            
            TransitionalCamera.gameObject.SetActive(false);



            if(CurrentSceneType == TypeOfScene.None)
            {
                CurrentSceneType = TypeOfScene.WorldMap;
            }

            //Debug.Log("Don't destroy game manager");
            ShipEquipment.Init();
        }

        else
        {
            DestroyImmediate(gameObject);
            //Debug.LogWarning("Destroyed game manager");
        }
    }

    public void Start()
    {
        OnEnterWorldMapCall(false);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(OptionsScreen == null)
        {
            OptionsScreen = FindObjectOfType<Options>(true);
        }

        if (OptionsScreen != null) 
        {
            OptionsScreen.gameObject.SetActive(false);
            OptionsScreen.OnGameStarted();
            OptionsScreen.OnBecomeHidden();
        }

        Helpers.RefreshReferenceToGraphicsRaycasterAndEventSystem();
        AudioManager.Instance.SetMusicAreaBySceneIndex(scene.buildIndex);
    }

    public void OnOptionsPressed()
    {
        if (IsPaused)
        {
            OnUnpause();
        }

        else
        {
            OnPause();
        }
    }

    public void OnInventoryPressed()
    {
        inventoryToggleQueued = true;    
    }

    // Update is called once per frame
    void Update()
    {
        //if (!IsOnWorldMap
        //    && Input.GetKeyDown(KeyCode.M)
        //    && !InventoryController.IsShopping)
        //{

        //    GoBackToWorldMap();
        //}

        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    InventoryController.Inventory.AddItem(30, 100); // CArbon
        //    InventoryController.Inventory.AddItem(27, 100); // Ice
        //    InventoryController.Inventory.AddItem(28, 100); // silicate
        //}

        if (WaitingForSceneLoad
            && FramesPassedTillLoadScenes >= 0)
        {
            FramesPassedTillLoadScenes++;

            if (FramesPassedTillLoadScenes >= 2
                && !IsOnWorldMap)
            {
                ActivateStackedScene();
            }

            else if(FramesPassedTillLoadScenes >= 2
                    && IsOnWorldMap)
            {
                GameManager.Instance.TransitionalCamera.gameObject.SetActive(false);
                OnEnterWorldMapCall(true);
                //Debug.LogError("We are on world map");
            }
        }


        else if (!WaitingForSceneLoad)
        {
            if (!IsPaused) 
            {
                if (inventoryToggleQueued)
                {
                    //Debug.Log("InventoryToggle");
                    inventoryToggleQueued = false;
                    if (InventoryController.ShowingInventory) 
                    {
                        InventoryController.OnInventoryHide(true);
                    }

                    else
                    {
                        InventoryController.OnInventoryShow(true);
                    }
                }
            }
        }

        //Debug.Log("Invenotry toggle is queued " + inventoryToggleQueued + " waiting for scene load " + WaitingForSceneLoad);

        //if (Input.GetKeyDown(KeyCode.Escape)
        //    || Input.GetKeyDown(KeyCode.O))
        //{
        //    if (IsPaused) 
        //    {
        //        OnUnpause();
        //    }

        //    else
        //    {
        //        OnPause();
        //    }
        //}
    }

    void OnPause()
    {

        cachedCursorLockMode = Cursor.lockState;
        cachedCursorHideMode = Cursor.visible;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (OptionsScreen != null) 
        {
            OptionsScreen.OnBecomeVisible();
            OptionsScreen.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("No options screen found. Surely one will be added eventually with full functionality");
        }

        IsPaused = true;
        NormalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        //Debug.Log("Should pause");
    }

    void OnUnpause()
    {
        Cursor.lockState = cachedCursorLockMode;
        Cursor.visible = cachedCursorHideMode;



        if (OptionsScreen != null) 
        {
            OptionsScreen.OnBecomeHidden();
            OptionsScreen.gameObject.SetActive(false);
        }

        IsPaused = false;
        Time.timeScale = NormalTimeScale;
        //Debug.Log("Should unpause");
    }

    public void SetMouseSensitivity(float sensitivity)
    {
        
    }

    public void SetMouseInvert(bool invert)
    {

    }





    /////////////////////////
    /// WIP

    //public void GoBackToWorldMap()
    //{
    //    IsOnWorldMap = true;

    //    if (WorldMapScene.Instance != null)
    //    {
    //        WorldMapScene.Instance.gameObject.SetActive(true);
    //    }

    //    else
    //    {
    //        SceneManager.LoadScene("WorldMap");
    //    }




    //    Debug.Log("Going back to world map");
    //}
    //public void EnterAsteroidField()
    //{
    //    Debug.LogError("ENTER ASTEROID FIELD");

    //    IsOnWorldMap = false;
    //    WorldMapScene.Instance.gameObject.SetActive(false);
    //    SceneManager.LoadScene("SampleScene");
    //}

    //public void EnterPlanet()
    //{
    //    Debug.LogError("ENTER PLANET");
    //    WorldMapScene.Instance.gameObject.SetActive(false);
    //    SceneManager.LoadScene("PlanetTestLauncher");
    //    IsOnWorldMap = false;
    //}

    //WIP

    public void GoBackToWorldMap()
    {
        Cursor.lockState = CursorLockMode.None;
        IncomingSceneType = TypeOfScene.WorldMap;

        GameManager.Instance.TransitionalCamera.gameObject.SetActive(true);

        HungerTracker.OnLeaveFirstPersonScene();

        if (WorldMapScene.Instance != null)
        {        
            SceneManager.UnloadSceneAsync(ActiveStackedScene);
            WorldMapScene.Instance.gameObject.SetActive(true);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("WorldMap"));
        }

        else
        {
            SceneManager.LoadScene("WorldMap");
        }



        IsOnWorldMap = true;
        WaitingForSceneLoad = true;
        FramesPassedTillLoadScenes = 0;
        CurrentSceneType = IncomingSceneType;
        IncomingSceneType = TypeOfScene.None;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("This caused visibility");

        //LifeSupportSystem.OnExitUnbreathablePlace();


        ShipLifeSupportSystem.OnExitShip();

        //Debug.Log("Going back to world map");

        AudioManager.Instance.SetMusicAreaBySceneIndex(1);
        LoadingScreen.Instance.EnableCanvas();
    }


    public void EnterAsteroidField()
    {
        SaveLifeSupportData(true);
        Debug.LogWarning("ENTER ASTEROID FIELD");

        IncomingSceneType = TypeOfScene.AsteroidField;
        StackAndLoadAndLaunchScene("MineableAsteroidScene", 2);
    }

    public void EnterPOI(PointOfInterest pointOfInterest)
    {

        currentPOI = pointOfInterest;
        EnterPOI();
    }

    public void EnterPOI()
    {        
        SaveLifeSupportData(true);
        IncomingSceneType = TypeOfScene.POI;
        Debug.Log("currentPOI: " + currentPOI);
        Debug.Log("Data: " + currentPOI.Data);
        Debug.Log("SceneToLoad: " + currentPOI.Data.SceneToLoad);
        StackAndLoadAndLaunchScene(currentPOI.Data.SceneToLoad, -1);
    }

    public void EnterPlanet()
    {
        SaveLifeSupportData(true);
        ShipLifeSupportSystem.OnExitShip();
        HungerTracker.OnEnterFirstPersonScene();

        //Debug.LogWarning("ENTER PLANET");

        PlanetData.PlanetGraphicsType planetType = CurrentPlanetData.PlanetGraphics;
        IncomingSceneType = TypeOfScene.Planet;

        //Debug.Log("Current planet type is " + planetType.ToString());

        //int rando = Random.Range(0, 6);
        //rando = 0;

        //if (rando == 0)
        //{
        //    StackAndLoadAndLaunchScene("Maapallo", 3);
        //}

        //else if (rando == 1)
        //{
        //    StackAndLoadAndLaunchScene("Marssi", 4);
        //}

        //else if (rando == 2)
        //{
        //    StackAndLoadAndLaunchScene("Kuu", 5);
        //}

        //else if (rando == 3)
        //{
        //    StackAndLoadAndLaunchScene("LumiMaa", 6);
        //}

        //else if (rando == 4)
        //{
        //    StackAndLoadAndLaunchScene("Pluto", 7);
        //}

        //else if (rando == 5)
        //{
        //    StackAndLoadAndLaunchScene("PuuMaa", 8);
        //}

        int maaPalloIndex = 4;
        int marssiIndex = 5;
        int kuuIndex = 6; // We are not using this scene yet. Why not?
        int lumiMaaIndex = 7;
        int plutoIndex = 8;
        int puuMaaIndex = 9;


        string maaPalloName = "Maapallo";
        string marssiName = "Marssi";
        string kuuName = "Kuu";
        string lumiMaaName = "LumiMaa";
        string plutoName = "Pluto";
        string puuMaaName = "PuuMaa";

        switch (planetType) 
        {
            case PlanetData.PlanetGraphicsType.None:
                Debug.LogError("We are entering planet, but we don't have a type of planet.");
                break;

            case PlanetData.PlanetGraphicsType.Placeholder1:
                StackAndLoadAndLaunchScene(maaPalloName, maaPalloIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder2:
                StackAndLoadAndLaunchScene(marssiName, marssiIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder3:
                StackAndLoadAndLaunchScene(plutoName, plutoIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder4:
                StackAndLoadAndLaunchScene(puuMaaName, puuMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder5:
                StackAndLoadAndLaunchScene(plutoName, plutoIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder6:
                StackAndLoadAndLaunchScene(puuMaaName, puuMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder7:
                StackAndLoadAndLaunchScene(kuuName, kuuIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder8:
                StackAndLoadAndLaunchScene(marssiName, marssiIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder9:
                StackAndLoadAndLaunchScene(marssiName, marssiIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder10:
                StackAndLoadAndLaunchScene(kuuName, kuuIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder11:
                StackAndLoadAndLaunchScene(puuMaaName, puuMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder12:
                StackAndLoadAndLaunchScene(kuuName, kuuIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder13:
                StackAndLoadAndLaunchScene(puuMaaName, puuMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder14:
                StackAndLoadAndLaunchScene(kuuName, kuuIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder15:
                StackAndLoadAndLaunchScene(puuMaaName, puuMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder16:
                StackAndLoadAndLaunchScene(lumiMaaName, lumiMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder17:
                StackAndLoadAndLaunchScene(maaPalloName, maaPalloIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder18:
                StackAndLoadAndLaunchScene(kuuName, kuuIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder19:
                StackAndLoadAndLaunchScene(maaPalloName, maaPalloIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder20:
                StackAndLoadAndLaunchScene(plutoName, plutoIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder21:
                StackAndLoadAndLaunchScene(maaPalloName, maaPalloIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder22:
                StackAndLoadAndLaunchScene(marssiName, marssiIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder23:
                StackAndLoadAndLaunchScene(plutoName, plutoIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder24:
                StackAndLoadAndLaunchScene(lumiMaaName, lumiMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder25:
                StackAndLoadAndLaunchScene(maaPalloName, maaPalloIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder26:
                StackAndLoadAndLaunchScene(marssiName, marssiIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder27:
                StackAndLoadAndLaunchScene(lumiMaaName, lumiMaaIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder28:
                StackAndLoadAndLaunchScene(maaPalloName, maaPalloIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder29:
                StackAndLoadAndLaunchScene(plutoName, plutoIndex);
                break;

            case PlanetData.PlanetGraphicsType.Placeholder30:
                StackAndLoadAndLaunchScene(lumiMaaName, lumiMaaIndex);
                break;

            default:
                Debug.LogError("We are entering planet, but we don't have a type of planet.");
                break;

                //    StackAndLoadAndLaunchScene("Maapallo", 3);


                //StackAndLoadAndLaunchScene("Marssi", 4);



                //StackAndLoadAndLaunchScene("Kuu", 5);



                //StackAndLoadAndLaunchScene("LumiMaa", 6);



                //StackAndLoadAndLaunchScene("Pluto", 7);



                //StackAndLoadAndLaunchScene("PuuMaa", 8);
        }


        //StackAndLoadAndLaunchScene("PlanetTestLauncher", 2);
    }

    public void StackAndLoadAndLaunchScene(string sceneName,
                                           int buildIndex)
    {
        GameManager.Instance.TransitionalCamera.gameObject.SetActive(true);
        WorldMapScene.Instance.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);

        if (buildIndex >= 0) 
        {
            UseStringWithActiveStackedScene = false;
            ActiveStackedScene = SceneManager.GetSceneByBuildIndex(buildIndex);
        }

        else
        {
            //ActiveStackedString = sceneName;
            //UseStringWithActiveStackedScene = true;

            ActiveStackedScene = SceneManager.GetSceneByName(sceneName);
            //Debug.LogError("We cannot get scene by name in this case, because the scene is not loaded yet.");
        }


        FramesPassedTillLoadScenes = 0;
        WaitingForSceneLoad = true;
        IsOnWorldMap = false;

        LoadingScreen.Instance.EnableCanvas();
    }

    public void ActivateStackedScene()
    {
        //Debug.Log("We are activating incoming scene of type " + IncomingSceneType.ToString());
        WaitingForSceneLoad = false;
        FramesPassedTillLoadScenes = -1;


        SceneManager.SetActiveScene(ActiveStackedScene);
        


        
        GameManager.Instance.TransitionalCamera.gameObject.SetActive(false);

        if (IncomingSceneType == TypeOfScene.Planet)
        {
            //Debug.Log("Here should be logic for launching a certain planet type");
            //PlanetLauncher planetLauncher = FindObjectOfType<PlanetLauncher>();
            //planetLauncher.LaunchPlanet(CurrentGalaxy.GalaxyData,
            //                            CurrentStarSystem.StarSystemData,
            //                            CurrentPlanet.PlanetData);
        }

        else if (IncomingSceneType == TypeOfScene.AsteroidField)
        {
            //Debug.LogWarning("This would be a good time and place to pass data from world map to an asteroid field. If needed...");
        }

        else if (IncomingSceneType == TypeOfScene.POI)
        {
            //Debug.LogWarning("Incoming scene is of type POI");
        }

        CurrentSceneType = IncomingSceneType;
        IncomingSceneType = TypeOfScene.None;

        if (CurrentSceneType == TypeOfScene.WorldMap)
        {
            OnEnterWorldMapCall(true);
        }
    }

    public void SetSceneWaitingForNextFrame()
    {

    }

    // WIP ENDED

    public void OnEnterWorldMapCall(bool saveLifeSupportData)
    {
        WaitingForSceneLoad = false;
        IsOnWorldMap = true;
        IncomingSceneType = TypeOfScene.None;

        if (saveLifeSupportData) 
        {
            SaveLifeSupportData(true);
        }

        if (ShipLifeSupportSystem.WaitingToShowRunningOutOfOxygenPrompt)
        {
            ShipLifeSupportSystem.ActivateRunninOutOfOxygenPrompt();
        }

        if (OnEnterWorldMap != null)
        {
            OnEnterWorldMap();
        }



       // Debug.LogWarning("On enter world map called");
    }

    public void OnLeaveAsteroidSurface()
    {
        GoBackToWorldMap();
        //Debug.Log("Ready to leave asteroid surface");
    }

    public void OnEnterAsteroidSurface()
    {
        SaveLifeSupportData(true);
        HungerTracker.OnEnterFirstPersonScene();
        //Debug.Log("Gamemanager knows we entered asteroid surface");
    }

    public void SaveLifeSupportData(bool saveInventoryToo)
    {
        //InventoryController.HydroponicsBay.SaveRelevantData();
        //LifeSupportSystem.SaveRelevantData();
        //ShipLifeSupportSystem.SaveRelevantData();

        float amountOfOxygenInLastBottle = LifeSupportSystem.GetAmountOfOxygenInLastBottle();

        ShipLifeSupportSystem.GetRelevantSaveData(out float amountOfOxygenInLastStorage,
                                                  out float timeThatHydroponicsHasBeenOutOfResources,
                                                  out float amountOfCarbonInLastUnit,
                                                  out float amountOfWaterInLastBottle);


        GameManager.Instance.SaverLoader.SaveLifeSupportData(amountOfOxygenInLastStorage,
                                                             timeThatHydroponicsHasBeenOutOfResources,
                                                             amountOfCarbonInLastUnit,
                                                             amountOfWaterInLastBottle);

        //Debug.LogError("Here has been some refactoring. Check that everything still works as intended!");

        if (saveInventoryToo) 
        {
            InventoryController.SaveInventory();
        }
    }

}
