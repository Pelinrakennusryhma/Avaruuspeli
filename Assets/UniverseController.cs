using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseController : MonoBehaviour
{
    public static UniverseController Instance;

    public GalaxyOnWorldMap[] AllGalaxies;

    public Material LineRendererMat;
    public Material PlanetOrbitMaterial;

    public bool HasBeenInitted;

    public GameObject GalaxyPrefab;

    public GameObject[] PlaceHolderGalaxies;

    public bool HasBeenGenerated;

    public void Start()
    {
        if (!HasBeenInitted) 
        {
            Init();
        }

        ShowGalaxies();
    }

    public void Init()
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
            if (!HasBeenGenerated) 
            {
                GenerateNewUniverse();
            }
        }

        AllGalaxies = GetComponentsInChildren<GalaxyOnWorldMap>(true);
        HasBeenInitted = true;

        Debug.LogError("Initting universe");
    }

    public bool TryLoadUniverse()
    {
        bool success = GameManager.Instance.SaverLoader.LoadUniverse();

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

    public void HideGalaxies()
    {
        if (!HasBeenInitted)
        {
            Init();
        }

        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].gameObject.SetActive(false);
            AllGalaxies[i].SetLineRenderersInactive();
            //Debug.LogError("Iterating galaxies " + i);
        }
    }

    public void HideGalaxyLineRenderers()
    {
        if (!HasBeenInitted)
        {
            Init();
        }

        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].SetLineRenderersInactive();
            //Debug.LogError("Iterating galaxies " + i);
        }
    }

    public void ShowGalaxies()
    {
        if (!HasBeenInitted)
        {
            Init();
        }

        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].gameObject.SetActive(true);
            AllGalaxies[i].DrawLinesBetweenGalaxies(this);
        }

       // Debug.Log("SHOW GALAXIES");
    }

    public void GenerateNewUniverse()
    {

        for (int i = 0; i < PlaceHolderGalaxies.Length; i++)
        {
            DestroyImmediate(PlaceHolderGalaxies[i].gameObject);
        }

        int amountOfGalaxies = Random.Range(10, 10);
        //amountOfStars = 12;
        float distance = 0.0f;

        int segments = Random.Range(0, 361);

        for (int i = 0; i < amountOfGalaxies; i++)
        {
            //Vector3 pos = transform.position + Vector3.forward * distance;

            GameObject objectOriginal = GalaxyPrefab;
            GameObject galaxy = Instantiate(objectOriginal, transform.position, Quaternion.identity, transform);
            galaxy.transform.localScale = new Vector3(100.0f, 100.0f, 100.0f);
            //star.transform.position = pos;


            //float rad = Mathf.Deg2Rad * (360f / segments);
            float rad = Mathf.Deg2Rad * Random.Range(0, 360.0f);
            float radius = distance;
            galaxy.transform.position = transform.position + new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);

            distance += Random.Range(110.0f, 150.0f);
            segments += Random.Range(90, 125);
            //Debug.Log("Spawned a star " + i);
        }

        GameManager.Instance.SaverLoader.SaveUniverse();

        AllGalaxies = GetComponentsInChildren<GalaxyOnWorldMap>(true);
        
        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            AllGalaxies[i].Init();
        }

        //StarSystems = GetComponentsInChildren<StarOnWorldMap>(true);

        Debug.LogError("NOW WOULD BE A GOOD TIME AND PLACE TO GENERATE A GALAXY");

        HasBeenGenerated = true;

        Debug.Log("Generating new universe");
    }
}
