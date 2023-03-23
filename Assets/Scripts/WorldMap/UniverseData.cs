using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UniverseData
{
    public float posX;
    public float posY;
    public float posZ;

    public float rotX;
    public float rotY;
    public float rotZ;
    public float rotW;

    public float localScaleX;
    public float localScaleY;
    public float localScaleZ;

    public void SetPosRotAndScale(Vector3 pos,
                                  Quaternion rot,
                                  Vector3 scale)
    {
        posX = pos.x;
        posY = pos.y;
        posZ = pos.z;

        rotX = rot.x;
        rotY = rot.y;
        rotZ = rot.z;
        rotW = rot.w;

        localScaleX = scale.x;
        localScaleY = scale.y;
        localScaleZ = scale.z;
    }

    public Vector3 GetPos()
    {
        return new Vector3(posX, 
                           posY, 
                           posZ);
    }

    public Quaternion GetRot()
    {
        return new Quaternion(rotX, 
                              rotY, 
                              rotZ, 
                              rotW);
    }

    public Vector3 GetLocalScale()
    {
        return new Vector3(localScaleX, 
                           localScaleY, 
                           localScaleZ);
    }
}
