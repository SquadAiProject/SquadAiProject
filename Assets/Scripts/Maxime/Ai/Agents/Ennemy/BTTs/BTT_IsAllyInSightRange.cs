using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTree;


public class BTT_IsAllyInSightRange : Node
{
    private Transform m_transform;
    
    private static int m_allyLayerMask = 1 << 6;


    public BTT_IsAllyInSightRange(Transform _transform)
    {
        m_transform = _transform;
    }

    public override NodeState Evaluate()
    {
        Collider[] colliders = Physics.OverlapSphere(m_transform.position, (float)m_tree.GetData("sightRadius"), m_allyLayerMask);
        if (colliders.Length > 0)
        {

            float distance = (float)m_tree.GetData("sightRadius");
            Collider closestCollider = null;
            for (int i = 0; i < colliders.Length; i++)
            {
                if (Vector3.Distance(colliders[i].transform.position, m_transform.position) <= distance)
                {
                    distance = Vector3.Distance(colliders[i].transform.position, m_transform.position);
                    closestCollider = colliders[i];
                }
            }

            if (closestCollider != null)
            {
                m_tree.SetData("target", closestCollider.transform.position);
                m_state = NodeState.SUCCESS;
                return m_state;
            }
        }
        
        m_tree.ClearData("target");
        m_state = NodeState.FAILURE;
        return m_state;
    }
}