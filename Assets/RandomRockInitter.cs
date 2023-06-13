using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockInitter : MonoBehaviour
{
    public bool AlwaysSpawn = false;
    public Resource ResourceType;
    private DestroyableRock Rock;

    public Resource[] SpawnableResources;

    public void Start()
    {
        Rock = GetComponent<DestroyableRock>();

        bool spawnIce = false;

        if (GameManager.Instance.CurrentPlanet != null)
        {
            PlanetData.PlanetGraphicsType planetType = GameManager.Instance.CurrentPlanet.PlanetData.PlanetGraphics;

            if (planetType == PlanetData.PlanetGraphicsType.Placeholder2
                || planetType == PlanetData.PlanetGraphicsType.Placeholder8
                || planetType == PlanetData.PlanetGraphicsType.Placeholder9
                || planetType == PlanetData.PlanetGraphicsType.Placeholder16
                || planetType == PlanetData.PlanetGraphicsType.Placeholder22
                || planetType == PlanetData.PlanetGraphicsType.Placeholder24
                || planetType == PlanetData.PlanetGraphicsType.Placeholder26
                || planetType == PlanetData.PlanetGraphicsType.Placeholder27
                || planetType == PlanetData.PlanetGraphicsType.Placeholder30) 
            {
                float iceRandom = Random.Range(0, 1.0f);

                if (iceRandom <= 0.3f)
                {
                    spawnIce = true;
                }
            }
        }

        else
        {
            string sceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

            if (sceneName.Equals("Marssi")
                || sceneName.Equals("LumiMaa"))
            {
                float iceRandom = Random.Range(0, 1.0f);

                Debug.Log("Ice random is " + iceRandom);

                if (iceRandom <= 0.3f)
                {
                    spawnIce = true;
                }
            }

        }

        if (!spawnIce)
        {
            float randomValue = Random.Range(0.0f, 1.0f);

            if (randomValue < 0.05f)
            {
                ResourceType = SpawnableResources[0];
            }

            else if (randomValue < 0.10f)
            {
                ResourceType = SpawnableResources[1];
            }

            else if (randomValue < 0.15f)
            {
                ResourceType = SpawnableResources[2];
            }

            else if (randomValue < 0.20f)
            {
                ResourceType = SpawnableResources[3];
            }

            else if (randomValue < 0.25f)
            {
                ResourceType = SpawnableResources[4];
            }

            else if (randomValue < 0.35f)
            {
                ResourceType = SpawnableResources[5];
            }

            else if (randomValue < 0.45f)
            {
                ResourceType = SpawnableResources[6];
            }

            else if (randomValue < 0.55f)
            {
                ResourceType = SpawnableResources[7];
            }

            else if (randomValue < 0.75f)
            {
                ResourceType = SpawnableResources[8];
            }

            else
            {
                ResourceType = SpawnableResources[9];
            }
        }

        else
        {
            ResourceType = SpawnableResources[10];
            //Debug.Log("Should see ice out there");
        }

        Rock.Init(ResourceType, null);

        float spawnAtAll = Random.Range(0.0f, 1.0f);

        //spawnAtAll = 0.0f;

        if (spawnAtAll <= 0.33f
            && !AlwaysSpawn)
        {
            Destroy(gameObject);
            //Debug.Log("Destroyed rock at start");
        }
    }

}
