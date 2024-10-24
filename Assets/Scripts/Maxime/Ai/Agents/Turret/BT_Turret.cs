using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using BehaviorTree;
using UnityEngine;


using Tree = BehaviorTree.Tree;


public class BT_Turret : Tree
{
    [SerializeField] private float sightRadius;

    [SerializeField] private float shootSpeed;
    [SerializeField] private float shootRotationSpeed;
    private float shootTimer;


    protected override Node SetupTree()
    {
        SetupBlackBoardVariables();

        Node root = new Sequence(new List<Node>
        {
            new BTT_Turret_IsAllyInSightRange(transform, sightRadius),
            new BTT_Turret_Shoot(transform, shootSpeed, shootRotationSpeed)
        });

        return root;
    }

    private void SetupBlackBoardVariables()
    {
        SetData("sightRadius", sightRadius);
        SetData("shootTimer", shootTimer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, sightRadius);
    }
}
