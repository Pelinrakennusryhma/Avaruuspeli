using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubblePiece : MonoBehaviour
{

    public Rigidbody Rigidbody;

    private bool HasBeenSpawned;
    private float DeathTimer;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
        HasBeenSpawned = false;
    }

    public void Spawn(Vector3 rockCenter)
    {
        //Debug.Log("Spawn rubble piece");

        HasBeenSpawned = true;
        DeathTimer = Random.Range(3.0f, 5.0f);

        if (CenterOfGravity.Instance == null)
        {
            Rigidbody.useGravity = true;
            Rigidbody.velocity = -(rockCenter - transform.position).normalized * Random.Range(10.5f, 16.09f);
        }

        else
        {
            Rigidbody.useGravity = false;
            //Rigidbody.velocity = (CenterOfGravity.Instance.transform.position - transform.position) * 10.0f;
            Rigidbody.velocity = -(rockCenter - transform.position).normalized * Random.Range(3.5f, 8.09f);
        }
    }

    public void FixedUpdate()
    {
        //if (CenterOfGravity.Instance != null)
        //{
        //    Rigidbody.velocity += (CenterOfGravity.Instance.transform.position - transform.position).normalized * 122.4f * Time.deltaTime;
        //}
    }

    public void Update()
    {
        if (HasBeenSpawned)
        {
            DeathTimer -= Time.deltaTime;

            if (DeathTimer <= 0)
            {
                //Destroy(gameObject);
            }
        }
    }


}
