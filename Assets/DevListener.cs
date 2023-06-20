using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevListener : MonoBehaviour
{
    void Awake()
    {
        // try to find an object, if fail: load launch scene

        DevCheck check = FindObjectOfType<DevCheck>();

        if (check == null)
        {
            SceneManager.LoadScene("LaunchScreen");
        }
    }
}
