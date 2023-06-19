using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevCheck : MonoBehaviour
{
    // We use this object for finding purposes.
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
