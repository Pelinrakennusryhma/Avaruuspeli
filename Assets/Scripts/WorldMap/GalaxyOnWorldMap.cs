using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyOnWorldMap : MonoBehaviour
{
    public GalaxyData GalaxyData;

    public WorldMapClickDetector ClickDetector;

    public StarOnWorldMap[] StarSystems;

    public MeshRenderer GalaxyMesh;
    public Collider GalaxyCollider;

    public LineRenderer[] LinesToOtherGalaxies;

    public bool LinesHaveBeenCreated;

    public UniverseController UniverseController;

    private bool HasBeenInitted;

    public bool HasBeenGenerated;

    public GameObject StarSystemPrefab;

    public GameObject[] PlaceHolderStarSystems;

    public WormholeOnWorldMap Wormhole;

    public void Start()
    {
        if (!HasBeenInitted)
        {
            //Init();
        }

        //HideStarsAndShowGalaxy();
    }

    public void Init(GalaxyData data)
    {
        GalaxyData = data;
        ClickDetector = GetComponent<WorldMapClickDetector>();
        ClickDetector.OnObjectClicked -= OnGalaxyClicked;
        ClickDetector.OnObjectClicked += OnGalaxyClicked;

        Wormhole.WormholeData = data.WormholeData;

        HideWormhole();

        HasBeenInitted = true;        
        

    }

    public void DrawLinesBetweenGalaxies(UniverseController universe)
    {
        GalaxyMesh.gameObject.SetActive(true);

        if (universe == null)
        {
            universe = FindObjectOfType<UniverseController>(true);
            universe.Init();
        }


        if (!HasBeenInitted)
        {
            Debug.LogError("JUST IN CASE INITIALIZATION REMOVED FROM HERE. MAKE SURE THINGS WORK");
            //Init();
        }

        UniverseController = universe;

        if (!LinesHaveBeenCreated)
        {
            //Create lines

            for (int i = 0; i < universe.AllGalaxies.Length; i++)
            {
                GameObject newObject = new GameObject();
                newObject.transform.parent = gameObject.transform;
                newObject.AddComponent<LineRenderer>();
            }

            LinesToOtherGalaxies = GetComponentsInChildren<LineRenderer>();
        }

        for (int i = 0; i < LinesToOtherGalaxies.Length; i++)
        {
            LinesToOtherGalaxies[i].enabled = true;
            LinesToOtherGalaxies[i].loop = false;
            LinesToOtherGalaxies[i].useWorldSpace = true;
            LinesToOtherGalaxies[i].startWidth = 1.0f;
            LinesToOtherGalaxies[i].endWidth = 1.0f;
            LinesToOtherGalaxies[i].material = universe.LineRendererMat;
        }

        Vector3 startPoint = transform.position;

        for (int i = 0; i < universe.AllGalaxies.Length; i++)
        {
            Vector3 endPoint = universe.AllGalaxies[i].transform.position;

            LinesToOtherGalaxies[i].SetPosition(0, startPoint);
            LinesToOtherGalaxies[i].SetPosition(1, endPoint);
        }

        //Debug.LogError("Drawing lines between galaxies");
    }

    public void OnGalaxyClicked(WorldMapClickDetector.ClickableObjectType type)
    {
        //Debug.LogWarning("Clicked galaxy");
        //WorldMapMouseController.Instance.SetZoomOrigin(transform.position);
        WorldMapMouseController.Instance.ZoomIn(transform.position,
                                                WorldMapMouseController.ZoomLevel.Galaxy,
                                                this, 
                                                null);
        //Debug.Log("Clicked a galaxy");


        GameManager.Instance.CurrentGalaxy = this;
        GameManager.Instance.CurrentGalaxyData = GalaxyData;


        ShowStars();
        UniverseController.Instance.HideGalaxyLineRenderers();
    }

    public void ShowStars()
    {
        //Debug.LogError("Now would be a good time to init stars");

        if (!HasBeenGenerated)
        {
            GenerateGalaxy();
        }

        for (int i = 0; i < StarSystems.Length; i++) 
        {
            StarSystems[i].gameObject.SetActive(true);
            StarSystems[i].DrawLinesBetweenStars(this);
        }

        ShowWormhole();

        GalaxyCollider.enabled = false;
        GalaxyMesh.enabled = false;

        
        //Debug.LogError("Show stars");
    }

    public void HideStarsAndShowGalaxy()
    {
        for (int i = 0; i < StarSystems.Length; i++)
        {
            StarSystems[i].gameObject.SetActive(false);
            StarSystems[i].HideLinesBetweenStars();
        }

        HideWormhole();
        GalaxyCollider.enabled = true;
        GalaxyMesh.enabled = true;
        DrawLinesBetweenGalaxies(UniverseController.Instance);
    }

    public void HideOtherStars(StarOnWorldMap toNotHide)
    {
        for (int i = 0; i < StarSystems.Length; i++)
        {
            if (StarSystems[i] != toNotHide
                && StarSystems[i] != null) 
            {
                StarSystems[i].gameObject.SetActive(false);
            }
        }
    }

    public void OnZoomOutToGalaxy()
    {
        HideStarsAndShowGalaxy();
    }

    public void SetLineRenderersInactive()
    {
        for (int i = 0; i < LinesToOtherGalaxies.Length; i++)
        {
            LinesToOtherGalaxies[i].gameObject.SetActive(false);
        }

        //Debug.LogError("Setting line renderers inactive. Array length is " + LinesToOtherGalaxies.Length);
    }

    public void HideStarLineRenderers()
    {
        for (int i = 0; i < StarSystems.Length; i++)
        {
            StarSystems[i].HideLinesBetweenStars();
        }
    }

    public void ShowWormhole()
    {
        if (GalaxyData.WormholeData != null
            && GalaxyData.WormholeData.ID > 0) 
        {
            Wormhole.gameObject.SetActive(true);
            //Debug.LogWarning("This galaxy has a wormhole");
        }

        else
        {
            //Debug.LogError("This galaxy doesn't have a wormhole");
        }
    }

    public void HideWormhole()
    {
        Wormhole.gameObject.SetActive(false);
    }

    public void GenerateGalaxy()
    {
        if (!HasBeenInitted)
        {
            //Debug.LogError("JUST IN CASE INITIALIZATION REMOVED FROM HERE. MAKE SURE THINGS WORK");
            // Init();
        }

        GalaxyData data = null;
        bool successfulLoad = GameManager.Instance.SaverLoader.LoadGalaxy(GalaxyData.ID,
                                                                          out data);

        for (int i = 0; i < PlaceHolderStarSystems.Length; i++)
        {
            DestroyImmediate(PlaceHolderStarSystems[i].gameObject);
        }

        //successfulLoad = false;

        if (!successfulLoad 
            || data == null
            || (data != null && data.StarSystems.Count <= 0))
        {
            GenerateNewGalaxy();
            Debug.Log("Generated a new galaxy");

        }

        else
        {
           // does not work
            RegenerateGalaxyFromData(data);
            Debug.Log("Regenerated galaxy from data");
        }

        StarSystems = GetComponentsInChildren<StarOnWorldMap>(true);

       // Debug.LogError("NOW WOULD BE A GOOD TIME AND PLACE TO GENERATE A GALAXY");

        HasBeenGenerated = true;
    }

    private void GenerateNewGalaxy()
    {
        int amountOfStars = Random.Range(6, 13);
        //amountOfStars = 12;
        float distance = 0.0f;

        int segments = Random.Range(0, 361);

        //Debug.Log("DECIDE HERE WHERE WORMHOLES SHOULD BE PLACED?");

        int wormholeIteration = -1;
        int addWormhole = 0;

        if (GalaxyData.WormholeData != null
            && GalaxyData.WormholeData.ID > 0)
        {
            addWormhole = 1;
            
            if (amountOfStars >= 10) 
            {
                wormholeIteration = Random.Range(3, amountOfStars - 4);
            }

            else
            {
                wormholeIteration = Random.Range(3, amountOfStars - 2);
            }
            //Debug.LogWarning("THIS GALAXY HAS A WROMHOLE. SET POSITION FOR ONE");
        }



        for (int i = 0; i < amountOfStars + addWormhole; i++)
        {

            if (addWormhole >= 0
                && i == wormholeIteration)
            {
                //float rad = Mathf.Deg2Rad * (360f / segments);
                distance += 2.0f;
                float rad = Mathf.Deg2Rad * Random.Range(0, 360.0f);
                float radius = distance;
                Wormhole.transform.position = transform.position + new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);

                distance += Random.Range(4.0f, 5.0f);
                segments += Random.Range(90, 125);

                Wormhole = GetComponentInChildren<WormholeOnWorldMap>(true);

                Wormhole.WormholeData = GalaxyData.WormholeData;
                Wormhole.WormholeData.SetPosRotAndScale(Wormhole.transform.position,
                                                        Wormhole.transform.rotation,
                                                        Wormhole.transform.localScale);

                GalaxyData.WormholeData = Wormhole.WormholeData;
                //GameManager.Instance.SaverLoader.SaveGalaxy();
                //Debug.LogError("Now would be the time to decide wormhole place");
            }

            else
            {
                //Vector3 pos = transform.position + Vector3.forward * distance;

                GameObject objectOriginal = StarSystemPrefab;
                GameObject star = Instantiate(objectOriginal, transform.position, Quaternion.identity, transform);
                star.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                //star.transform.position = pos;


                //float rad = Mathf.Deg2Rad * (360f / segments);
                float rad = Mathf.Deg2Rad * Random.Range(0, 360.0f);
                float radius = distance;
                star.transform.position = transform.position + new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);

                distance += Random.Range(2.0f, 3.0f);
                segments += Random.Range(90, 125);
                //Debug.Log("Spawned a star " + i);

                StarSystemOnFocus starSystem = star.GetComponentInChildren<StarSystemOnFocus>(true);
                StarSystemData starSystemData = new StarSystemData();
                starSystemData.ID = i + 1;
                starSystemData.SetPosRotAndScale(starSystem.transform.position,
                                                 starSystem.transform.rotation,
                                                 star.transform.localScale);

                //Debug.Log("star system saved pos is x " + starSystemData.posX + " y " + starSystemData.posY + " z " + starSystemData.posZ);

                //Debug.LogError("Should have set pos rot and scale");
                starSystem.Initialize(starSystemData, this);

            }
            //GalaxyData.StarSystems.Add(starSystemData);

            //Debug.Log("INITIALIZING WITHIN GALAXY");
        }

        GameManager.Instance.SaverLoader.SaveGalaxy(GalaxyData.ID,
                                                    GalaxyData.StarSystems);
    }

    public void RegenerateGalaxyFromData(GalaxyData data)
    {
        if (!HasBeenInitted)
        {
            //Debug.LogError("JUST IN CASE INITIALIZATION REMOVED FROM HERE. MAKE SURE THINGS WORK");
            // Init();
        }

        //GenerateNewGalaxy();
        //return;

        for (int i = 0; i < PlaceHolderStarSystems.Length; i++)
        {
            DestroyImmediate(PlaceHolderStarSystems[i].gameObject);
        }

        //Debug.LogError("Data count is " + data.Count);

        if (GalaxyData.WormholeData != null
            && GalaxyData.WormholeData.ID > 0)
        {
            Wormhole = GetComponentInChildren<WormholeOnWorldMap>(true);
            Wormhole.transform.position = GalaxyData.WormholeData.GetPos();
            Wormhole.transform.rotation = GalaxyData.WormholeData.GetRot();
            Wormhole.transform.localScale = GalaxyData.WormholeData.GetLocalScale();

            //Debug.LogError("WE HAVE A SAVED WORMHOLE!!!!!!");          
        }

        for (int i = 0; i < data.StarSystems.Count; i++)
        {
            GameObject objectOriginal = StarSystemPrefab;
            GameObject star = Instantiate(objectOriginal, transform.position, Quaternion.identity, transform);


            star.transform.position = data.StarSystems[i].GetPos();
            star.transform.rotation = data.StarSystems[i].GetRot();
            star.transform.localScale = data.StarSystems[i].GetLocalScale();
            //star.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            //Debug.Log("Star system pos is " + star.transform.position);

            StarSystemOnFocus starSystem = star.GetComponentInChildren<StarSystemOnFocus>(true);
            //starSystem.transform.position = data.StarSystems[i].GetPos();
            //starSystem.transform.rotation = data.StarSystems[i].GetRot();
            //starSystem.transform.localScale = data.StarSystems[i].GetLocalScale();



            starSystem.Initialize(data.StarSystems[i], this);
        }

        StarSystems = GetComponentsInChildren<StarOnWorldMap>(true);

        // Debug.LogError("NOW WOULD BE A GOOD TIME AND PLACE TO GENERATE A GALAXY");
    }
}
