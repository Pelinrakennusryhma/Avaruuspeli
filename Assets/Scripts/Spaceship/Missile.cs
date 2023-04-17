using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    GameObject explosionEffect;
    ActorSpaceship _target;
    SpaceshipMissile _shooter;
    GameObject shooterShip;

    float moveSpeed = 80f;
    float explosionRadius = 20f;
    float explosionDamage = 90f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ChaseTarget();
    }

    public void Init(ActorSpaceship target, SpaceshipMissile shooter)
    {
        _target = target;
        _shooter = shooter;
        shooterShip = shooter.gameObject;
    }

    void ChaseTarget()
    {
        if(_target != null && _target.ship != null)
        {
            float step = Time.deltaTime * moveSpeed;
            Vector3 targetPos = _target.ship.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);

            Vector3 targetDir = targetPos - transform.position;
            Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDir);
        } else
        {
            transform.position += transform.forward * Time.deltaTime * moveSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != shooterShip)
        {
            if(_shooter != null && _target != null)
            {
                _shooter.ReleaseTarget(_target);
            }

            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in hitColliders)
        {
            IDamageable damageable = collider.gameObject.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.Damage(explosionDamage, shooterShip);
            }
        }

        Destroy(gameObject);
    }
}
