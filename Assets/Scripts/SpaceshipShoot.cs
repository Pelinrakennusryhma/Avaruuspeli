using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipShoot : MonoBehaviour
{
    [SerializeField]
    Color color = Color.green;
    [SerializeField]
    Material material;
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
    Material coloredMaterial;

    void Start()
    {
        cooldown = shootInterval;

        ColorMaterial();
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

    void ColorMaterial()
    {
        coloredMaterial = new Material(material);
        coloredMaterial.color = color;
        coloredMaterial.SetColor("_EmissionColor", color);
    }

    void Shoot()
    {
        GameObject laserBoltObject = Instantiate(laserBoltPrefab, laserOrigins[0].position, laserOrigins[0].rotation, laserParent);
        LaserBolt laserBolt = laserBoltObject.GetComponent<LaserBolt>();
        laserBolt.Init(laserForce, coloredMaterial, laserDamage, laserLifetime, gameObject);
    }
}
