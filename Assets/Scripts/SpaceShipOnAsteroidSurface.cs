using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipOnAsteroidSurface : MonoBehaviour
{
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player touched ship");
            GameManager.Instance.OnLeaveAsteroidSurface();
        }
    }
}
