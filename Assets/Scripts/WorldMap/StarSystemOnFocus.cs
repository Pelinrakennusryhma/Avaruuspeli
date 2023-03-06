using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemOnFocus : MonoBehaviour
{
    public GalaxyOnWorldMap ParentGalaxy;

    public StarSystemData StarSystemData;

    public CenterStarOnWorldMap CenterStar;

    public PlanetOnWorldMap[] Planets;

    public AsteroidFieldOnWorldMap[] AsteroidFields;

    public bool HasBeenInitted;

    public bool HasBeenGenerated;

    public GameObject[] PlanetPlaceHolders;
    public AsteroidFieldOnWorldMap PlaceHolderAsteroidField;


    public GameObject Planet1Original;
    public GameObject Planet2Original;
    public GameObject Planet3Original;
    public GameObject Planet4Original;
    public GameObject Planet5Original;
    public GameObject Planet6Original;
    public GameObject Planet7Original;
    public GameObject Planet8Original;
    public GameObject Planet9Original;
    public GameObject Planet10Original;

    public GameObject AsteroidFieldOriginal;


    public void Awake()
    {
        if (!HasBeenInitted)
        {
            // Don't do this on awake yet, because in that case we would generate all the planets in the universe at once
            // This might take too much time. Or maybe not, don't know yet.
            //Initialize();
        }
    }

    public void Initialize(StarSystemData data,
                           GalaxyOnWorldMap parentgalaxy)
    {
        ParentGalaxy = parentgalaxy;

        StarSystemData = data;

        // Maybe a little shitty solution
        for (int i = 0; i < PlanetPlaceHolders.Length; i++)
        {
            DestroyImmediate(PlanetPlaceHolders[i].gameObject);
        }

        //DestroyImmediate(PlaceHolderAsteroidField.gameObject);

        PlaceHolderAsteroidField.gameObject.SetActive(false);

        HasBeenGenerated = TryLoadStarSystem(out data);

        bool isLoaded = false;

        if (HasBeenGenerated)
        {
            isLoaded = true;
        }

        if (!HasBeenGenerated)
        {
            GenerateNewStarSystem();
            HasBeenGenerated = true;
        }

        Planets = GetComponentsInChildren<PlanetOnWorldMap>(true);
        AsteroidFields = GetComponentsInChildren<AsteroidFieldOnWorldMap>(true);

        for (int i = 0; i < Planets.Length; i++)
        {
            //PlanetData planetData = null;

            //for (int j =0; j < StarSystemData.Planets.Count; j++)
            //{
            //    if (StarSystemData[])
            //    {

            //    }
            //}

            Planets[i].Init(CenterStar, this);
            Planets[i].SetInitialStartingPosition(!isLoaded);

            //if (isLoaded 
            //    && planetData != null)
            //{
            //    Planets[i].SetPlanetData(planetData);
            //}
        }

        for(int i = 0; i < AsteroidFields.Length; i++)
        {
            AsteroidFields[i].SpawnRocks();
        }

        HasBeenInitted = true;
    }

    public void OnBecomeFocused()
    {
        GameManager.Instance.CurrentStarSystem = this;
        GameManager.Instance.CurrentStarSystemData = StarSystemData;

        if (!HasBeenInitted)
        {
            Debug.LogError("JUST IN CASE INITIALIZATION REMOVED FROM HERE. MAKE SURE THINGS ACTUALLY WORK");
            //Initialize();
        }


        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].DrawOrbit();
        }
    }

    public void OnBecomeUnfocused()
    {
        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].HideOrbit();
        }
    }

    public void GenerateNewStarSystem()
    {
        //Debug.LogError("Generating star system");

        // Instantiate proper prefabs
        // Currently this spawns only a random planet

        int amountOfOrbits = Random.Range(5, 13);

        int amountOfAsteroidFields = Random.Range(1, 3);

       // Debug.LogError("HOW ABOUT STAR SCALE? THAT SHOULD AFFECT THE STARTNG ORBIT");


        // Scale Center star

        float centerStarLocalScale = Random.Range(1.0f, 3.0f);
        //centerStarLocalScale = 3.0f;
        centerStarLocalScale *= 0.2f;

        CenterStar.transform.localScale = new Vector3(centerStarLocalScale,
                                                      centerStarLocalScale,
                                                      centerStarLocalScale);


        CenterStarData starData = new CenterStarData();
        starData.SetPosRotAndScale(CenterStar.transform.position,
                                   CenterStar.transform.rotation,
                                   CenterStar.transform.localScale);


        StarSystemData.CenterStar = starData;

        float orbit = Random.Range(0.6f, 0.8f);

        if (centerStarLocalScale >= 2.0f)
        {
            orbit = 1.4f;
        }

        else if (centerStarLocalScale >= 1.5f)
        {
            orbit = 0.8f;
        }

        //orbit = 0.4f;

        bool closeAsteroidFieldHasBeenSpawned = false;
        bool farAsteroidFieldHasBeenSpawned = false;

        int amountOfIterationsSinceAsteroidSpawn = 0;

        for (int i = 0; i < amountOfOrbits; i++)
        {
            int randomPlanet;

            if (amountOfIterationsSinceAsteroidSpawn >= 4) 
            {
                 randomPlanet = Random.Range(0, 11);
            }

            else
            {
                randomPlanet = Random.Range(1, 11);
            }

            if (randomPlanet == 0)
            {
                //Debug.Log("Asteroid Spawn should happen on iteration " + i + " has passed " + amountOfIterationsSinceAsteroidSpawn);                
                amountOfIterationsSinceAsteroidSpawn = 0;

                if (i <= amountOfOrbits / 2)
                {
                    closeAsteroidFieldHasBeenSpawned = true;
                }

                else
                {
                    farAsteroidFieldHasBeenSpawned = true;
                }
            }

            amountOfIterationsSinceAsteroidSpawn++;





            if (i >= amountOfOrbits / 2
                && !closeAsteroidFieldHasBeenSpawned
                && amountOfIterationsSinceAsteroidSpawn >= 2)
            {
                randomPlanet = 0;
                closeAsteroidFieldHasBeenSpawned = true;
                amountOfIterationsSinceAsteroidSpawn = 0;
                //Debug.LogWarning("Should spawn close asteroid field. Iteration is" + i);
            }

            if (amountOfAsteroidFields >= 2
                && !farAsteroidFieldHasBeenSpawned
                && i >= amountOfOrbits - 1
                && amountOfIterationsSinceAsteroidSpawn >= 1)
            {
                randomPlanet = 0;
                farAsteroidFieldHasBeenSpawned = true;
                amountOfIterationsSinceAsteroidSpawn = 0;
                //Debug.LogWarning("Should spawn far asteroid field. Iteration is" + i);
            }


            GameObject objectOriginal;
            bool isAsteroidField = false;

            PlanetData.PlanetGraphicsType planetGraphicsType = PlanetData.PlanetGraphicsType.None;

            if (randomPlanet == 1)
            {
                objectOriginal = Planet1Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder1;

            }

            else if (randomPlanet == 2)
            {
                objectOriginal = Planet2Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder2;
            }

            else if (randomPlanet == 3)
            {
                objectOriginal = Planet3Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder3;
            }

            else if (randomPlanet == 4)
            {
                objectOriginal = Planet4Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder4;
            }

            else if (randomPlanet == 5)
            {
                objectOriginal = Planet5Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder5;
            }

            else if (randomPlanet == 6)
            {
                objectOriginal = Planet6Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder6;
            }

            else if (randomPlanet == 7)
            {
                objectOriginal = Planet7Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder7;
            }

            else if (randomPlanet == 8)
            {
                objectOriginal = Planet8Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder8;
            }

            else if (randomPlanet == 9)
            {
                objectOriginal = Planet9Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder9;
            }

            else if (randomPlanet == 10)
            {
                objectOriginal = Planet10Original;
                planetGraphicsType = PlanetData.PlanetGraphicsType.Placeholder10;
            }

            else
            {
                objectOriginal = AsteroidFieldOriginal;
                isAsteroidField = true;
            }

            GameObject planet = Instantiate(objectOriginal, transform.position, Quaternion.identity, transform);

            bool aBigPlanet = false;

            if (!isAsteroidField) 
            {
                planet.transform.position = transform.position + -planet.transform.forward * orbit;

                float randomScale = 1.0f;

                if (i > 2) 
                {
                    randomScale = Random.Range(1.0f, 3.4f);

                    if (randomScale > 2.7f) 
                    {
                        aBigPlanet = true;
                    }

                }


                // Can't use bigger scales until the ship is rendered on top of everything!
                //randomScale = 1.0f;

                planet.transform.localScale = new Vector3(planet.transform.localScale.x * randomScale,
                                                         planet.transform.localScale.y * randomScale,
                                                         planet.transform.localScale.z * randomScale);

                PlanetOnWorldMap planetComponent = planet.GetComponent<PlanetOnWorldMap>();
                PlanetData data = new PlanetData();
                data.ID = i + 1;
                data.SetPosRotAndScale(planet.transform.position,
                                       planet.transform.rotation,
                                       planet.transform.localScale);

                //Debug.LogWarning("Pos and rot is not saved at correct pos");

                data.Orbit = orbit;
                data.PlanetGraphics = planetGraphicsType;
                planetComponent.SetPlanetData(data);
                StarSystemData.Planets.Add(data);

            }

            else
            {
                AsteroidFieldOnWorldMap asteroidField = planet.GetComponent<AsteroidFieldOnWorldMap>();
                asteroidField.ReferenceObject.transform.position = transform.position + new Vector3(0, 0, orbit);

                AsteroidFieldData data = new AsteroidFieldData();
                data.ID = i + 1;
                data.SetPosRotAndScale(asteroidField.transform.position,
                                       asteroidField.transform.rotation,
                                       asteroidField.transform.localScale);
                data.SetReferenceObjectPos(asteroidField.ReferenceObject.transform.position);

                data.Orbit = orbit;

                asteroidField.SetAsteroidFieldData(data, this);

                StarSystemData.Asteroids.Add(data);

            }

            if (!aBigPlanet) 
            {
                orbit += Random.Range(0.25f, 0.4f);
            }

            else
            {
                orbit += 0.4f;
            }
            //Debug.Log("Instantiated a planet " + objectOriginal.name);
        }

        // Set starting orbits and scales
        // Set planet datas
        // Something else?

        SaveStarSystem();
    }

    public bool TryLoadStarSystem(out StarSystemData data)
    {
        data = null;
        bool hasBeenLoaded = GameManager.Instance.SaverLoader.LoadStarSystem(ParentGalaxy.GalaxyData.ID,
                                                                             StarSystemData.ID,
                                                                             out data);

        if (hasBeenLoaded 
            && data != null)
        {
            RegenerateStarSystemFromdata(data);
        }

        //Debug.Log("Trying to load star system. Success: " + hasBeenLoaded);

        return hasBeenLoaded;
    }

    public void RegenerateStarSystemFromdata(StarSystemData data)
    {

        StarSystemData = data;
 

        CenterStarData starData = new CenterStarData();

        if (data == null)
        {
            Debug.LogError("Null data");
        }

        if (CenterStar == null)
        {
            Debug.Log("Null center star");
        }

        if (data.CenterStar == null)
        {
            Debug.LogError("Center star data is all null");
        }

        CenterStar.transform.position = StarSystemData.CenterStar.GetPos();
        CenterStar.transform.rotation = StarSystemData.CenterStar.GetRot();
        CenterStar.transform.localScale = StarSystemData.CenterStar.GetLocalScale();

        for (int i = 0; i < StarSystemData.Planets.Count; i++)
        {
            GameObject objectOriginal = null;

            switch (StarSystemData.Planets[i].PlanetGraphics)
            {
                case PlanetData.PlanetGraphicsType.None:
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder1:
                    objectOriginal = Planet1Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder2:
                    objectOriginal = Planet2Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder3:
                    objectOriginal = Planet3Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder4:
                    objectOriginal = Planet4Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder5:
                    objectOriginal = Planet5Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder6:
                    objectOriginal = Planet6Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder7:
                    objectOriginal = Planet7Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder8:
                    objectOriginal = Planet8Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder9:
                    objectOriginal = Planet9Original;
                    break;
                case PlanetData.PlanetGraphicsType.Placeholder10:
                    objectOriginal = Planet10Original;
                    break;
                default:
                    break;
            }

            if (objectOriginal == null)
            {
                Debug.LogError("Can't spawn a planet, because object original is null");
            }

            else
            {
                //Debug.LogWarning("Can spawn a planet, because object original is not null");

                GameObject planet = Instantiate(objectOriginal, transform.position, Quaternion.identity, transform);

                PlanetOnWorldMap planetComponent = planet.GetComponent<PlanetOnWorldMap>();
                planetComponent.SetPlanetData(StarSystemData.Planets[i]);

                planet.transform.position = StarSystemData.Planets[i].GetPos();
                planet.transform.rotation = StarSystemData.Planets[i].GetRot();
                planet.transform.localScale = StarSystemData.Planets[i].GetLocalScale();


            }
        }

        for (int i = 0; i < StarSystemData.Asteroids.Count; i++)
        {
            GameObject objectOriginal = AsteroidFieldOriginal;
            GameObject asteroidField = Instantiate(objectOriginal, transform.position, Quaternion.identity, transform);

            AsteroidFieldOnWorldMap asteroidFieldComponent = asteroidField.GetComponent<AsteroidFieldOnWorldMap>();

            asteroidFieldComponent.AsteroidFieldData = StarSystemData.Asteroids[i];

            Vector3 savedReferenceObjectPos = new Vector3(StarSystemData.Asteroids[i].ReferenceObjectPosX,
                                                          StarSystemData.Asteroids[i].ReferenceObjectPosY,
                                                          StarSystemData.Asteroids[i].ReferenceObjectPosZ);

            asteroidFieldComponent.ReferenceObject.transform.position = savedReferenceObjectPos;
        }
    }

    public void SaveStarSystem()
    {
        GameManager.Instance.SaverLoader.SaveStarSystem(ParentGalaxy.GalaxyData.ID,
                                                        StarSystemData.ID,
                                                        StarSystemData.CenterStar,
                                                        StarSystemData.Planets,
                                                        StarSystemData.Asteroids,
                                                        StarSystemData.GetPos(),
                                                        StarSystemData.GetRot(),
                                                        StarSystemData.GetLocalScale(),
                                                        StarSystemData);
        //Debug.Log("Now it would be a good time and place to save a star system");
    }
}
