using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movTexture : MonoBehaviour
{
    // Scroll main texture based on time

    float scrollSpeed = 0.1f;
    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer> ();
    }

    void Update()
    {
        float offset = Time.time * scrollSpeed;
        rend.material.SetColor("_EmissionColor", Color.red);
        rend.material.SetTextureOffset("_MainTex", new Vector2(0, -offset));
    }
}
