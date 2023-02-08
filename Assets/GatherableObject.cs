using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public ResourceInventory.ResourceType ResourceType;

    public float OffsetFromGround = 0.2f;

    private float LerpTime;
    private float PickupTime;
    private bool HasBeenPickedUp = false;
    private Vector3 PickUpStartPos;
    private Rigidbody Rigidbody;
    private Collider MainCollider;

    private void Awake()
    {
       // Rigidbody = GetComponent<Rigidbody>();
       // Collider mainCollider = GetComponent<Collider>();
    }

    public void OnPickUp()
    {

        if (FirstPersonPlayerCharacter.Instance == null) 
        {
            gameObject.SetActive(false);
        }

        else
        {
            //if (MainCollider == null)
            //{
            //    Collider mainCollider = GetComponent<Collider>();
            //}

            //MainCollider.isTrigger = true;
            //gameObject.SetActive(false);
            LerpTime = Random.Range(0.1f, 0.2f);
            PickupTime = Time.time;
            HasBeenPickedUp = true;
            PickUpStartPos = transform.position;
            //Debug.Log("We have first person instance");
        }

        if (ResourceInventory.Instance != null)
        {
            ResourceInventory.Instance.CollectResource(ResourceType);
        }
    }

    public virtual void OnSpawn()
    {
        SnapToGround();
    }

    public virtual void SnapToGround()
    {
        //BoxCollider.isTrigger = false;


        if (CenterOfGravity.Instance != null)
        {
            //Debug.LogError("FIX THE SNAPPING TO GROUND. THIS DOESN'T WORK");

            Vector3 offset = (CenterOfGravity.Instance.transform.position - transform.position).normalized * 30.0f;
            transform.position += -offset;

            //Debug.DrawRay(transform.position, -offset, Color.red, 10000f);


            //transform.position = CenterOfGravity.Instance.transform.position;

            RaycastHit[] hitInfos = Physics.RaycastAll(transform.position,
                                    offset * 2.0f);

            for (int i = 0; i < hitInfos.Length; i++)
            {
                //Debug.Log("Hit " + hitInfos[i].collider.gameObject.name);

                if (hitInfos[i].collider.gameObject == CenterOfGravity.Instance.gameObject)
                {
                    transform.position = hitInfos[i].point + -offset.normalized * OffsetFromGround;
                    //Debug.Log("Snapped to " + hitInfos[i].collider.gameObject.name);
                    break;
                }
            }


            //Debug.Log(transform.position);
        }

        else
        {
            //Debug.LogWarning("Executed alternative to gravity");

            Vector3 offset = Vector3.up.normalized * 30.0f;
            transform.position += offset;

            RaycastHit[] hitInfos = Physics.RaycastAll(transform.position,
                                                       -offset * 2.0f);

            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].collider.gameObject != gameObject)
                {
                    transform.position = hitInfos[i].point + offset.normalized * OffsetFromGround;
                    break;
                }
            }


        }

        //BoxCollider.isTrigger = true;

        //Debug.Log("Should snap gatherable dice to ground");
    }

    private void Update()
    {
        if (HasBeenPickedUp)
        {
            float ratio = (Time.time - PickupTime) / LerpTime;

            transform.position = Vector3.Lerp(PickUpStartPos, 
                                              FirstPersonPlayerCharacter.Instance.StandingCapsule.transform.position 
                                              + -FirstPersonPlayerCharacter.Instance.StandingCapsule.transform.up * 0.5f, 
                                              ratio);

            if (ratio >= 1.0f)
            {
                HasBeenPickedUp = false;
                gameObject.SetActive(false);
            }
        }
    }
}
