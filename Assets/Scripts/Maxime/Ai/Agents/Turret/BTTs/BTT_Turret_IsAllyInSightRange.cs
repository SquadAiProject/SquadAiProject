using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class BTT_Turret_IsAllyInSightRange : Node
{
    private Transform m_transform;
    private float m_shightRadius;

    private static int m_allyLayerMask = 1 << 6;
    private static int m_obstacleLayerMask = 1 << 8;


    public BTT_Turret_IsAllyInSightRange(Transform _transform, float _shightRadius)
    {
        m_transform = _transform;
        m_shightRadius = _shightRadius;
    }

    public override NodeState Evaluate()
    {
        Collider[] colliders = Physics.OverlapSphere(m_transform.position, m_shightRadius, m_allyLayerMask);
        if (colliders.Length > 0)
        {
            float distance = m_shightRadius;
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
