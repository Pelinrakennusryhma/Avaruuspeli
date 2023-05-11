using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPracticeTarget : MonoBehaviour
{
    private Health Health;

    private Rigidbody Rigidbody;

    private MeshCollider[] MeshColliders;

    private void Awake()
    {
        Health = GetComponent<Health>();
        Health.ResetHealth(5);

        Health.OnDie -= OnDeath;
        Health.OnDie += OnDeath;
        Health.OnDamageTaken -= TakingDamage;
        Health.OnDamageTaken += TakingDamage;

        Rigidbody = GetComponent<Rigidbody>();
        MeshColliders = GetComponentsInChildren<MeshCollider>();
    }

    public void TakingDamage(int damage)
    {
        //Debug.Log("Target practice target knows we are taking damage: " + damage);

        if (Health.HitPoints - damage <= 1)
        {
            SetUnderPhysics();
        }
    }

    public void OnDeath()
    {


        //Destroy(gameObject);
    }

    public void SetUnderPhysics()
    {
        for (int i = 0; i < MeshColliders.Length; i++)
        {
            MeshColliders[i].convex = true;
        }

        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;
        Rigidbody.mass = 10;
    }
}
