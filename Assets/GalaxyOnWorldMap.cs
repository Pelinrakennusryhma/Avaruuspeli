using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyOnWorldMap : MonoBehaviour
{
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

    public void Start()
    {
        if (!HasBeenInitted)
        {
            //Init();
        }

        //HideStarsAndShowGalaxy();
    }

    public void Init()
    {
        ClickDetector = GetComponent<WorldMapClickDetector>();
        ClickDetector.OnObjectClicked -= OnGalaxyClicked;
        ClickDetector.OnObjectClicked += OnGalaxyClicked;

        HasBeenInitted = true;        
        

    }

    public void DrawLinesBetweenGalaxies(UniverseController universe)
    {
        if (universe == null)
        {
            universe = FindObjectOfType<UniverseController>(true);
            universe.Init();
        }


        if (!HasBeenInitted)
        {
            Init();
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

        ShowStars();
        UniverseController.Instance.HideGalaxyLineRenderers();
    }

    public void ShowStars()
    {
        Debug.LogError("Now would be a good time to init stars");

        if (!HasBeenGenerated)
        {
            GenerateGalaxy();
        }

        for (int i = 0; i < StarSystems.Length; i++) 
        {
            StarSystems[i].gameObject.SetActive(true);
            StarSystems[i].DrawLinesBetweenStars(this);
        }

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

        GalaxyCollider.enabled = true;
        GalaxyMesh.enabled = true;
        DrawLinesBetweenGalaxies(UniverseController.Instance);
    }

    public void HideOtherStars(StarOnWorldMap toNotHide)
    {
        for (int i = 0; i < StarSystems.Length; i++)
        {
            if (StarSystems[i] != toNotHide) 
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

    public void GenerateGalaxy()
    {
        if (!HasBeenInitted)
        {
            Init();
        }

        bool successfulLoad = GameManager.Instance.SaverLoader.LoadGalaxy();

        for (int i = 0; i < PlaceHolderStarSystems.Length; i++)
        {
            DestroyImmediate(PlaceHolderStarSystems[i].gameObject);
        }

        if (!successfulLoad) 
        {
            int amountOfStars = Random.Range(6, 13);
            //amountOfStars = 12;
            float distance = 0.0f;

            int segments = Random.Range(0, 361);

            for (int i = 0; i < amountOfStars; i++)
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
            }

            GameManager.Instance.SaverLoader.SaveGalaxy();

        }

        StarSystems = GetComponentsInChildren<StarOnWorldMap>(true);

       // Debug.LogError("NOW WOULD BE A GOOD TIME AND PLACE TO GENERATE A GALAXY");

        HasBeenGenerated = true;
    }
}
