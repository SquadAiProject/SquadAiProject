using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class BTT_IsAtWanderLocation : Node
{
    private Transform m_transform;


    public BTT_IsAtWanderLocation(Transform _transform)
    {
        m_transform = _transform;
    }

    public override NodeState Evaluate()
    {
        if (m_tree.GetData("wanderPoint") != null)
        {
            Vector3 wanderPoint = (Vector3)m_tree.GetData("wanderPoint");
            
            // If agent is at wanderPoint, continue sequence and set new wanderPoint
            if (Vector3.Distance(m_transform.position, wanderPoint) < 1.1f)
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
            // Otherwise continue going to wanderPoint
            else
            {
                m_state = NodeState.FAILURE;
                return m_state;
            }
        }
        // If there is no wanderPoint, continue sequence and set new wanderPoint
        else
        {
            m_state = NodeState.SUCCESS;
            return m_state;
        }
    }
}
