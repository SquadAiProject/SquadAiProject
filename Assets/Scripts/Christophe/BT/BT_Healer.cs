using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;
using Tree = BehaviorTree.Tree;

public class BT_Healer : Tree
{
    public Player player;
    public NavMeshAgent navMeshAgent;
    public bool isHealing;

    protected override Node SetupTree()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        healLine = GetComponent<LineRenderer>();
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetData("Player", player);
        player.AddNewNPC(gameObject);
        int index = player.NPCs.IndexOf(gameObject);
        
        SetData("IsHealing", isHealing);
        SetData("HealRange", healRange);

        Node root = new Sequence(new List<Node>     // NPC is always looking at player and follow player
        {
            new BTT_LookAtMousePos(transform),
            new BTT_Attack(gameObject),
            new BTT_Heal(gameObject),
            new BTT_NPCFollow(transform, navMeshAgent, index)
        });

        return root;
    }

    public LineRenderer healLine;
    public Transform healStartPoint;
    public float healRange = 15;
    public GameObject healthCrossPartcle;

    public void Heal(GameObject target)
    {
        healLine.enabled = true;
        healLine.SetPosition(1, healStartPoint.transform.position); 
        healLine.SetPosition(0, target.transform.position);
        healthCrossPartcle.SetActive(true);
        healthCrossPartcle.transform.position = target.transform.position + new Vector3(0, 1, 0);
    }

    public void ShutDownLine()
    {
        healLine.enabled = false;
        healthCrossPartcle.SetActive(false);
    }
}
