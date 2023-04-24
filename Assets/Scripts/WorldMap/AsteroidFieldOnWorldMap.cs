using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldOnWorldMap : MonoBehaviour
{
    public AsteroidFieldData AsteroidFieldData;

    public StarSystemOnFocus ParentStarSystem;


    //public WorldMapClickDetector WorldMapClickDetector;

    public GameObject ReferenceObject;

    public GameObject Asteroid01aOriginal;
    public GameObject Asteroid01bOriginal;
    public GameObject Asteroid01cOriginal;
    public GameObject Asteroid01dOriginal;
    public GameObject Asteroid01eOriginal;
    public GameObject Asteroid01fOriginal;
    public GameObject Asteroid01gOriginal;

    public bool RocksHaveBeenSpawned;

    public void Awake()
    {
        //WorldMapClickDetector = GetComponent<WorldMapClickDetector>();
        //WorldMapClickDetector.OnObjectClicked -= OnAsteroidFieldClicked;
        //WorldMapClickDetector.OnObjectClicked += OnAsteroidFieldClicked;

    }

    public void SetAsteroidFieldData(AsteroidFieldData data,
                                     StarSystemOnFocus parentStarSystem)
    {
        AsteroidFieldData = data;
        ParentStarSystem = parentStarSystem;
    }

    public void OnAsteroidFieldClicked(WorldMapClickDetector.ClickableObjectType objectType)
    {
        GameManager.Instance.CurrentAsteroidField = this;
        GameManager.Instance.CurrentAsteroidFieldData = AsteroidFieldData;
        //Debug.LogError("Clicked an asteroid field collider");
    }

    public void SpawnRocks()
    {
        if (!RocksHaveBeenSpawned)
        {
            DoTheInstantation(-0.01f, -0.01f, 0.01f, 0.01f);
            DoTheInstantation(4.25f, -0.005f, -0.001f, - 4.25f);
            DoTheInstantation(-2.25f, 0.005f, 0.001f, 2.25f);
        }

        RocksHaveBeenSpawned = true;

        //Debug.LogError("SPAWN ROCKS " + Time.time);
    }

    public void DoTheInstantation(float toAddMin,
                                  float minusRadius,
                                  float plusRadius,
                                  float toAddMax)
    {
        float radius = (transform.position - ReferenceObject.transform.position).magnitude;

        //Debug.Log("Rock radius is " + radius );

        int amountOfRocks = 36;

        if (radius >= 4.5f)
        {
            amountOfRocks = 72;
        }

        else if (radius > 3.0f)
        {
            amountOfRocks = 48;
        }

        GameObject[] rocks = new GameObject[amountOfRocks];

        for (int i = 0; i < rocks.Length; i++)
        {
            GameObject original = Asteroid01aOriginal;

            //int seed = i;

            int seed = Random.Range(0, 361);

            if (seed % 7 == 1) 
            {
                original = Asteroid01aOriginal;
            }

            else if (seed % 7 == 2)
            {
                original = Asteroid01bOriginal;
            }

            else if (seed % 7 == 3)
            {
                original = Asteroid01cOriginal;
            }

            else if (seed % 7 == 4)
            {
                original = Asteroid01dOriginal;
            }

            else if (seed % 7 == 5)
            {
                original = Asteroid01eOriginal;
            }

            else if (seed % 7 == 6)
            {
                original = Asteroid01fOriginal;
            }

            else if (seed % 7 == 7)
            {
                original = Asteroid01gOriginal;
            }

            rocks[i] = GameObject.Instantiate(original, transform);           
            rocks[i].gameObject.SetActive(true);


            //rocks[i].transform.position = transform.position;
            //rocks[i].transform.Rotate(Vector3.up, i * 10);
            //rocks[i].transform.position = -rocks[i].transform.forward * radius;

            radius += Random.Range(minusRadius, plusRadius);

            float rad = Mathf.Deg2Rad * (i * 360f / ((float) amountOfRocks + Random.Range(toAddMin, toAddMax)));
            rocks[i].transform.position = transform.position + new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);
            Vector3 randomRot = new Vector3(Random.Range(0f, 0f),
                                            Random.Range(-1.0f, 1.0f),
                                            Random.Range(0f, 0f));

            float amount = Random.Range(0, 180);
            rocks[i].transform.Rotate(randomRot, amount, Space.World);

            //Debug.Log("Instantiated");
        }

        //Debug.Log("Rock radius is " + radius);
    }
}
