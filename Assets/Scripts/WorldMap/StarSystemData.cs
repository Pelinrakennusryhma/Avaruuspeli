using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StarSystemData : UniverseData
{
    public enum StarSystemEnum
    {
        None = 0,
        Test1 = 1,
        Test2 = 2,
        Test3 = 3
    }

    public string Name = "STAR SYSTEM";
    public int ID = -1;

    public float Test1 = 6000.0f;
    public int Test2 = 6000;
    public StarSystemEnum Test;

    public CenterStarData CenterStar;
    public List<PlanetData> Planets = new List<PlanetData>();
    public List<AsteroidFieldData> Asteroids = new List<AsteroidFieldData>();

    public List<PlanetData> ConvertArrayToList(PlanetData[] data)
    {
        List<PlanetData> list = new List<PlanetData>();

        for (int i = 0; i < data.Length; i++)
        {
            list.Add(data[i]);
        }

        return list;
    }

    public PlanetData[] ConvertListToArray(List<PlanetData> data)
    {
        return data.ToArray();
    }

    public List<AsteroidFieldData> ConvertArrayToList(AsteroidFieldData[] data)
    {
        List<AsteroidFieldData> list = new List<AsteroidFieldData>();

        for (int i = 0; i < data.Length; i++)
        {
            list.Add(data[i]);
        }

        return list;
    }

    public AsteroidFieldData[] ConvertListToArray(List<AsteroidFieldData> data)
    {
        return data.ToArray();
    }
}
