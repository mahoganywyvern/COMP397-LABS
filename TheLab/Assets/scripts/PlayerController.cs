using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[RequireComponent(typeof(CharacterController))]

public class PlayerController : Subject
{

    TheLab _inputs;

    Vector2 _move;

    [Header("Character Controller")]

    [SerializeField] CharacterController _controller;

    [Header("Movements")]

    [SerializeField] float _speed;

    [SerializeField] float _gravity = -30.0f;

    [SerializeField] float _jumpHeight = 3.0f;

    [SerializeField] Vector3 _velocity;

    [Header("Ground Detection")]

    [SerializeField] Transform _groundCheck;

    [SerializeField] float _groundRadius = 0.5f;

    [SerializeField] LayerMask _groundMask;

    [SerializeField] bool _isGrounded;

    [Header("Respawn Locations")]

    [SerializeField] Transform _respawn;



    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _inputs = new TheLab();
        _inputs.Player.Move.performed += context => _move = context.ReadValue<Vector2>();
        _inputs.Player.Move.canceled += context => _move = Vector2.zero;
        _inputs.Player.Jump.performed += context => Jump();
    }
    void OnEnable()
    {
        _inputs.Enable();
    }

   void OnDisable()
    {
        _inputs.Disable();
    }

    private void FixedUpdate()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundRadius, _groundMask);

        if (_isGrounded && _velocity.y < 0.0f)
        {
            _velocity.y = -2.0f;
        }
        Vector3 movement = new Vector3(_move.x, 0.0f, _move.y) * _speed * Time.fixedDeltaTime;
        _controller.Move(movement);
        _velocity.y += _gravity * Time.fixedDeltaTime;
        _controller.Move(_velocity * Time.fixedDeltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_groundCheck.position, _groundRadius);
    }

    void Jump()
    {
        if (_isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2.0f * _gravity);
            NotifyObservers(PlayerEnums.Jump);
        }


    }


    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Triggering with {other.gameObject.tag}");

        if (other.CompareTag("Death"))
        {
            _controller.enabled = false;
            // transform.position = new Vector3(0.0f,3.0f,0.0f);
            transform.position = _respawn.position;
            _controller.enabled = true;
            NotifyObservers(PlayerEnums.Died);
        }
    }


}