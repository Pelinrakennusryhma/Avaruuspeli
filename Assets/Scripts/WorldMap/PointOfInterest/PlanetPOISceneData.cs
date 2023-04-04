using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/POIScene/Planet", order = 0)]
public class PlanetPOISceneData : POISceneData
{
    //[SerializeField]
    //private int minNumEnemies;
    //[SerializeField]
    //private int maxNumEnemies;
    //public int NumEnemies { get { return Random.Range(minNumEnemies, maxNumEnemies + 1); } }
    //[field: SerializeField]
    //public int NumAsteroids { get; private set; }
    //[SerializeField]
    //private int minMineables;
    //[SerializeField]
    //private int maxMineables;
    //public int NumMineables { get { return Random.Range(minMineables, maxMineables + 1); } }
    [field: SerializeField]
    public Resource[] MineableResources { get; private set; }
    [field: SerializeField]
    public int NumMerchants { get; private set; }

    public override string GetDescription()
    {
        string description = descriptionTemplate;

        if (MineableResources.Length > 0)
        {
            string mineralString = "";
            for (int i = 0; i < MineableResources.Length; i++)
            {
                mineralString += $"{MineableResources[i].itemName}";

                if (i < MineableResources.Length - 1)
                {
                    mineralString += ", ";
                }
            }

            description += $"\n - {mineralString}";
        }


        if (NumMerchants > 0)
        {
            if (NumMerchants == 1)
            {
                description += $"\n - One merchant.";
            }
            else
            {
                description += $"\n - {NumMerchants} merchants.";
            }

        }

        return description;
    }
}
