using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//enum EnemyAmountThresholds
//{
//    Small = 3,
//    Moderate = 6,
//    Big = 10,
//    Huge = 11,
//}

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/POIScene/Asteroid", order = 0)]
public class AsteroidPOISceneData : POISceneData
{
    [SerializeField]
    private int minNumEnemies;
    [SerializeField]
    private int maxNumEnemies;
    public int NumEnemies { get { return Random.Range(minNumEnemies, maxNumEnemies + 1); } }
    [field: SerializeField]
    public int NumAsteroids { get; private set; }
    [SerializeField]
    private int minMineables;
    [SerializeField]
    private int maxMineables;
    public int NumMineables { get { return Random.Range(minMineables, maxMineables + 1); } }
    [field: SerializeField]
    public Resource[] MineableResources { get; private set; }

    public override string GetDescription()
    {
        string description = descriptionTemplate;

        if(MineableResources.Length > 0)
        {
            string mineralString = "";
            for (int i = 0; i < MineableResources.Length; i++)
            {
                mineralString += $"{MineableResources[i].itemName}";

                if(i < MineableResources.Length - 1)
                {
                    mineralString += ", ";
                }
            }

            description += $"\n - {mineralString}";
        }


        if(minNumEnemies > 0)
        {
            if(minNumEnemies == 1)
            {
                description += $"\n - At least one enemy ship";
            } else
            {
                description += $"\n - At least {minNumEnemies} enemy ships";
            }
            
        }

        return description;
    }

    //string GetEnemyAmountDescription()
    //{
    //    switch (minNumEnemies)
    //    {
    //        case int n when n <= (int)EnemyAmountThresholds.Small:
    //            return EnemyAmountThresholds.Small.ToString();
    //        case int n when n <= (int)EnemyAmountThresholds.Moderate:
    //            return EnemyAmountThresholds.Moderate.ToString();
    //        case int n when n <= (int)EnemyAmountThresholds.Big:
    //            return EnemyAmountThresholds.Big.ToString();
    //        default:
    //            return EnemyAmountThresholds.Huge.ToString();
    //    }
    //}
}
