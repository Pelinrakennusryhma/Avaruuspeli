using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MeleePracticeTarget : MonoBehaviour
{
    public Health Health;

    public Rigidbody Rigidbody;

    public VisualEffect Explosion;

    public MeshRenderer Renderer;

    public CapsuleCollider Collider;

    public void Awake()
    {
        Health = GetComponent<Health>();
        Health.ResetHealth(10);

        Health.OnDie -= OnDeath;
        Health.OnDie += OnDeath;

        Health.OnAddForceAtPosition -= AddForceToPos;
        Health.OnAddForceAtPosition += AddForceToPos;

        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<CapsuleCollider>();
        Renderer = GetComponent<MeshRenderer>();

        Explosion.Stop();
    }


    public void AddForceToPos(Vector3 pos,
                              Vector3 force,
                              ForceMode forceMode)
    {
        Rigidbody.AddForceAtPosition(force, pos, forceMode);
        Debug.Log("Adding force to melee practice target " + Time.time);
    }

    public void OnDeath()
    {
        Collider.enabled = false;
        Renderer.enabled = false;        
        Destroy(Rigidbody);
        Explosion.playRate = 2.0f;
        Explosion.Play();

        //Destroy(gameObject);
    }
}
