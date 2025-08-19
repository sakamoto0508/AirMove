using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputBuffer), typeof(PlayerState), typeof(PlayerData))]
public class PlayerController : MonoBehaviour
{
    private InputBuffer _inputBuffer;
    private PlayerData _playerData;
    private PlayerState _playerState;
    private PlayerMove _playerMove;
    private PlayerJump _playerJump;
    private GroundCheck _groundCheck;
    public bool _isGrounded { get; private set; }
    private void RegisterInputAction()
    {
        _inputBuffer.MoveAction.performed += OnInputMove;
        _inputBuffer.MoveAction.canceled += OnInputMove;
        _inputBuffer.JumpAction.started += OnInputJump;
    }

    private void OnDestroy()
    {
        _inputBuffer.MoveAction.performed -= OnInputMove;
        _inputBuffer.MoveAction.canceled -= OnInputMove;
        _inputBuffer.JumpAction.started -= OnInputJump;
    }

    private void Awake()
    {
        _inputBuffer = GetComponent<InputBuffer>();
        _playerData = GetComponent<PlayerData>();
        _playerState = GetComponent<PlayerState>();
        _playerMove = GetComponent<PlayerMove>();
        _playerJump = GetComponent<PlayerJump>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void OnInputMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _playerMove?.Move(input, _playerData,_isGrounded);
        }
        else if (context.canceled)
        {
            _playerMove?.Stop();
        }
    }

    private void OnInputJump(InputAction.CallbackContext context)
    {
        _playerJump?.Jump(_playerData, _isGrounded);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RegisterInputAction();
    }

    // Update is called once per frame
    void Update()
    {
        _isGrounded = _groundCheck.IsGrounded(_playerData);
    }
}
