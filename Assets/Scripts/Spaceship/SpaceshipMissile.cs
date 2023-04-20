using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipMissile : UITrackable
{
    [SerializeField]
    GameObject missilePrefab;
    [SerializeField]
    Transform missileOrigin;
    ActorSpaceship actor;
    public bool shooting = false;

    float closestAngleToTarget = Mathf.Infinity;
    ActorSpaceship focusedTarget = null;
    ActorSpaceship prevFocusedTarget = null;
    List<ActorSpaceship> lockedTargets = new List<ActorSpaceship>();

    GameObject projectileParent;

    [SerializeField]
    int maxMissiles = 4;
    int currentMissiles;
    public override float MaxValue => maxMissiles;

    public override float CurrentValue => currentMissiles;

    [SerializeField]
    float shootInterval = 0.25f;
    float cooldown = 0f;
    [SerializeField]
    float maxLockAngle = 3f;


    void Start()
    {
        currentMissiles = maxMissiles;
        actor = GetComponentInParent<ActorSpaceship>();
        projectileParent = GameObject.FindGameObjectWithTag("ProjectileParent");
    }

    // Update is called once per frame
    void Update()
    {
        cooldown -= Time.deltaTime;

        if(cooldown <= 0f)
        {
            ScanForTarget();
        }

        if (shooting)
        {
            if (cooldown <= 0)
            {
                cooldown = shootInterval;
                LockOnTarget();
            }
        }
    }

    void LockOnTarget()
    {
        if(focusedTarget != null && !lockedTargets.Contains(focusedTarget) && currentMissiles > 0)
        {
            lockedTargets.Add(focusedTarget);
            focusedTarget.UnfocusShip(actor);
            focusedTarget.LockMissile(actor);
            Debug.Log("locking on: " + focusedTarget.name);
            currentMissiles--;
            GameObject missileObject = Instantiate(missilePrefab, missileOrigin.position, Quaternion.identity, projectileParent.transform);
            Missile spawnedMissile = missileObject.GetComponent<Missile>();
            spawnedMissile.Init(focusedTarget, this);
        }
    }

    public void ReleaseTarget(ActorSpaceship target)
    {
        target.UnlockMissile(actor);
        lockedTargets.Remove(target);
        focusedTarget = null;
    }

    void ScanForTarget()
    {
        prevFocusedTarget = focusedTarget;
        closestAngleToTarget = Mathf.Infinity;
        focusedTarget = null;
        foreach (ActorSpaceship hostileActor in actor.faction.hostileActors)
        {
            Vector3 projectedPos = hostileActor.GetProjectedPosition(250f, transform.position);
            Vector3 targetDir = projectedPos - transform.position;
            float angle = Vector3.Angle(targetDir, transform.forward);
            if (angle < closestAngleToTarget && angle < maxLockAngle)
            {
                closestAngleToTarget = angle;
                focusedTarget = hostileActor;
            }
            //Debug.Log("angle: " + angle + " name: " + hostileActor.name);
        }

        if(prevFocusedTarget != focusedTarget)
        {
            if(prevFocusedTarget != null)
            {
                prevFocusedTarget.UnfocusShip(actor);
            }

            if (focusedTarget)
            {
                if (!lockedTargets.Contains(focusedTarget) && currentMissiles > 0)
                {
                    focusedTarget.FocusShip(actor);
                }
            }
            //Debug.Log("focused target changed");
        }

        //Debug.Log("closestTarget: " + focusedTarget);
    }
}
