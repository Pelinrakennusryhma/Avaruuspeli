using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HakkuOsuu : MonoBehaviour
{

    public GameObject floatingTextPrefab;
    // public Transform target;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
/*     public void Update()
    {
        Vector3 relativePos = target.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;
    } */

    public void ShowFloatingText(string määrä)
    {
        Debug.Log("OSUI!!!");


        // Vector3 relativePos = target.position - transform.position;

        // the second argument, upwards, defaults to Vector3.up
        // Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        //transform.rotation = rotation;

        Quaternion rotation = Quaternion.LookRotation(Camera.main.transform.right);
        GameObject pisteTextInstance = Instantiate(floatingTextPrefab, transform.position, rotation, transform);
        pisteTextInstance.GetComponentInChildren<TextMesh>().text = määrä;
        pisteTextInstance.transform.LookAt(Camera.main.transform);
        Destroy(pisteTextInstance, 0.3f);


/*         floatingTextPrefab.GetComponent<TextMesh>().text = määrä;
        Instantiate(floatingTextPrefab, transform.position, Quaternion.identity, transform); */
    }

}
