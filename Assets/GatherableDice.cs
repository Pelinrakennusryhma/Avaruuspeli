using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableDice : GatherableObject
{
    public BoxCollider BoxCollider;

    private Vector3 RandomRot;

    [SerializeField]
    CenterOfGravity centerOfGravity;

    private void Awake()
    {
        OffsetFromGround = 0.2f;
        BoxCollider = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();
        RandomRot = new Vector3(Random.Range(-90.0f, 90.0f), 
                                Random.Range(-90.0f, 90.0f), 
                                Random.Range(-90.0f, 90.0f));
    }

    public void OnEnable()
    {
        //SnapToGround();
    }

    public override void OnSpawn(CenterOfGravity centerOfGravity)
    {
        SnapToGround();
    }

    private void FixedUpdate()
    {
        transform.Rotate(RandomRot * Time.deltaTime);
    }
}
