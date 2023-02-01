using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUppableDice :  PickUppableObject
{
    public BoxCollider PhysicsCollider;
    public Rigidbody Rigidbody;

    public override void OnPickUp(FirstPersonPlayerController holder, 
                                  PickUpFunctionality holderPickUpFunctionality)
    {
        base.OnPickUp(holder, holderPickUpFunctionality);        

        PhysicsCollider.enabled = false;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        Rigidbody.isKinematic = true;
        Rigidbody.useGravity = false;

    }

    public override void OnReleasePickUp()
    {
        base.OnReleasePickUp();

        PhysicsCollider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public void LateUpdate()
    {
        if (Holder != null) 
        {

            Rigidbody.transform.position = Holder.Camera.transform.position + Holder.Camera.transform.forward * 1.5f;
            Rigidbody.transform.Rotate(Vector3.right *22.0f * Time.deltaTime + Vector3.up * 18.0f * Time.deltaTime);
            //Debug.Log("Should move with player " + Time.time);
        }
    }

    public override void OnReleasePickUp(Vector3 forceToAdd,
                                         Vector3 angluarVelocity)
    {
        base.OnReleasePickUp(forceToAdd,
                             angluarVelocity);

        PhysicsCollider.enabled = true;
        Rigidbody.isKinematic = false;
        Rigidbody.useGravity = true;
        Rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        Rigidbody.AddForce(forceToAdd, ForceMode.Impulse);
        Rigidbody.angularVelocity = angluarVelocity;
    }
}
