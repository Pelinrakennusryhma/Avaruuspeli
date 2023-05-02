using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipECM : MonoBehaviour, IUseable
{
    void Start()
    {
        
    }

    void Update()
    {
        Debug.Log("ECM ticking");
    }

    public void Use()
    {
        Debug.Log("ECM used");
    }
}
