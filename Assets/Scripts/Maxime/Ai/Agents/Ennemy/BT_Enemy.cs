using System;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;


using Tree = BehaviorTree.Tree;


public class BT_Enemy : Tree
{
    [SerializeField] private float minWanderRadius;
    [SerializeField] private float maxWanderRadius;
    [SerializeField] private float sightRadius;
    [SerializeField] private float walkSpeed;
    
    [SerializeField] private float shootRange;
    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootRotationSpeed;
    private float shootTimer;

    
    protected override Node SetupTree()
    {
        SetupStaticBlackBoardVariables();
        
        Node root = new Selector(new List<Node>
        {
            // Go to ally if spotted
            new Sequence(new List<Node>
            {
                new BTT_IsAllyInSightRange(transform),
                new Selector(new List<Node>
                {
                    // Shoot ally if spotted
                    new Sequence(new List<Node>
                    {
                        new BTT_IsAllyInShootingRange(transform, shootRange),
                        new BTT_Shoot(transform, shootSpeed, shootRotationSpeed)
                    }),
                    new BTT_GoToTargetShootPoint(transform)
                })
            }),
            // Wander
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new BTT_IsAtWanderLocation(transform),
                    new BTT_SetWanderLocation(transform, minWanderRadius, maxWanderRadius)
                }),
                new BTT_GoToWanderPoint(transform)
            })
        });

        return root;
    }

    private void SetupStaticBlackBoardVariables()
    {
        SetData("sightRadius", sightRadius);
        SetData("walkSpeed", walkSpeed);
        
        SetData("shootRange", shootRange);
        SetData("shootTimer", shootTimer);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, shootRange);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, minWanderRadius);
        Gizmos.DrawWireSphere(transform.position, maxWanderRadius);
        if (GetData("wanderPoint") != null)
        {
            Gizmos.DrawSphere((Vector3)GetData("wanderPoint"), 0.3f);
        }
    }
}
