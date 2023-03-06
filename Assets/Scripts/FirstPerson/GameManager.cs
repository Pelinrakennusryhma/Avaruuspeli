using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public GalaxyOnWorldMap CurrentGalaxy;
    public StarSystemOnFocus CurrentStarSystem;
    public PlanetOnWorldMap CurrentPlanet;
    public AsteroidFieldOnWorldMap CurrentAsteroidField;


    public GalaxyData CurrentGalaxyData;
    public StarSystemData CurrentStarSystemData;
    public PlanetData CurrentPlanetData;
    public AsteroidFieldData CurrentAsteroidFieldData;

    public enum TypeOfScene
    {
        None = 0,
        WorldMap = 1,
        Planet = 2,
        AsteroidField = 3
    }

    public TypeOfScene IncomingSceneType;

    public void Awake()
    {
        IncomingSceneType = TypeOfScene.None;
        if (Instance == null)
        {
            //Cursor.visible = false;
            //Cursor.lockState = CursorLockMode.Locked;

            Instance = this;
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            OptionsScreen = FindObjectOfType<Options>(true);

            if (OptionsScreen != null) 
            {
                OptionsScreen.gameObject.SetActive(false);
                OptionsScreen.OnGameStarted();
            }
            
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SceneManager.sceneLoaded += OnSceneLoaded;        
            
            TransitionalCamera.gameObject.SetActive(false);
            SaverLoader = GetComponentInChildren<SaverLoader>(true);
            Helpers = GetComponentInChildren<Helpers>(true);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OptionsScreen = FindObjectOfType<Options>(true);

        if (OptionsScreen != null) 
        {
            OptionsScreen.gameObject.SetActive(false);
            OptionsScreen.OnGameStarted();
        }

        Helpers.RefreshReferenceToGraphicsRaycasterAndEventSystem();
    }

    public void OnPausePressed()
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

    // Update is called once per frame
    void Update()
    {
        if (!IsOnWorldMap
            && Input.GetKeyDown(KeyCode.M))
        {
            GoBackToWorldMap();
        }

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
            }
        }

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

    public void OnPause()
    {
        OptionsScreen.OnBecomeVisible();
        OptionsScreen.gameObject.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        IsPaused = true;
        NormalTimeScale = Time.timeScale;
        Time.timeScale = 0;
        //Debug.Log("Should pause");
    }

    public void OnUnpause()
    {
        OptionsScreen.OnBecomeHidden();
        OptionsScreen.gameObject.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

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

        IncomingSceneType = TypeOfScene.WorldMap;

        GameManager.Instance.TransitionalCamera.gameObject.SetActive(true);

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
        IncomingSceneType = TypeOfScene.None;

        Debug.Log("Going back to world map");
    }
    public void EnterAsteroidField()
    {
        Debug.LogWarning("ENTER ASTEROID FIELD");

        IncomingSceneType = TypeOfScene.AsteroidField;
        StackAndLoadAndLaunchScene("SampleScene", 1);
    }

    public void EnterPlanet()
    {
        Debug.LogWarning("ENTER PLANET");

        IncomingSceneType = TypeOfScene.Planet;
        StackAndLoadAndLaunchScene("PlanetTestLauncher", 2);
    }

    public void StackAndLoadAndLaunchScene(string sceneName,
                                           int buildIndex)
    {
        GameManager.Instance.TransitionalCamera.gameObject.SetActive(true);
        WorldMapScene.Instance.gameObject.SetActive(false);
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        ActiveStackedScene = SceneManager.GetSceneByBuildIndex(buildIndex);
        //ActiveStackedScene = SceneManager.GetSceneByName(sceneName);
        FramesPassedTillLoadScenes = 0;
        WaitingForSceneLoad = true;
        IsOnWorldMap = false;
    }

    public void ActivateStackedScene()
    {
        Debug.Log("We are activating incoming scne of type " + IncomingSceneType.ToString());
        WaitingForSceneLoad = false;
        FramesPassedTillLoadScenes = -1;
        SceneManager.SetActiveScene(ActiveStackedScene);
        GameManager.Instance.TransitionalCamera.gameObject.SetActive(false);

        if (IncomingSceneType == TypeOfScene.Planet)
        {
            PlanetLauncher planetLauncher = FindObjectOfType<PlanetLauncher>();
            planetLauncher.LaunchPlanet(CurrentGalaxy.GalaxyData,
                                        CurrentStarSystem.StarSystemData,
                                        CurrentPlanet.PlanetData);
        }

        else if (IncomingSceneType == TypeOfScene.AsteroidField)
        {
            Debug.LogWarning("This would be a good time and place to pass data from world map to an asteroid field. If needed...");
        }

        IncomingSceneType = TypeOfScene.None;
    }

    public void SetSceneWaitingForNextFrame()
    {

    }

    // WIP ENDED

    public void OnEnterWorldMap()
    {
        IsOnWorldMap = true;
        IncomingSceneType = TypeOfScene.None;
    }

    public void OnLeaveAsteroidSurface()
    {
        GoBackToWorldMap();
        Debug.Log("Ready to leave asteroid surface");
    }
}
