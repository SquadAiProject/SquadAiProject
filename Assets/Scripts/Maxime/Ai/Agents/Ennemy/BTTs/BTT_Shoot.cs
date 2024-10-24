using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using BehaviorTree;


using Random = UnityEngine.Random;


public class BTT_Shoot : Node
{
    private Transform m_transform;
    private float m_shootSpeed;
    private float m_shootRotationSpeed;
    
    
    public BTT_Shoot(Transform _transform, float _shootSpeed, float _shootRotationSpeed)
    {
        m_transform = _transform;
        m_shootSpeed = _shootSpeed;
        m_shootRotationSpeed = _shootRotationSpeed;
    }

    public override NodeState Evaluate()
    {
        m_tree.gameObject.GetComponent<NavMeshAgent>().SetDestination(m_transform.position);
        // Rotate
        Vector3 lookDirection = (Vector3)m_tree.GetData("target") - m_transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, Time.deltaTime * m_shootRotationSpeed);

        // Shoot timer
        if ((float)m_tree.GetData("shootTimer") <= 0.0f)
        {
            m_tree.SetData("shootTimer", m_shootSpeed);
            // Shoot
            m_tree.gameObject.GetComponent<Enemy>().Shoot();
        }
        else
        {
            m_tree.SetData("shootTimer", (float)m_tree.GetData("shootTimer") - Time.deltaTime);
        }
        
        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
