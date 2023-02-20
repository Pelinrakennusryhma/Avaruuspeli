using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseController : MonoBehaviour
{
    public static UniverseController Instance;

    public GalaxyOnWorldMap[] AllGalaxies;

    public Material LineRendererMat;

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

        if (!TryLoadUniverse())
        {
            GenerateNewUniverse();
        }

        AllGalaxies = GetComponentsInChildren<GalaxyOnWorldMap>(true);
        ShowGalaxies();
    }

    public bool TryLoadUniverse()
    {
        bool success = false;

        Debug.Log("Trying to load universe");

        if (success)
        {
            Debug.Log("Universe load succesful");
            return true;
        }

        else
        {
            Debug.Log("Universe load failed");
            return false;
        }
    }

    public void GenerateNewUniverse()
    {
        Debug.Log("Generating new universe");
    }

    public void HideGalaxies()
    {
        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].gameObject.SetActive(false);
            AllGalaxies[i].SetLineRenderersInactive();
            Debug.LogError("Iterating galaxies " + i);
        }
    }

    public void HideGalaxyLineRenderers()
    {
        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].SetLineRenderersInactive();
            Debug.LogError("Iterating galaxies " + i);
        }
    }

    public void ShowGalaxies()
    {
        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].gameObject.SetActive(true);
            AllGalaxies[i].DrawLinesBetweenGalaxies(this);
        }

        Debug.Log("SHOW GALAXIES");
    }
}
