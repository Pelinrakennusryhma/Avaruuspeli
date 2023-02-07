using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTree;

public class SpaceshipBT : BTree
{
    public static float detectTargetRange = 20f;
    [SerializeField]
    List<GameObject> targets;

    EnemyControls enemyControls;
    Transform shipTransform;

    private void Awake()
    {
        enemyControls = GetComponent<EnemyControls>();
        shipTransform = transform.GetChild(0);

    }
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {               
                new CheckForTarget(shipTransform, targets),
                new TaskChaseTarget(shipTransform),
                new CheckTargetInShootingRange(shipTransform),
                new TaskShoot(enemyControls),
            }),
            new TaskPatrol(enemyControls)
        });

        return root;
    }
}
