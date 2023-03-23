using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CenterStarData : UniverseData
{
    public enum StarEnum1
    {
        None = 0,
        GiantRed = 1,
        GiantYellow = 2,
        Test3 = 3,
        Test4 = 4,
        Test5 = 5
    }

    public string Name = "STAR";
    public int ID = -1;
    public float Test1 = 10.0f;
    public int Test2 = 12;
    public StarEnum1 Test;

}
