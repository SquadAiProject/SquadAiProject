using BehaviorTree;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Tree = BehaviorTree.Tree;

public class BT_Guardian : Tree
{
    public Player player;
    public NavMeshAgent navMeshAgent;
    public static bool isDefending = false;

    protected override Node SetupTree()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetData("Player", player);
        player.AddNewNPC(gameObject);
        player.guardians.Add(gameObject);
        int index = player.NPCs.IndexOf(gameObject);
        int indexDefend = player.guardians.IndexOf(gameObject);
        SetData("IsDefending", isDefending);
        
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node> // NPC is always looking at player and follow player
            {
                new BTT_LookAtMousePos(transform),
                new BTT_Attack(gameObject),
                new BTT_NPCFollow(transform, navMeshAgent, index)
            }),
            new Sequence(new List<Node> // Defense Player Sequence in this case Guardian stay in front of player
            {
                new BTT_Defend(navMeshAgent,indexDefend)
            })
        });

        return root;
    }
}
