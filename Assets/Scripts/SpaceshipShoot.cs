using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipShoot : MonoBehaviour
{
    [SerializeField]
    float laserForce = 250f;
    [SerializeField]
    float laserDamage = 10f;
    [SerializeField]
    float laserLifetime = 3f;
    [SerializeField]
    private GameObject laserBoltPrefab;
    [SerializeField]
    private Transform[] laserOrigins;
    [SerializeField]
    float shootInterval = 3f;
    [SerializeField]
    Transform laserParent;
    public bool shooting = false;

    float cooldown;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        cooldown = shootInterval;
    }

    private void Update()
    {
        cooldown -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (shooting)
        {
            if(cooldown <= 0)
            {
                cooldown = shootInterval;
                Shoot();
            }
        }
    }

    void Shoot()
    {
        //Debug.Log("pew" + Time.time);
        GameObject laserBoltObject = Instantiate(laserBoltPrefab, laserOrigins[0].position, laserOrigins[0].rotation, laserParent);
        LaserBolt laserBolt = laserBoltObject.GetComponent<LaserBolt>();
        laserBolt.Init(laserForce, laserDamage, laserLifetime, gameObject, rb.velocity);
    }
}
