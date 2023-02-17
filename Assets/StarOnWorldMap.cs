using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarOnWorldMap : MonoBehaviour
{
    public WorldMapClickDetector ClickDetector;

    public MeshRenderer StarSystemMesh;

    public Collider StarSystemCollider;

    public GameObject Children;

    public void Awake()
    {
        ClickDetector = GetComponent<WorldMapClickDetector>();
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

    }

    public void OnZoomOutToStarSystem()
    {
        StarSystemMesh.enabled = true;
        StarSystemCollider.enabled = true;
        Children.SetActive(false);
        Debug.Log("On zoom out on star");
    }
}
