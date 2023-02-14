using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetProjection : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sprite;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        CalculatePosition(10f);
    }

    public void CalculatePosition(float projectileSpeed)
    {
        sprite.transform.position = transform.position + (rb.velocity * projectileSpeed * Time.deltaTime);
    }
}
