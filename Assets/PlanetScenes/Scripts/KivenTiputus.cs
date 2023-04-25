using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KivenTiputus : MonoBehaviour
{
    public GameObject Kivi;
    public GameObject spawn;
    public float TuhoutumisAika = 6f;
    private bool isCoroutineExecuting = false;

    // Start is called before the first frame update
    void Start()
    {
        // Instantiate(Kivi, new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ExecuteAfterTime(2));
    }



    IEnumerator ExecuteAfterTime(float time)
    {
        if (isCoroutineExecuting)
            yield break;
 
        isCoroutineExecuting = true;
 
        yield return new WaitForSeconds(time);
  
        // Code to execute after the delay
        // Kivi.transform.rotation = Quaternion.Euler(new Vector3(120,120,120));
        GameObject KiviClone = Instantiate(Kivi, new Vector3(spawn.transform.position.x, spawn.transform.position.y, spawn.transform.position.z), Quaternion.identity);
        Destroy(KiviClone, TuhoutumisAika);
        isCoroutineExecuting = false;
    }
}
