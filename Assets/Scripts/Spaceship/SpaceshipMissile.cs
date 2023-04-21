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

    // Obstacle layer
    int layerMask = 1 << 11;

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

            Debug.Log("locking on: " + focusedTarget.name);
            currentMissiles--;
            GameObject missileObject = Instantiate(missilePrefab, missileOrigin.position, Quaternion.identity, projectileParent.transform);
            Missile spawnedMissile = missileObject.GetComponent<Missile>();
            spawnedMissile.Init(focusedTarget, this);
            focusedTarget.LockMissile(actor, spawnedMissile);
        }
    }

    public void ReleaseTarget(ActorSpaceship target)
    {
        target.UnlockMissile(actor);
        lockedTargets.Remove(target);

        // dirty hack to resume pulsing after missile hits if cursor is on the target
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

            // is the target prefered as being closer in angle and is the angle within allowed angle
            if (angle < closestAngleToTarget && angle < maxLockAngle)
            {
                bool targetInSight = true;
                RaycastHit hit;
                // check for asteroids blocking the sight
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                    //Debug.Log("Did Hit: " + hit.collider.gameObject.name);

                    float distanceToShip = Vector3.Distance(transform.position, hostileActor.ship.transform.position);
                    if (hit.distance < distanceToShip)
                    {
                        targetInSight = false;
                    }
                }

                if (targetInSight)
                {
                    closestAngleToTarget = angle;
                    focusedTarget = hostileActor;
                }           
            }
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
        }
    }
}
