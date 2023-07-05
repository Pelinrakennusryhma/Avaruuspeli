using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    [SerializeField] GameObject canvasObject;
    bool linkedToGameManager = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

            if (canvasObject == null)
            {
                canvasObject = transform.GetChild(0).gameObject;
            }

            SceneManager.sceneLoaded += OnSceneLoaded;

            DontDestroyOnLoad(gameObject);
        }

    }

    public void EnableCanvas()
    {
        canvasObject.SetActive(true);
    }

    void DisableCanvas()
    {
        canvasObject.SetActive(false);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        canvasObject.SetActive(false);

        if (!linkedToGameManager && GameManager.Instance != null)
        {
            linkedToGameManager = true;
            GameManager.Instance.OnEnterWorldMap -= DisableCanvas;
            GameManager.Instance.OnEnterWorldMap += DisableCanvas;
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //private void Update()
    //{
    //    Debug.Log("?!?!?!?!?1");
    //    if (canvasObject.activeSelf)
    //    {   
    //        timer += Time.deltaTime;
    //        Debug.Log("Timer");

    //        if(timer >= animSpeed)
    //        {
    //            timer = 0f;

    //            dotString += ".";

    //            if(dotString.Length > 3)
    //            {
    //                dotString = "";
    //            }

    //            loadingText.text = originalText + dotString;
    //        }

    //    }
    //}
}
