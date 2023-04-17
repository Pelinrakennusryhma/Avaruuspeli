using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMissile : MonoBehaviour
{
    [SerializeField]
    GameObject missilePrefab;
    [SerializeField]
    Transform missileOrigin;
    ActorSpaceship actor;
    public bool shooting = false;

    float closestAngleToTarget = Mathf.Infinity;
    ActorSpaceship closestTarget = null;
    List<ActorSpaceship> lockedTargets = new List<ActorSpaceship>();

    GameObject projectileParent;

    void Start()
    {
        actor = GetComponentInParent<ActorSpaceship>();
        projectileParent = GameObject.FindGameObjectWithTag("ProjectileParent");
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
        if(closestTarget != null && !lockedTargets.Contains(closestTarget))
        {
            lockedTargets.Add(closestTarget);
            closestTarget.LockMissile(actor);
            Debug.Log("locking on: " + closestTarget.name);
            GameObject missileObject = Instantiate(missilePrefab, missileOrigin.position, Quaternion.identity, projectileParent.transform);
            Missile spawnedMissile = missileObject.GetComponent<Missile>();
            spawnedMissile.Init(closestTarget, this);
        }
    }

    public void ReleaseTarget(ActorSpaceship target)
    {
        target.UnlockMissile(actor);
        lockedTargets.Remove(target);

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
            //Debug.Log("angle: " + angle + " name: " + hostileActor.name);
        }

        Debug.Log("closestTarget: " + closestTarget);
    }
}
