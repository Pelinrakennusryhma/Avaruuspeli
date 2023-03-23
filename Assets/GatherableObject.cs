using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableObject : MonoBehaviour
{
    public Resource ResourceType { get; private set; }

    public float OffsetFromGround = 0.2f;

    protected float LerpTime;
    protected float PickupTime;
    protected bool HasBeenPickedUp = false;
    protected Vector3 PickUpStartPos;
    protected Rigidbody rb;
    protected Collider MainCollider;
    protected CenterOfGravity _centerOfGravity;
    MeshCollider meshCollider;
    Vector3 targetPosition;

    private void Awake()
    {
       rb = GetComponent<Rigidbody>();
       meshCollider = GetComponent<MeshCollider>();
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

    public void Init(Resource resourceType)
    {
        ResourceType = resourceType;
    }

    public virtual void OnSpawn(CenterOfGravity centerOfGravity)
    {
        _centerOfGravity = centerOfGravity;
        SnapToGround();
    }

    public virtual void Activate(CenterOfGravity centerOfGravity)
    {
        _centerOfGravity = centerOfGravity;
        MeshCollider asteroidCollider = centerOfGravity.GetComponent<MeshCollider>();
        targetPosition = CalculateTargetPos(asteroidCollider);
        meshCollider.enabled = true;
    }

    Vector3 CalculateTargetPos(MeshCollider asteroidCollider)
    {
        Vector3 pos = asteroidCollider.ClosestPoint(transform.position);
        Vector3 dirNormal = (transform.position - asteroidCollider.transform.position).normalized;
        pos += (dirNormal * transform.localScale.x);
        return pos;
    }

    public virtual void SnapToGround()
    {
        //BoxCollider.isTrigger = false;


        if (_centerOfGravity != null)
        {
            //Debug.LogError("FIX THE SNAPPING TO GROUND. THIS DOESN'T WORK");

            Vector3 offset = (_centerOfGravity.transform.position - transform.position).normalized * 30.0f;
            transform.position += -offset;

            //Debug.DrawRay(transform.position, -offset, Color.red, 10000f);


            //transform.position = CenterOfGravity.Instance.transform.position;

            RaycastHit[] hitInfos = Physics.RaycastAll(transform.position,
                                    offset * 2.0f);

            for (int i = 0; i < hitInfos.Length; i++)
            {
                //Debug.Log("Hit " + hitInfos[i].collider.gameObject.name);

                if (hitInfos[i].collider.gameObject == _centerOfGravity.gameObject)
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
        } else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 3f);
        }
    }
}
