using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidFieldOnWorldMap : MonoBehaviour
{
    public WorldMapClickDetector WorldMapClickDetector;

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
        WorldMapClickDetector = GetComponent<WorldMapClickDetector>();
        WorldMapClickDetector.OnObjectClicked -= OnAsteroidFieldClicked;
        WorldMapClickDetector.OnObjectClicked += OnAsteroidFieldClicked;

    }

    public void OnAsteroidFieldClicked(WorldMapClickDetector.ClickableObjectType objectType)
    {
        Debug.LogError("Clicked an asteroid field collider");
    }

    public void SpawnRocks()
    {
        if (!RocksHaveBeenSpawned)
        {
            DoTheInstantation();
        }

        RocksHaveBeenSpawned = true;

        Debug.LogError("SPAWN ROCKS " + Time.time);
    }

    public void DoTheInstantation()
    {
        float radius = (transform.position - ReferenceObject.transform.position).magnitude;

        GameObject[] rocks = new GameObject[36];

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

            float rad = Mathf.Deg2Rad * (i * 360f / 36);
            rocks[i].transform.position = transform.position + new Vector3(Mathf.Sin(rad) * radius, 0, Mathf.Cos(rad) * radius);

            //Debug.Log("Instantiated");
        }

        //Debug.Log("Rock radius is " + radius);
    }
}
