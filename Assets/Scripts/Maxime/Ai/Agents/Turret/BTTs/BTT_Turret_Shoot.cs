using System.Collections;
using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

public class BTT_Turret_Shoot : Node
{
    private Transform m_transform;

    private float m_shootSpeed;
    private float m_shootRotationSpeed;


    public BTT_Turret_Shoot(Transform _transform, float _shootSpeed, float _shootRotationSpeed)
    {
        m_transform = _transform;
        m_shootSpeed = _shootSpeed;
        m_shootRotationSpeed = _shootRotationSpeed;
    }

    public override NodeState Evaluate()
    {
        // Rotate the turret
        Vector3 lookDirection = (Vector3)m_tree.GetData("targetToShoot") - m_transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        m_transform.rotation = Quaternion.Slerp(m_transform.rotation, targetRotation, Time.deltaTime * m_shootRotationSpeed);
        
        // Shoot timer
        if ((float)m_tree.GetData("shootTimer") <= 0.0f)
        {
            m_tree.SetData("shootTimer", m_shootSpeed);
            // Shoot
            m_tree.gameObject.GetComponent<Turret>().Shoot();
        }
        else
        {
            m_tree.SetData("shootTimer", (float)m_tree.GetData("shootTimer") - Time.deltaTime);
        }

        m_state = NodeState.SUCCESS;
        return m_state;
    }
}
