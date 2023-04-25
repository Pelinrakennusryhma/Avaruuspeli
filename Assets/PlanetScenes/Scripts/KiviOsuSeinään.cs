using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KiviOsuSeinään : MonoBehaviour
{
 void OnCollisionEnter(Collision other)
 {

    if (other.gameObject.tag == "Seinä")
     {
        Destroy(this);
     }
 }
}
