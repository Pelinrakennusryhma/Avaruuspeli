using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Globals : MonoBehaviour
{
    public static Globals Instance { get; private set; }
    public string landKey;
    public float leaveSpaceshipSceneDelay;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public bool IsSpaceshipScene()
    {
        return SceneManager.GetActiveScene().buildIndex == 2;
    }

}
