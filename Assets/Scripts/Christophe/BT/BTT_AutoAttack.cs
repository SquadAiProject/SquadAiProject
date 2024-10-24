using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;


public class BTT_AutoAttack : Node
{
    private Player m_player;
    private GameObject m_gameObject;
    private float m_timer;
    private float m_originTimer;
    private bool isInit = false;

    public BTT_AutoAttack(GameObject _gameObject, float _timer)
    {
        m_gameObject = _gameObject;
        m_timer = _timer;
        m_originTimer = m_timer;
    }

    public override NodeState Evaluate()
    {
        if (!isInit)
        {
            m_player = (Player)m_tree.GetData("Player");
            isInit = true;
        }
        
        if (!BT_Attacker.isAutoAttacking)
        {
            m_state = NodeState.FAILURE;
            return m_state;
        }
        
        Vector3 lookDirection = BT_Attacker.shootPos - m_gameObject.transform.position;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            m_gameObject.transform.rotation = Quaternion.Slerp(m_gameObject.transform.rotation, targetRotation, Time.deltaTime * 10f);
        }

        m_timer -= Time.deltaTime;

        if (m_timer <= 0)
        {
            m_gameObject.GetComponent<Agent>().SpawnBullet();
            m_timer = m_originTimer;
        }
        
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
