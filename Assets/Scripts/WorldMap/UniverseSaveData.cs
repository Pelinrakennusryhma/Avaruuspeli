using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UniverseSaveData
{
    public List<GalaxyData> Galaxies;

    public static List<GalaxyData> ConvertArrayToList(GalaxyData[] data)
    {
        List<GalaxyData> list = new List<GalaxyData>();

        for (int i = 0; i < data.Length; i++)
        {
            list.Add(data[i]);
        }

        return list;
    }

    public static GalaxyData[] ConvertListToArray(List<GalaxyData> data)
    {
        return data.ToArray();
    }
}
