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
    float rateOfFire = 600;
    [SerializeField]
    Transform laserParent;

    public bool shooting = false;
    float cooldown;
    float timer;
    Material coloredMaterial;

    void Start()
    {
        cooldown = 60 / rateOfFire;
        timer = cooldown;

        ColorMaterial();

        if(laserParent == null)
        {
            laserParent = GameObject.FindGameObjectWithTag("ProjectileParent").transform;
        }
    }

    private void Update()
    {
        timer -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if (shooting)
        {
            if(timer <= 0)
            {
                timer = cooldown;
                Shoot();
            }
        }
    }

    public void Init(float damage, float velocity, float rateOfFire)
    {
        laserDamage = damage;
        laserSpeed = velocity;
        this.rateOfFire = rateOfFire;

        cooldown = 60 / rateOfFire;
        timer = cooldown;
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

        AudioManager.Instance.PlayOneShot(FMODEvents.Instance.LaserShoot, transform.position);
    }
}
