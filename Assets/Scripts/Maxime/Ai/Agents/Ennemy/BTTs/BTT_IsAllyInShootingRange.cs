using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class BTT_IsAllyInShootingRange : Node
{
    private Transform m_transform;
    private float m_shootRange;

    private static int m_allyLayerMask = 1 << 6;
    private static int m_obstacleLayerMask = 1 << 8;


    public BTT_IsAllyInShootingRange(Transform _transform, float _shootRange)
    {
        m_transform = _transform;
        m_shootRange = _shootRange;
    }

    public override NodeState Evaluate()
    {
        Collider[] colliders = Physics.OverlapSphere(m_transform.position, m_shootRange, m_allyLayerMask);
        if (colliders.Length > 0)
        {
            float distance = m_shootRange;
            Collider closestCollider = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                // Get the closest ally
                if (Vector3.Distance(colliders[i].transform.position, m_transform.position) <= distance)
                {
                    distance = Vector3.Distance(colliders[i].transform.position, m_transform.position);
                    closestCollider = colliders[i];
                }
            }
            
            // // Check if raycast between agent and ally is succesful
            RaycastHit hit;
            if (closestCollider)
            {
                Vector3 directionToTarget = (closestCollider.transform.position - m_transform.position).normalized;
                if (Physics.Raycast(m_transform.position, directionToTarget, out hit, distance, m_obstacleLayerMask))
                {
                    closestCollider = null;
                }
            }

            if (closestCollider != null)
            {
                m_tree.SetData("targetToShoot", closestCollider.transform.position);
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        
        m_tree.ClearData("targetToShoot");
        m_state = NodeState.FAILURE;
        return m_state;
    }
}
