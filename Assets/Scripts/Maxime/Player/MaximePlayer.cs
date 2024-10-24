using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class MaximePlayer : MonoBehaviour
{
    [SerializeField] private InputActionReference ia_movement;
    [SerializeField] private float m_walkSpeed = 5.0f;
    private CharacterController m_characterController;


    private void Start()
    {
        m_characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        Move(ia_movement.action.ReadValue<Vector2>());
    }

    private void Move(Vector2 _moveValue)
    {
        transform.position += new Vector3(_moveValue.x, 0.0f, _moveValue.y) * Time.deltaTime * m_walkSpeed;
    }
}
