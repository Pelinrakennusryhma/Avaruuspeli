using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarSystemOnFocus : MonoBehaviour
{
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

    private void Initialize()
    {
        // Maybe a little shitty solution
        for (int i = 0; i < PlanetPlaceHolders.Length; i++)
        {
            DestroyImmediate(PlanetPlaceHolders[i].gameObject);
        }

        //DestroyImmediate(PlaceHolderAsteroidField.gameObject);

        PlaceHolderAsteroidField.gameObject.SetActive(false);

        HasBeenGenerated = TryLoadStarSystem();

        if (!HasBeenGenerated)
        {
            GenerateStarSystem();
            HasBeenGenerated = true;
        }

        Planets = GetComponentsInChildren<PlanetOnWorldMap>(true);
        AsteroidFields = GetComponentsInChildren<AsteroidFieldOnWorldMap>(true);

        for (int i = 0; i < Planets.Length; i++)
        {
            Planets[i].Init(CenterStar);
            Planets[i].SetInitialStartingPosition();
        }

        for(int i = 0; i < AsteroidFields.Length; i++)
        {
            AsteroidFields[i].SpawnRocks();
        }

        HasBeenInitted = true;
    }

    public void OnBecomeFocused()
    {
        if (!HasBeenInitted)
        {
            Initialize();
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

    public void GenerateStarSystem()
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

            if (randomPlanet == 1)
            {
                objectOriginal = Planet1Original;
            }

            else if (randomPlanet == 2)
            {
                objectOriginal = Planet2Original;
            }

            else if (randomPlanet == 3)
            {
                objectOriginal = Planet3Original;
            }

            else if (randomPlanet == 4)
            {
                objectOriginal = Planet4Original;
            }

            else if (randomPlanet == 5)
            {
                objectOriginal = Planet5Original;
            }

            else if (randomPlanet == 6)
            {
                objectOriginal = Planet6Original;
            }

            else if (randomPlanet == 7)
            {
                objectOriginal = Planet7Original;
            }

            else if (randomPlanet == 8)
            {
                objectOriginal = Planet8Original;
            }

            else if (randomPlanet == 9)
            {
                objectOriginal = Planet9Original;
            }

            else if (randomPlanet == 10)
            {
                objectOriginal = Planet10Original;
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
            }

            else
            {
                planet.GetComponent<AsteroidFieldOnWorldMap>().ReferenceObject.transform.position = transform.position + new Vector3(0, 0, orbit);
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

    public bool TryLoadStarSystem()
    {
        bool hasBeenLoaded = GameManager.Instance.SaverLoader.LoadStarSystem();

        //Debug.Log("Trying to load star system. Success: " + hasBeenLoaded);

        return hasBeenLoaded;
    }

    public void SaveStarSystem()
    {
        GameManager.Instance.SaverLoader.SaveStarSystem();
        //Debug.Log("Now it would be a good time and place to save a star system");
    }
}
