using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;


public class BTT_GoToWanderPoint : Node
{
    private Transform m_transform;

    public BTT_GoToWanderPoint(Transform _transform)
    {
        m_transform = _transform;
    }

    public override NodeState Evaluate()
    {
        if (m_tree.GetData("wanderPoint") != null)
        {
            Vector3 target = (Vector3)m_tree.GetData("wanderPoint");
            if (Vector3.Distance(m_transform.position, target) > 0.1f)
            {
                // Set destination
                m_tree.gameObject.GetComponent<NavMeshAgent>().SetDestination(target);
                // Set the rotation
                Vector3 lookDirection = target - m_transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, Time.deltaTime * 10f);
            }
            else
            {
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        
        m_state = NodeState.RUNNING;
        return m_state;
    }
}
