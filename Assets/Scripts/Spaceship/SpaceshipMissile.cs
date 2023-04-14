using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMissile : MonoBehaviour
{
    [SerializeField]
    float maxLockAngle = 10f;
    ActorSpaceship actor;
    public bool shooting = false;

    float closestAngleToTarget = Mathf.Infinity;
    ActorSpaceship closestTarget = null;
    List<ActorSpaceship> lockedTargets = new List<ActorSpaceship>();

    void Start()
    {
        actor = GetComponentInParent<ActorSpaceship>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shooting)
        {
            ScanForTarget();
            LockOnTarget();
        }
    }

    void LockOnTarget()
    {
        if(closestTarget != null)
        {
            closestTarget.LockMissile(actor);
        }
    }

    void ScanForTarget()
    {
        closestAngleToTarget = Mathf.Infinity;
        closestTarget = null;
        foreach (ActorSpaceship hostileActor in actor.faction.hostileActors)
        {
            Vector3 projectedPos = hostileActor.GetProjectedPosition(250f, transform.position);
            Vector3 targetDir = projectedPos - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);

            if (angle < closestAngleToTarget)
            {
                closestAngleToTarget = angle;
                closestTarget = hostileActor;
            }
            Debug.Log("angle: " + angle + " name: " + hostileActor.name);
        }

        Debug.Log("closestTarget: " + closestTarget);
    }
}
