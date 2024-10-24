using System;
using BehaviorTree;
using UnityEngine;

public class BTT_Attack : Node
{
    private GameObject m_gameObject;
    private bool isInit = false;
    private Player m_player;
    private bool canShoot;

    public BTT_Attack(GameObject _gameObject)
    {
        m_gameObject = _gameObject;
    }
    
    private void OnEnable()
    {
        Player.OnBroadcastMessage += OnReceiveMessage;
    }

    private void OnDisable()
    {
        Player.OnBroadcastMessage -= OnReceiveMessage;
    }

    private void OnReceiveMessage()
    {
        if (m_gameObject)
            m_gameObject.GetComponent<Agent>().SpawnBullet();
        else
            OnDisable();
    }
    
    public static event Action BindEvent;
    private void Bind()
    {
        BindEvent?.Invoke();
    }
    
    public override NodeState Evaluate()
    {
        if (!isInit)
        {
            m_player = (Player)m_tree.GetData("Player");
            OnEnable();
            isInit = true;
        }
        
        if (m_tree.GetData("IsDefending") != null)
        {
            if (BT_Guardian.isDefending)
            {
                m_state = NodeState.FAILURE;
                return m_state;
            }
        }

        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
