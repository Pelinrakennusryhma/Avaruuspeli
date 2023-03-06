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

    public AsteroidFieldOnWorldMap[] AsteroidFields;

    public void Awake()
    {
        ClickDetector = GetComponent<WorldMapClickDetector>();
        StarSystemOnFocus = GetComponentInChildren<StarSystemOnFocus>(true);
        ClickDetector.OnObjectClicked -= OnStarClicked;
        ClickDetector.OnObjectClicked += OnStarClicked;
        Children.SetActive(false);
        AsteroidFields = GetComponentsInChildren<AsteroidFieldOnWorldMap>(true);
    }

    public void OnStarClicked(WorldMapClickDetector.ClickableObjectType type)
    {
        //Debug.Log("Clicked star");
        //WorldMapMouseController.Instance.SetZoomOrigin(transform.position);
        WorldMapMouseController.Instance.ZoomIn(transform.position,
                                                WorldMapMouseController.ZoomLevel.StarSystem,
                                                WorldMapMouseController.Instance.CurrentGalaxy,
                                                this);

        StarSystemMesh.enabled = false;
        StarSystemCollider.enabled = false;
        Children.SetActive(true);

        if (WorldMapMouseController.Instance.CurrentStarSystem == null)
        {
            Debug.Log("Null current star system");
        }

        if (WorldMapMouseController.Instance.CurrentGalaxy == null)
        {
            Debug.Log("Null galaxy");
        }

        WorldMapMouseController.Instance.CurrentGalaxy.HideOtherStars(WorldMapMouseController.Instance.CurrentStarSystem);
        StarSystemOnFocus.OnBecomeFocused();
        OwnerGalaxy.HideStarLineRenderers();
        OwnerGalaxy.HideWormhole();

        for (int i  = 0; i < AsteroidFields.Length; i++)
        {
            AsteroidFields[i].SpawnRocks();
        }
    }

    public void OnZoomOutToStarSystems()
    {
        StarSystemMesh.enabled = true;
        StarSystemCollider.enabled = true;
        Children.SetActive(false);

        WorldMapMouseController.Instance.CurrentGalaxy.ShowStars();
        StarSystemOnFocus.OnBecomeUnfocused();

        //Debug.Log("On zoom out on star");
    }

    public void DrawLinesBetweenStars(GalaxyOnWorldMap galaxy)
    {
        OwnerGalaxy = galaxy;

        if (!LinesHaveBeenCreated)
        {
            //Create lines

            for (int i = 0; i < OwnerGalaxy.StarSystems.Length + 1; i++)
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
            LinesBetweenStarSystems[i].startWidth = 0.02f;
            LinesBetweenStarSystems[i].endWidth = 0.02f;
            LinesBetweenStarSystems[i].material = UniverseController.Instance.LineRendererMat;
        }

        Vector3 startPoint = transform.position;

        for (int i = 0; i < OwnerGalaxy.StarSystems.Length; i++)
        {
            Vector3 endPoint = OwnerGalaxy.StarSystems[i].transform.position;

            LinesBetweenStarSystems[i].SetPosition(0, startPoint);
            LinesBetweenStarSystems[i].SetPosition(1, endPoint);
        }

        // Draw lines to worm hole too
        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].enabled = true;
        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].loop = false;
        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].useWorldSpace = true;
        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].startWidth = 0.02f;
        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].endWidth = 0.02f;
        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].material = UniverseController.Instance.LineRendererMat;

        startPoint = transform.position;
        Vector3 endPos = OwnerGalaxy.Wormhole.transform.position;

        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].SetPosition(0, startPoint);
        LinesBetweenStarSystems[LinesBetweenStarSystems.Length - 1].SetPosition(1, endPos);

    }

    public void HideLinesBetweenStars()
    {
        for (int i = 0; i < LinesBetweenStarSystems.Length; i++)
        {
            LinesBetweenStarSystems[i].gameObject.SetActive(false);
        }
    }
}
