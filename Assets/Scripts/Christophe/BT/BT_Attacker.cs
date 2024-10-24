using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Sequence = BehaviorTree.Sequence;
using Tree = BehaviorTree.Tree;


public class BT_Attacker : Tree
{
    public Player player;
    public NavMeshAgent navMeshAgent;
    public static float attackRange = 0.1f;
    public float autoAttackInterval = 1.0f;
    public static Vector3 shootPos;
    public static bool isAutoAttacking = false;

    protected override Node SetupTree()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetData("Player", player);
        player.AddNewNPC(gameObject);
        int index = player.NPCs.IndexOf(gameObject);
        
        SetData("IsAutoAttacking", isAutoAttacking);
        
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node> // NPC is always looking at player and follow player
            {
                new BTT_LookAtMousePos(transform),
                new BTT_Attack(gameObject),
                new BTT_NPCFollow(transform, navMeshAgent, index)
            }),
            new Sequence(new List<Node> // AutoAttack Sequence in this case NPC can't move
            {
                new BTT_AutoAttack(gameObject, autoAttackInterval)
            })
        });

        return root;
    }
}
