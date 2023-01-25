using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipShoot : MonoBehaviour
{
    [SerializeField]
    float laserForce = 100f;
    [SerializeField]
    float laserLifetime = 3f;
    [SerializeField]
    private GameObject laserBoltPrefab;
    [SerializeField]
    private Transform[] laserOrigins;
    [SerializeField]
    float shootInterval = 3f;
    public bool shooting = false;

    float nextShot = 0f;
    float shootTimer = 0f;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        shootTimer += Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (shooting)
        {
            if(shootTimer > nextShot)
            {
                nextShot = Time.time + shootInterval;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        //Debug.Log("pew" + Time.time);
        GameObject laserBoltObject = Instantiate(laserBoltPrefab, laserOrigins[0].position, laserOrigins[0].rotation);
        LaserBolt laserBolt = laserBoltObject.GetComponent<LaserBolt>();
        laserBolt.Init(laserForce, laserLifetime, gameObject, rb.velocity);
    }
}
