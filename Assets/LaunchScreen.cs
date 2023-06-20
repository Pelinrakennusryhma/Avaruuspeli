using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaunchScreen : MonoBehaviour
{
    public void OnNewGameButtonPressed()
    {
        GameManager.LaunchType = GameManager.TypeOfLaunch.NewGame;
        SceneManager.LoadScene("WorldMap");
        //Debug.Log("Pressed new game");
    }

    public void OnDevGameButtonPressed()
    {
        GameManager.LaunchType = GameManager.TypeOfLaunch.DevGame;
        SceneManager.LoadScene("WorldMap");
        //Debug.Log("Pressed dev game");
    }

    public void OnLoadGameButtonPressed()
    {
        GameManager.LaunchType = GameManager.TypeOfLaunch.LoadedGame;
        SceneManager.LoadScene("WorldMap");
        //Debug.Log("Pressed load game");
    }

    public void OnQuitButtonPressed()
    {
        //Debug.Log("Pressed quit");
        Application.Quit();
    }
}
