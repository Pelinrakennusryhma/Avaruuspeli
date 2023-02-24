using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBolt : MonoBehaviour
{
    GameObject _shooterShip;
    public GameObject source
    {
        get { return _shooterShip; }
    }
    float _damage = 0f;
    float _speed = 0f;
    ParticleSystem explosionEffect;
    bool hasHit = false;
    int _increaseColSizeTimes = 2;
    float _colSizeMultiplier = 4f;
    float _increaseColSizeDelay = 0.33f;
    BoxCollider col;

    private void Awake()
    {
        explosionEffect = GetComponentInChildren<ParticleSystem>();
        col = GetComponent<BoxCollider>();
    }

    public void Init(float speed, Material material, float damage, float lifetime, GameObject shooterShip, bool increaseColSize=true)
    {
        _damage = damage;
        _speed = speed;
        _shooterShip = shooterShip;
        
        SetMaterial(material);
        StartCoroutine(DestroySelf(lifetime));

        if (increaseColSize)
        {
            for (int i = 1; i < _increaseColSizeTimes + 1; i++)
            {
                StartCoroutine(IncreaseSize(_increaseColSizeDelay * i));
            }
        }
    }

    IEnumerator DestroySelf(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }

    IEnumerator IncreaseSize(float delay)
    {
        yield return new WaitForSeconds(delay);
        col.size = new Vector3(col.size.x * _colSizeMultiplier, col.size.y * _colSizeMultiplier, col.size.z);
    }

    private void Update()
    {
        if (!hasHit)
        {
            transform.position += transform.forward * Time.deltaTime * _speed;
        }

    }

    void SetMaterial(Material material)
    {
        GetComponentInChildren<MeshRenderer>().material = material;
        explosionEffect.GetComponent<ParticleSystemRenderer>().material = material;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != _shooterShip)
        {
            hasHit = true;
            explosionEffect.Play();
            float destroyDelay = explosionEffect.main.duration + 0.1f;
            StartCoroutine(DestroySelf(destroyDelay));

            if(other.gameObject.layer == LayerMask.NameToLayer("Damageable"))
            {
                IDamageable damageable = other.GetComponent<IDamageable>();
                damageable.Damage(_damage, source);
            }
        }
    }
}
