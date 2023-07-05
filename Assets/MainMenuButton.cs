using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MonoBehaviour
{
    public void GoToMainMenu()
    {
        GameManager gameManager = FindObjectOfType<GameManager>();
        DevListener devListener = FindObjectOfType<DevListener>();
        DevCheck devCheck = FindObjectOfType<DevCheck>();
        LoadingScreen loadingScreen = FindObjectOfType<LoadingScreen>();

        if (gameManager != null)
        {
            Destroy(gameManager.gameObject);
            GameManager.Instance = null;
        }

        if (devListener != null)
        {
            Destroy(devListener.gameObject);
        }

        if (devCheck != null)
        {
            Destroy(devCheck.gameObject);
        }
        
        if(loadingScreen != null)
        {
            Destroy(loadingScreen.gameObject);
            LoadingScreen.Instance = null;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("LaunchScreen");

    }
}
