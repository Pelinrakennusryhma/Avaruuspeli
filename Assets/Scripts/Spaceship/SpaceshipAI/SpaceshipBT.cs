using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public enum Stance
{
    Aggressive,
    Passive,
    Travel
}

public class SpaceshipBT : BTree
{
    [SerializeField]
    Stance stance;
    public static float detectTargetRange = 500f;
    [SerializeField]
    float patrolArea = 1000f;

    EnemyControls enemyControls;
    Transform shipTransform;
    SpaceshipShoot spaceshipShoot;
    SpaceshipEvents spaceshipEvents;

    private void Awake()
    {
        enemyControls = GetComponent<EnemyControls>();
        shipTransform = transform.GetChild(0);
        spaceshipShoot = shipTransform.GetComponent<SpaceshipShoot>();
        spaceshipEvents = shipTransform.GetComponent<SpaceshipEvents>();

    }
    protected override Node SetupTree()
    {
        Node root = null;
        switch (stance)
        {
            case Stance.Aggressive:
                root = new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckForObstacle(enemyControls, shipTransform),
                        new TaskAvoidObstacle(enemyControls, shipTransform)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckForHit(spaceshipEvents),
                        new TaskEvadeAttacker(enemyControls)
                    }),
                    new Sequence(new List<Node>
                    {
                        new CheckTargetInShootingRange(shipTransform),
                        new TaskShoot(enemyControls),
                        new CheckForTarget(shipTransform, enemyControls.faction.hostileActors),
                        new TaskChaseTarget(enemyControls, shipTransform, spaceshipShoot.laserSpeed),
                    }),
                    new TaskPatrol(enemyControls, patrolArea)
                });
                break;
            case Stance.Passive:
                root = new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        new CheckForObstacle(enemyControls, shipTransform),
                        new TaskAvoidObstacle(enemyControls, shipTransform)
                    }),
                    new Sequence(new List<Node>
                    {
                        // add passive logic
                        new CheckForTarget(shipTransform, enemyControls.faction.hostileActors),
                    }),
                    new TaskPatrol(enemyControls, patrolArea)
                });
                break;
            case Stance.Travel:
                root = new Selector(new List<Node>
                {
                    //new Sequence(new List<Node>
                    //{
                    //    // defend self or something?
                    //    new CheckForTarget(shipTransform, targets),
                    //}),
                    new Sequence(new List<Node>
                    {
                        new CheckForObstacle(enemyControls, shipTransform),
                        new TaskAvoidObstacle(enemyControls, shipTransform)
                    }),
                    new TaskMoveToPosition(enemyControls, shipTransform)
                });
                break;
            default:
                break;
        }

        return root;
    }
}
