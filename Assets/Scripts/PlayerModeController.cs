using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModeController : MonoBehaviour
{
    [SerializeField]
    GameObject playerShip;
    void Awake()
    {
        if(playerShip == null)
        {
            playerShip = GameObject.FindGameObjectWithTag("PlayerShip");
        }

        GameEvents.instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
    }

    void OnLeaveAsteroid()
    {
        playerShip.SetActive(true);
    }
}
