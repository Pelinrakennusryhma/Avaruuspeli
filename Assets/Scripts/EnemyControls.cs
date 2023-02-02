using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : ActorSpaceship
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    protected override void OnDeath()
    {
        Destroy(gameObject);
    }
}
