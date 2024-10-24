using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;

using Random = UnityEngine.Random;


public class BTT_SetWanderLocation : Node
{
    private Transform m_transform;
    private float m_minWanderRadius;
    private float m_maxWanderRadius;
    
    
    public BTT_SetWanderLocation(Transform _transform, float _minWanderRadius, float _maxWanderRadius)
    {
        m_transform = _transform;
        m_minWanderRadius = _minWanderRadius;
        m_maxWanderRadius = _maxWanderRadius;
    }

    public override NodeState Evaluate()
    {
        float angle = Random.value * (2.0f * Mathf.PI);
        Vector3 direction = new Vector3(Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
        float radius = Random.Range(m_minWanderRadius, m_maxWanderRadius);
        Vector3 position = direction * radius;
        Vector3 randomPos3D = m_transform.position + position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos3D, out hit, m_maxWanderRadius, NavMesh.AllAreas))
        {
            randomPos3D.y = hit.position.y;
            if (Vector3.Distance(hit.position, randomPos3D) < 0.1f)
            {
                m_tree.SetData("wanderPoint", hit.position);
                m_state = NodeState.SUCCESS;
                return m_state;
            }
            
            else
            {
                m_tree.ClearData("wanderPoint");
                m_state = NodeState.FAILURE;
                return m_state;
            }
        }
        else
        {
            // If no valid position is found, return failure and reloop
            m_tree.ClearData("wanderPoint");
            m_state = NodeState.FAILURE;
            return m_state;
        }
    }
}
