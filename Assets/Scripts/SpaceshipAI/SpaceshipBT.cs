using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public enum Stance
{
    Aggressive,
    Passive
}

public class SpaceshipBT : BTree
{
    [SerializeField]
    Stance stance;
    public static float detectTargetRange = 120f;
    [SerializeField]
    List<GameObject> targets;
    [SerializeField]
    Transform devTargetTransform;

    EnemyControls enemyControls;
    Transform shipTransform;

    private void Awake()
    {
        enemyControls = GetComponent<EnemyControls>();
        shipTransform = transform.GetChild(0);

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
                        new CheckForTarget(shipTransform, targets),
                        new TaskChaseTarget(enemyControls),
                        new CheckTargetInShootingRange(shipTransform),
                        new TaskShoot(enemyControls),
                    }),
                    new TaskPatrol(enemyControls, devTargetTransform)
                });
                break;
            case Stance.Passive:
                root = new Selector(new List<Node>
                {
                    new Sequence(new List<Node>
                    {
                        // add passive logic
                        new CheckForTarget(shipTransform, targets),
                    }),
                    new TaskPatrol(enemyControls, devTargetTransform)
                });
                break;
            default:
                break;
        }

        return root;
    }
}
