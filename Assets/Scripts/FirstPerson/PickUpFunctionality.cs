using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpFunctionality : MonoBehaviour
{
    public LayerMask PickUpLayerMask;

    public List<PickUppableObject> PickUppableObjectsInTriggerArea;
    public PickUppableObject CurrentlyHoldingObject;

    public FirstPersonPlayerController FirstPersonPlayerController;

    public float LaunchForce;

    private FirstPersonPlayerControls Controls;

    // Start is called before the first frame update
    void Start()
    {
        Controls = GetComponent<FirstPersonPlayerControls>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (GameManager.Instance.IsPaused)
        {
            return;
        }

        if (Controls.InteractDown
            && CurrentlyHoldingObject == null)
        {
            PickUppableObject closestObject = null;
            float distanceToClosest = 10000.0f;

            for (int i = 0; i < PickUppableObjectsInTriggerArea.Count; i++)
            {
                if (PickUppableObjectsInTriggerArea[i] == null)
                {
                    return;
                }

                float distance = (transform.position - PickUppableObjectsInTriggerArea[i].transform.position).magnitude;

                if (distance <= distanceToClosest)
                {
                    closestObject = PickUppableObjectsInTriggerArea[i];
                    distanceToClosest = distance;
                }
            }

            if (closestObject != null)
            {
                CurrentlyHoldingObject = closestObject;
                CurrentlyHoldingObject.OnPickUp(FirstPersonPlayerController,
                                                this);
            }

            //Debug.Log("Should pick up " + Time.time);
        }



        if (Controls.Fire1Down
            && CurrentlyHoldingObject != null )
        {
            LaunchForce = Random.Range(10.0f, 20.0f);
            //Debug.Log("Launchforce " + LaunchForce);
            //Debug.Log("Release holded object!! " + Time.time);
            Vector3 anglularVelocity = new Vector3(Random.Range(-100, 100), 
                                                   Random.Range(-100, 100), 
                                                   Random.Range(-100, 100));

            CurrentlyHoldingObject.OnReleasePickUp(FirstPersonPlayerController.Camera.transform.forward * LaunchForce,
                                                   anglularVelocity);
            CurrentlyHoldingObject = null;
        }              
    }

    public void OnEnterTriggerArea(PickUppableObject pickUppableObject)
    {
        if (!PickUppableObjectsInTriggerArea.Contains(pickUppableObject)) 
        {
            PickUppableObjectsInTriggerArea.Add(pickUppableObject);
            //Debug.Log("Enter trigger area");
        }
    }

    public void OnExitTriggerArea(PickUppableObject pickUppableObject)
    {

        PickUppableObjectsInTriggerArea.Remove(pickUppableObject);
        
    }
}
