using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/POIScene/Asteroid", order = 0)]
public class AsteroidPOISceneData : POISceneData
{
    [field: SerializeField]
    public int NumEnemies { get; private set; }
    [field: SerializeField]
    public int NumAsteroids { get; private set; }
    [field: SerializeField]
    public int NumMineables { get; private set; }
    [SerializeField]
    Resource[] mineableResources;

    public override string GetDescription()
    {
        string description = descriptionTemplate;

        for (int i = 0; i < mineableResources.Length; i++)
        {
            description += $"\n - {mineableResources[i].itemName}";
        }

        return description;
    }
}
