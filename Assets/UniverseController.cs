using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseController : MonoBehaviour
{
    public static UniverseController Instance;

    public GalaxyOnWorldMap[] AllGalaxies;

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
        }
    }

    public void ShowGalaxies()
    {
        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].gameObject.SetActive(true);
        }
    }
}
