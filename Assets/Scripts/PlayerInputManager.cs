using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput), typeof(CharacterController), typeof(AudioSource))]
public class PlayerInputManager : MonoBehaviour
{
    [Header("Input References")]
    [SerializeField] private InputActionReference move;
    [SerializeField] private InputActionReference look;
    
    [Header("Player Parameters")]
    [SerializeField] private float speed, rotationSpeed;
    [SerializeField] private float minRotation, maxRotation;
    [SerializeField] private float rotationLerp;

    private AudioSource m_footsteps;
    private PlayerInput m_playerInput;
    private CharacterController m_characterController;
    
    private Vector2 m_rawMovement, m_rawRotation;
    private Vector3 m_realMovement;
    private Quaternion m_realRotation;

    private void Awake()
    {
        m_playerInput = GetComponent<PlayerInput>();
        m_playerInput.onActionTriggered += OnActionTriggered;

        m_characterController = GetComponent<CharacterController>();

        m_footsteps = GetComponent<AudioSource>();
        
        transform.rotation = Quaternion.identity;
        m_realRotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, m_realRotation, rotationLerp);
        
        m_realMovement = transform.forward * m_rawMovement.y + transform.right * m_rawMovement.x;
        m_realMovement.y = 0;
        m_realMovement.Normalize();
        m_realMovement *= speed * Time.deltaTime;

        if (m_realMovement.sqrMagnitude > 0)
        {
            if(!m_footsteps.isPlaying)
                m_footsteps.Play();
        }
        else
        {
            m_footsteps.Pause();
        }
        
        m_characterController.Move(m_realMovement);
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
            return;
        }

        if (obj.action == look.action)
        {
            if (obj.performed)
            {
                m_rawRotation = obj.ReadValue<Vector2>() * rotationSpeed;
                
                m_realRotation *= Quaternion.AngleAxis(m_rawRotation.x, Vector3.up);
                m_realRotation *= Quaternion.AngleAxis(-m_rawRotation.y, Vector3.right);

                Vector3 clampedRotation = m_realRotation.eulerAngles;
                if (clampedRotation.x > 180)
                    clampedRotation.x -= 360;
                
                clampedRotation.z = 0;
                clampedRotation.x = Math.Clamp(clampedRotation.x, minRotation, maxRotation);
                m_realRotation = Quaternion.Euler(clampedRotation);
                return;
            }
        }
    }
}
