using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GalaxyOnWorldMap : MonoBehaviour
{
    public WorldMapClickDetector ClickDetector;

    public StarOnWorldMap[] StarSystems;

    public MeshRenderer GalaxyMesh;
    public Collider GalaxyCollider;


    public void Awake()
    {
        ClickDetector = GetComponent<WorldMapClickDetector>();
        ClickDetector.OnObjectClicked -= OnGalaxyClicked;
        ClickDetector.OnObjectClicked += OnGalaxyClicked;

        StarSystems = GetComponentsInChildren<StarOnWorldMap>(true);

        HideStars();

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
    }

    public void ShowStars()
    {
        for (int i = 0; i < StarSystems.Length; i++) 
        {
            StarSystems[i].gameObject.SetActive(true);
        }

        GalaxyCollider.enabled = false;
        GalaxyMesh.enabled = false;
        Debug.LogError("Show stars");
    }

    public void HideStars()
    {
        for (int i = 0; i < StarSystems.Length; i++)
        {
            StarSystems[i].gameObject.SetActive(false);
        }

        GalaxyCollider.enabled = true;
        GalaxyMesh.enabled = true;
    }

    public void OnZoomOutToGalaxy()
    {
        HideStars();
    }
}
