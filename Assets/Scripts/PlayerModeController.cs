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

        GameEvents.Instance.EventPlayerLanded.AddListener(OnLand);
        GameEvents.Instance.EventPlayerLeftAsteroid.AddListener(OnLeaveAsteroid);
    }

    void OnLand(MineableAsteroidTrigger asteroid)
    {
        
    }

    void OnLeaveAsteroid()
    {
        playerShip.SetActive(true);
    }
}
