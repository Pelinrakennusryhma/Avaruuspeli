using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAtmosphere : MonoBehaviour
{

    private float RotateRate;
    private Vector3 RotateDir;

    private void Awake()
    {
        RotateDir = new Vector3(Random.Range(-1.0f, 1.0f),
                                Random.Range(-1.0f, 1.0f),
                                Random.Range(-1.0f, 1.0f));
        RotateRate = Random.Range(10.0f, 20.0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(RotateDir, RotateRate * Time.deltaTime);
    }
}
