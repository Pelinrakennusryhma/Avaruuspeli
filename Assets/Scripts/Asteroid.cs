using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    MeshFilter meshFilter;
    [SerializeField]
    SphereCollider sphereCollider;
    [SerializeField]
    Mesh[] meshes;

    // Start is called before the first frame update
    void Start()
    {
        SetMesh();
    }

    void SetMesh()
    {
        //Mesh randomMesh = meshes[Random.Range(0, meshes.Length)];
        Mesh randomMesh = meshes[1];
        meshFilter.mesh = randomMesh;

        // Move mesh to match the collider
        Vector3 meshCenter = randomMesh.bounds.center;
        Debug.Log(meshCenter);
        meshFilter.transform.localPosition = new Vector3(meshCenter.x, meshCenter.y, meshCenter.z);

        // Scale mesh to match the collider
        Vector3 meshExtents = randomMesh.bounds.extents;
        float avgExtent = (meshExtents.x + meshExtents.y + meshExtents.z) / 3;
        sphereCollider.radius = avgExtent;

    }
}
