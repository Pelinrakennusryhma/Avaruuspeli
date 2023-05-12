using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipShoot : MonoBehaviour
{
    [SerializeField]
    Material material;
    [SerializeField]
    public float laserSpeed = 250f;
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

    public EventReference LaserShootEvent;

    public bool shooting = false;
    float cooldown;
    Material coloredMaterial;

    void Start()
    {
        cooldown = shootInterval;

        ColorMaterial();

        if(laserParent == null)
        {
            laserParent = GameObject.FindGameObjectWithTag("ProjectileParent").transform;
        }
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
        Color laserColor;
        ActorSpaceship actor = GetComponentInParent<ActorSpaceship>();
        if(actor != null)
        {
            laserColor = actor.faction.laserColor;
        } else
        {
            laserColor = Color.green;
        }
        coloredMaterial = new Material(material);
        coloredMaterial.color = laserColor;
        coloredMaterial.SetColor("_EmissionColor", laserColor);
    }

    void Shoot()
    {
        GameObject laserBoltObject = Instantiate(laserBoltPrefab, laserOrigins[0].position, laserOrigins[0].rotation, laserParent);
        LaserBolt laserBolt = laserBoltObject.GetComponent<LaserBolt>();
        laserBolt.Init(laserSpeed, coloredMaterial, laserDamage, laserLifetime, gameObject);

        FMODUnity.RuntimeManager.PlayOneShot(LaserShootEvent, transform.position);
    }
}
