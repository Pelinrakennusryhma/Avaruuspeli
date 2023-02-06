using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class SpaceshipBT : BTree
{
    public static float detectTargetRange = 10f;
    [SerializeField]
    List<GameObject> targets;

    SpaceshipShoot spaceshipShoot;
    Transform shipTransform;

    private void Awake()
    {
        spaceshipShoot = GetComponentInChildren<SpaceshipShoot>();
        shipTransform = spaceshipShoot.transform;
    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckTargetInShootingRange(shipTransform),
                new TaskShoot(spaceshipShoot)
            }),
            new Sequence(new List<Node>
            {
                new CheckForTarget(shipTransform, targets),
                new TaskChaseTarget(shipTransform),
            }),
            new TaskPatrol()
        });

        return root;
    }
}
