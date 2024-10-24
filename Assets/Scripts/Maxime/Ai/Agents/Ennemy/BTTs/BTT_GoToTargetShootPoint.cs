using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;
using UnityEngine.AI;


public class BTT_GoToTargetShootPoint : Node
{
    private Transform m_transform;

    private static int m_obstacleLayerMask = 1 << 8;


    public BTT_GoToTargetShootPoint(Transform _transform)
    {
        m_transform = _transform;
    }

    public override NodeState Evaluate()
    {
        if (m_tree.GetData("target") != null)
        {
            Vector3 target = (Vector3)m_tree.GetData("target");
            RaycastHit hit;
            Vector3 directionToTarget = (target - m_transform.position).normalized;
            if (Vector3.Distance(m_transform.position, target) > (float)m_tree.GetData("shootRange"))
            {
                // Set destination
                m_tree.gameObject.GetComponent<NavMeshAgent>().SetDestination(target);
                // Set the rotation
                Vector3 lookDirection = m_transform.forward;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, Time.deltaTime * 10.0f);
            }
            // Continue going to shootPoint if the ally is covered by an obstacle 
            else if (Physics.Raycast(m_transform.position, directionToTarget, out hit, Vector3.Distance(m_transform.position, target), m_obstacleLayerMask))
            {
                // Set destination
                m_tree.gameObject.GetComponent<NavMeshAgent>().SetDestination(target);
                // Set the rotation
                Vector3 lookDirection = m_transform.forward;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, Time.deltaTime * 10.0f);
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
