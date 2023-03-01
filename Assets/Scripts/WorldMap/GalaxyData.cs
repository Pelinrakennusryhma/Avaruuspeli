using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GalaxyData : UniverseData
{
    [System.Serializable]
    public enum UniverseEnum
    {
        None = 0,
        Spiral = 1,
        Test2 = 2,
        Test3 = 3
    }

    public string Name = "GALAXY";    
    public int ID = -1;
    public float Test1 = 1000.0f;
    public int Test2 = 1000;

    public UniverseEnum Test;

    public List<StarSystemData> StarSystems = new List<StarSystemData>();

    public static List<StarSystemData> ConvertArrayToList(StarSystemData[] data)
    {
        List<StarSystemData> list = new List<StarSystemData>();

        for (int i = 0; i< data.Length; i++)
        {
            list.Add(data[i]);
        }

        return list;
    }

    public static StarSystemData[] ConvertListToArray(List<StarSystemData> data)
    {
        return data.ToArray();
    }
}
