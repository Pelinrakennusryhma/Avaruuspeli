using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUppableObject : MonoBehaviour
{

    public FirstPersonPlayerController Holder;
    public PickUpFunctionality HolderPickUpFunctionality;

    public virtual void OnPickUp(FirstPersonPlayerController holder,
                                 PickUpFunctionality holderPickUpFunctionality)
    {
        Holder = holder;
        HolderPickUpFunctionality = holderPickUpFunctionality;
        //Debug.Log("Picked up " + " " + gameObject.name + " " + Time.time);
    }

    public virtual void OnReleasePickUp()
    {
        Holder = null;
        HolderPickUpFunctionality = null;
        //Debug.Log("Released pick up " + " " + gameObject.name + " " + Time.time);
    }

    public virtual void OnReleasePickUp(Vector3 forceToAdd,
                                        Vector3 angularVelocity)
    {
        Holder = null;
        HolderPickUpFunctionality = null;
        //Debug.Log("Released pick up " + " " + gameObject.name + " " + Time.time);
    }
}
