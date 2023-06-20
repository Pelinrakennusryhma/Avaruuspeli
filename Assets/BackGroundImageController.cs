using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundImageController : MonoBehaviour
{
    // Start is called before the first frame update

    public GameObject Buttons;

    public GameObject Image1;
    public GameObject Image2;
    void Awake()
    {
        if (Random.Range(0, 2) == 0)
        {
            Image1.gameObject.SetActive(true);
            Image2.gameObject.SetActive(false);
            Buttons.transform.localPosition = Vector3.zero;
        }

        else
        {
            Image1.gameObject.SetActive(false);
            Image2.gameObject.SetActive(true);
            Buttons.transform.localPosition = new Vector3(120, 0, 0);
        }
    }


}
