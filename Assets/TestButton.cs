using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour
{
    public void OnButtonPressed()
    {
        Debug.Log("Pressed a test button " + gameObject.name);
    }
}
