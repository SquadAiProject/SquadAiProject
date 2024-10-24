using BehaviorTree;
using UnityEngine;

public class BTT_LookAtMousePos : Node
{
    private Player m_player;
    private Transform m_transform;
    private bool isInit = false;

    public BTT_LookAtMousePos(Transform _transform)
    {
        m_transform = _transform;
    }

    public override NodeState Evaluate()
    {
        if (!isInit)
        {
            m_player = (Player)m_tree.GetData("Player");
            isInit = true;
        }

        if (m_tree.GetData("IsAutoAttacking") != null)
        {
            if (BT_Attacker.isAutoAttacking)
            {
                m_state = NodeState.FAILURE;
                return m_state;
            }
        }
        
        if (m_tree is BT_Attacker) {m_tree.SetData("IsAutoAttacking", BT_Attacker.isAutoAttacking);}
        
        if (m_tree is BT_Guardian) {m_tree.SetData("IsDefending", BT_Guardian.isDefending);}
        
        Vector3 lookDirection = m_player.shootPos - m_transform.position;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
        
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
