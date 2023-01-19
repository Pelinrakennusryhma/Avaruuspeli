using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    Mesh[] meshes;

    MeshFilter mFilter;
    // Start is called before the first frame update
    void Start()
    {
        mFilter = GetComponent<MeshFilter>();
        SetMesh();
    }

    void SetMesh()
    {
        mFilter.mesh = meshes[Random.Range(0, meshes.Length)];
    }
}
