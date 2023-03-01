using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlanetData : UniverseData
{
    public enum PlanetEnum1
    {
        None = 0,
        Uninhabited = 1,
        Inhabited = 2,
        Test3 = 3,
        Test4 = 4,
        Test5 = 5
    }

    public enum PlanetGraphicsType
    {
        None = 0,
        Placeholder1 = 1,
        Placeholder2 = 2,
        Placeholder3 = 3,
        Placeholder4 = 4,
        Placeholder5 = 5,
        Placeholder6 = 6,
        Placeholder7 = 7,
        Placeholder8 = 8,
        Placeholder9 = 9,
        Placeholder10 = 10
    }


    //public enum ToBreak
    //{
    //    None = 0,
    //    One = 1
    //}

    //public ToBreak ToBreaking;

    public string Name = "PLANET";
    public int ID = -1;
    public float Test1 = 10.0f;
    public int Test2 = 12;
    public PlanetEnum1 Test;
    public PlanetGraphicsType PlanetGraphics;
    public float Orbit;
}
