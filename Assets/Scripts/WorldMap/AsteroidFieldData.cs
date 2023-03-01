using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AsteroidFieldData : UniverseData
{
    public enum AsteroidEnum1
    {
        None = 0,
        Test1 = 1,
        Test2 = 2,
        Test3 = 3,
        Test4 = 4,
        Test5 = 5
    }

    public string Name = "ASTEROID FIELD";
    public int ID = -1;
    public float Test1 = 10.0f;
    public int Test2 = 12;
    public AsteroidEnum1 Test;

    public float Orbit;

    public float ReferenceObjectPosX;
    public float ReferenceObjectPosY;
    public float ReferenceObjectPosZ;

    public void SetReferenceObjectPos(Vector3 pos)
    {
        ReferenceObjectPosX = pos.x;
        ReferenceObjectPosY = pos.y;
        ReferenceObjectPosZ = pos.z;
    }
}
