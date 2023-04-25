using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TippuiLäpi : MonoBehaviour
{
    // Pitää yllä muuttujaa leveliä johon merkataan seuraavaa kentän nimi.
    public string leveli;

    // Törmäyksen tutkiva funktio
    void OnCollisionEnter(Collision collision)
    {
        // Jos Pelaaja tag osuu...
        if (collision.gameObject.tag == "Pelaaja")
        {
            //If the GameObject has the same tag as specified, output this message in the console
            Debug.Log("Huti");
            
            // Lataa seuraavan kentän.
            SceneManager.LoadScene(leveli);
        }
    }
}
