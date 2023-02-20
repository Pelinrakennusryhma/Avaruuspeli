using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarOnWorldMap : MonoBehaviour
{
    public WorldMapClickDetector ClickDetector;

    public MeshRenderer StarSystemMesh;

    public Collider StarSystemCollider;

    public GameObject Children;

    public StarSystemOnFocus StarSystemOnFocus;

    public GalaxyOnWorldMap OwnerGalaxy;

    public LineRenderer[] LinesBetweenStarSystems;

    public bool LinesHaveBeenCreated;

    public void Awake()
    {
        ClickDetector = GetComponent<WorldMapClickDetector>();
        StarSystemOnFocus = GetComponentInChildren<StarSystemOnFocus>(true);
        ClickDetector.OnObjectClicked -= OnStarClicked;
        ClickDetector.OnObjectClicked += OnStarClicked;
        Children.SetActive(false);
    }

    public void OnStarClicked(WorldMapClickDetector.ClickableObjectType type)
    {
        Debug.Log("Clicked star");
        //WorldMapMouseController.Instance.SetZoomOrigin(transform.position);
        WorldMapMouseController.Instance.ZoomIn(transform.position,
                                                WorldMapMouseController.ZoomLevel.StarSystem,
                                                WorldMapMouseController.Instance.CurrentGalaxy,
                                                this);

        StarSystemMesh.enabled = false;
        StarSystemCollider.enabled = false;
        Children.SetActive(true);
        WorldMapMouseController.Instance.CurrentGalaxy.HideOtherStars(WorldMapMouseController.Instance.CurrentStarSystem);
        StarSystemOnFocus.OnBecomeFocused();
        OwnerGalaxy.HideStarLineRenderers();
    }

    public void OnZoomOutToStarSystems()
    {
        StarSystemMesh.enabled = true;
        StarSystemCollider.enabled = true;
        Children.SetActive(false);

        WorldMapMouseController.Instance.CurrentGalaxy.ShowStars();
        StarSystemOnFocus.OnBecomeUnfocused();

        Debug.Log("On zoom out on star");
    }

    public void DrawLinesBetweenStars(GalaxyOnWorldMap galaxy)
    {
        OwnerGalaxy = galaxy;

        if (!LinesHaveBeenCreated)
        {
            //Create lines

            for (int i = 0; i < OwnerGalaxy.StarSystems.Length; i++)
            {
                GameObject newObject = new GameObject();
                newObject.transform.parent = gameObject.transform;
                newObject.AddComponent<LineRenderer>();
            }

            LinesBetweenStarSystems = GetComponentsInChildren<LineRenderer>();
        }

        for (int i = 0; i < LinesBetweenStarSystems.Length; i++)
        {
            LinesBetweenStarSystems[i].enabled = true;
            LinesBetweenStarSystems[i].loop = false;
            LinesBetweenStarSystems[i].useWorldSpace = true;
            LinesBetweenStarSystems[i].startWidth = 0.04f;
            LinesBetweenStarSystems[i].endWidth = 0.04f;
            LinesBetweenStarSystems[i].material = UniverseController.Instance.LineRendererMat;
        }

        Vector3 startPoint = transform.position;

        for (int i = 0; i < OwnerGalaxy.StarSystems.Length; i++)
        {
            Vector3 endPoint = OwnerGalaxy.StarSystems[i].transform.position;

            LinesBetweenStarSystems[i].SetPosition(0, startPoint);
            LinesBetweenStarSystems[i].SetPosition(1, endPoint);
        }
    }

    public void HideLinesBetweenStars()
    {
        for (int i = 0; i < LinesBetweenStarSystems.Length; i++)
        {
            LinesBetweenStarSystems[i].gameObject.SetActive(false);
        }
    }
}
