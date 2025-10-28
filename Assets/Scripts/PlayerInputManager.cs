using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController))]
public class PlayerInputManager : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference move;
    
    [Header("Player Parameters")]
    [SerializeField] private float speed;
    
    private PlayerInput m_playerInput;
    private CharacterController m_characterController;
    private Vector2 m_rawMovement;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_playerInput.onActionTriggered += OnActionTriggered;

        m_characterController = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        m_characterController.Move(speed * Time.deltaTime * new Vector3(m_rawMovement.x, 0, m_rawMovement.y));
    }

    private void OnActionTriggered(InputAction.CallbackContext obj)
    {
        if (obj.action == move.action)
        {
            if (obj.performed)
            {
                m_rawMovement = obj.ReadValue<Vector2>();
                return;
            }

            if (obj.canceled)
            {
                m_rawMovement = Vector2.zero;
                return;
            }
        }
    }
}
