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

    public List<StarSystemData> StarSystems = new List<StarSystemData>();

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

        else
        {
            GenerateUniverseFromSaveFile();
            Debug.LogWarning("Do not generate a new universe. We have a valid save file");
        }

        AllGalaxies = GetComponentsInChildren<GalaxyOnWorldMap>(true);
        HasBeenInitted = true;

        //Debug.LogError("Initting universe");
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


        AllGalaxies = GetComponentsInChildren<GalaxyOnWorldMap>(true);

        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            GalaxyData galaxyData = new GalaxyData();
            galaxyData.ID = i + 1;
            galaxyData.SetPosRotAndScale(AllGalaxies[i].transform.position,
                                         AllGalaxies[i].transform.rotation,
                                         AllGalaxies[i].transform.localScale);

            AllGalaxies[i].Init(galaxyData);
        }

        Debug.Log("DECIDE HERE HOW WORMHOLES SHOULD BE SPAWNED?");

        int maximumAmountOfWormholePairs = amountOfGalaxies / 2;

        int amountOfWormholePairs = Random.Range(2, maximumAmountOfWormholePairs); 

        List<GalaxyOnWorldMap> galaxiesWithoutWormholes = new List<GalaxyOnWorldMap>();

        for(int i = 0; i< AllGalaxies.Length; i++)
        {
            galaxiesWithoutWormholes.Add(AllGalaxies[i]);
        }

        for (int i = 0; i < amountOfWormholePairs; i++)
        {                 
            //Debug.LogWarning("Iterating wormhole pairs");
       


            int rando1 = Random.Range(0, galaxiesWithoutWormholes.Count);
            int rando2;

            while(true)
            {
                bool foundUnique = false;

                rando2 = Random.Range(0, galaxiesWithoutWormholes.Count);

                if (rando2 != rando1)
                {
                    foundUnique = true;
                }

                if (foundUnique)
                {                
                    break;
                }

            }

            GalaxyOnWorldMap galaxy1 = galaxiesWithoutWormholes[rando1];
            GalaxyOnWorldMap galaxy2 = galaxiesWithoutWormholes[rando2];

            WormholeData data1 = new WormholeData();
            WormholeData data2 = new WormholeData();

            data1.ID = 1;
            data1.GalaxyId = galaxy1.GalaxyData.ID;


            data2.ID = 2;
            data2.GalaxyId = galaxy2.GalaxyData.ID;
           
            
            data1.PairWormhole = data2;
            data1.PairWormholeGalaxyID = data2.GalaxyId;            
            data2.PairWormhole = data1;
            data2.PairWormholeGalaxyID = data1.GalaxyId; 

            galaxy1.GalaxyData.WormholeData = data1;
            galaxy2.GalaxyData.WormholeData= data2;

            galaxiesWithoutWormholes.Remove(galaxy1);
            galaxiesWithoutWormholes.Remove(galaxy2);

            //Debug.Log("Rando1 is " + rando1 + " rando2 is " + rando2);
        }



        List<GalaxyData> allGalaxies = new List<GalaxyData>();

        for (int i = 0; i < AllGalaxies.Length; i++)
        {
            allGalaxies.Add(AllGalaxies[i].GalaxyData);
        }

        GameManager.Instance.SaverLoader.SaveUniverse(allGalaxies);

        //StarSystems = GetComponentsInChildren<StarOnWorldMap>(true);

        //Debug.LogError("NOW WOULD BE A GOOD TIME AND PLACE TO GENERATE A GALAXY");

        HasBeenGenerated = true;

        Debug.Log("Generating new universe");
    }

    public void GenerateUniverseFromSaveFile()
    {
        for (int i = 0; i < PlaceHolderGalaxies.Length; i++)
        {
            DestroyImmediate(PlaceHolderGalaxies[i].gameObject);
        }

        //Debug.LogWarning("We are here trying to recreate a previous universe");


        //Debug.LogWarning("MAKE SURE WORMHOLES ARE LOADED TOO!");

        List<GalaxyData> savedGalaxies = GameManager.Instance.SaverLoader.GetSavedGalaxyDatas();
        int amountOfGalaxies = savedGalaxies.Count;

        for (int i = 0; i < savedGalaxies.Count; i++)
        {
            //Vector3 pos = transform.position + Vector3.forward * distance;

            GameObject objectOriginal = GalaxyPrefab;

            GameObject galaxy = Instantiate(objectOriginal, 
                                            transform.position, 
                                            Quaternion.identity, 
                                            transform);

            galaxy.transform.localScale = savedGalaxies[i].GetLocalScale();

            galaxy.transform.position = savedGalaxies[i].GetPos();
            galaxy.transform.rotation = savedGalaxies[i].GetRot();
        }

        AllGalaxies = GetComponentsInChildren<GalaxyOnWorldMap>(true);

        if (savedGalaxies.Count != AllGalaxies.Length)
        {
            Debug.LogError("We have different length for two arrays, that should be the same");
        }

        for (int i = 0; i < AllGalaxies.Length; i++)
        {


            GalaxyData galaxyData = savedGalaxies[i];

            AllGalaxies[i].Init(galaxyData);
        }

        HasBeenGenerated = true;
    }
}
