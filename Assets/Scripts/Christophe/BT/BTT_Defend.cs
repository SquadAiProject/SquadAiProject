using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class BTT_Defend : Node
{
    private Player m_player;
    private bool isInit = false;
    private NavMeshAgent m_navMeshAgent;
    private int m_index;
    
    public BTT_Defend(NavMeshAgent _navMeshAgent,int _index)
    {
        m_index = _index;
        m_navMeshAgent = _navMeshAgent;
    }

    public override NodeState Evaluate()
    {
        if (!isInit)
        {
            m_player = (Player)m_tree.GetData("Player");
            isInit = true;
        }

        if (m_player.health >= 100)
        {
            BT_Guardian.isDefending = false;
        }
        int followNumber = m_index;
        int rowIndex = followNumber / 6;
        int positionInRow = followNumber % 6;

        Vector3 targetPos = m_player.transform.position + m_player.transform.forward * (rowIndex + 1) * 2.0f;

        if (positionInRow % 2 == 0)
            targetPos += m_player.transform.right * (positionInRow * 0.5f) * 5 * 0.5f;
        else
            targetPos -= m_player.transform.right * ((positionInRow + 1) * 0.5f) * 5 * 0.5f;

        m_navMeshAgent.speed = 15;
        m_navMeshAgent.SetDestination(targetPos);
        
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
