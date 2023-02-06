using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyer : MonoBehaviour
{
    [SerializeField]
    float lifetime = 10f;

    void Start()
    {
        StartCoroutine(DestroyObject(lifetime));
    }
    
    IEnumerator DestroyObject(float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
