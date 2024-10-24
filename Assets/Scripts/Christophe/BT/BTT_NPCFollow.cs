using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BTT_NPCFollow : Node
{
    private Transform m_transform;
    private NavMeshAgent m_navMeshAgent;
    private Player m_player;
    private int m_index;
    private bool isInit = false;
    
    public BTT_NPCFollow(Transform _transform, NavMeshAgent _navMeshAgent, int _index)
    {
        m_transform = _transform;
        m_navMeshAgent = _navMeshAgent;
        m_index = _index;
    }

    private Vector3 targetPos;
    
    void FollowAsSquad()
    {
        int followNumber = m_index;
        int rowIndex = followNumber / 3;
        int positionInRow = followNumber % 3;

        targetPos = m_player.transform.position - m_player.transform.forward * (rowIndex + 1) * 2.0f;

        if (positionInRow % 2 == 0)
        {
            targetPos += m_player.transform.right * (positionInRow * 0.5f) * 5 * 0.5f;
        }
        else
        {
            targetPos -= m_player.transform.right * ((positionInRow + 1) * 0.5f) * 5 * 0.5f;
        }
    }

    void FollowAsCircle()
    {
        int followNumber = m_index;
        int totalUnits = m_player.NPCs.Count;
        float angleStep = 360.0f / totalUnits;
        float currentAngle = followNumber * angleStep;
        float radians = currentAngle * Mathf.Deg2Rad;
        
        targetPos = m_player.transform.position + new Vector3(
            Mathf.Cos(radians) * 3,
            0,
            Mathf.Sin(radians) * 3
        );
    }

    public override NodeState Evaluate()
    {
        if (!isInit)
        {
            m_player = (Player)m_tree.GetData("Player");
            isInit = true;
        }

        if (m_player.health <= 80)
        {
            BT_Guardian.isDefending = true;
        }

        switch (m_player.format)
        {
            case Player.FormatType.Squad:
                FollowAsSquad();
                break;
            case Player.FormatType.Circle:
                FollowAsCircle();
                break;
        }
        
        float distanceToPlayer = Vector3.Distance(m_transform.position, targetPos);
        
        if (distanceToPlayer <= BT_Attacker.attackRange)
        {
            m_navMeshAgent.SetDestination(m_transform.position);
            m_state = NodeState.FAILURE;
            return m_state;
        }
        
        m_navMeshAgent.SetDestination(targetPos);
        
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
