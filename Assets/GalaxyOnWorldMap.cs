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

    public void Awake()
    {
        if (!HasBeenInitted) 
        {
            Init();
        }

        HideStarsAndShowGalaxy();
    }

    private void Init()
    {
        ClickDetector = GetComponent<WorldMapClickDetector>();
        ClickDetector.OnObjectClicked -= OnGalaxyClicked;
        ClickDetector.OnObjectClicked += OnGalaxyClicked;

        StarSystems = GetComponentsInChildren<StarOnWorldMap>(true);


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

        Debug.LogError("Setting line renderers inactive. Array length is " + LinesToOtherGalaxies.Length);
    }

    public void HideStarLineRenderers()
    {
        for (int i = 0; i < StarSystems.Length; i++)
        {
            StarSystems[i].HideLinesBetweenStars();
        }
    }
}
