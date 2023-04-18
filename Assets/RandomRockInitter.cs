using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRockInitter : MonoBehaviour
{
    public Resource ResourceType;
    private DestroyableRock Rock;

    public Resource[] SpawnableResources;

    public void Start()
    {
        Rock = GetComponent<DestroyableRock>();

        float randomValue = Random.Range(0.0f, 1.0f);

        if (randomValue < 0.05f )
        {
            ResourceType = SpawnableResources[0];
        }

        else if (randomValue < 0.15f)
        {
            ResourceType = SpawnableResources[1];
        }

        else if (randomValue < 0.33f)
        {
            ResourceType = SpawnableResources[2];
        }

        else if (randomValue < 0.55f)
        {
            ResourceType = SpawnableResources[3];
        }

        else
        {
            ResourceType = SpawnableResources[4];
        }

        Rock.Init(ResourceType, null);

        float spawnAtAll = Random.Range(0.0f, 1.0f);

        //spawnAtAll = 0.0f;

        if (spawnAtAll <= 0.33f)
        {
            Destroy(gameObject);
            //Debug.Log("Destroyed rock at start");
        }
    }

}
