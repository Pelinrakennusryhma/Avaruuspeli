using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMapOverlayCamera : MonoBehaviour
{

    public Camera MainCamera;

    private Camera CameraComponent;
    // Start is called before the first frame update
    void Start()
    {
        CameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = MainCamera.transform.position;
        transform.rotation = MainCamera.transform.rotation;
        CameraComponent.fieldOfView = MainCamera.fieldOfView;
    }
}
