using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PelaajaTavaranKeräys : MonoBehaviour
{
   public GameObject pelaaja;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Kulta")) {
            //Destroy(Alus);
            GameManagerByAnttiK.Instance.Kullat += 2;
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Kivi")) {
            //Destroy(Alus);
            GameManagerByAnttiK.Instance.Kivet += 10;
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Timantti")) {
            //Destroy(Alus);
            GameManagerByAnttiK.Instance.Timantit += 2;
            collision.gameObject.SetActive(false);
        }

/*         if(collision.gameObject.CompareTag("Elämiä")) {
            //Destroy(Alus);
            GameManager.Instance.Life += 2;
            collision.gameObject.SetActive(false);
        }
        if(collision.gameObject.CompareTag("Save")) {
            GameManager.Instance.Save(Alus);
            //Destroy(Alus);
            // GameManager.Instance.Life += 2;
            collision.gameObject.SetActive(false);
        } */

    }
}
