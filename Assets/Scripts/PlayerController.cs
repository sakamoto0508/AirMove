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
    private SlopeCheck _slopeCheck;
    private PlayerSprint _playerSprint;
    private PlayerCrouch _playerCrouch;
    private PlayerSliding _playerSliding;
    private Vector2 _currentMoveInput = Vector2.zero;
    public bool _isGrounded { get; private set; } = false;
    public bool _isSlope { get; private set; } = false;
    private void RegisterInputAction()
    {
        _inputBuffer.MoveAction.performed += OnInputMove;
        _inputBuffer.MoveAction.canceled += OnInputMove;
        _inputBuffer.JumpAction.started += OnInputJump;
        _inputBuffer.SprintAction.started += OnInputSprint;
        _inputBuffer.CrouchAction.started += OnInputCrouch;
        _inputBuffer.SlidingAction.started += OnInputSliding;
    }

    private void OnDestroy()
    {
        _inputBuffer.MoveAction.performed -= OnInputMove;
        _inputBuffer.MoveAction.canceled -= OnInputMove;
        _inputBuffer.JumpAction.started -= OnInputJump;
        _inputBuffer.SprintAction.started -= OnInputSprint;
        _inputBuffer.CrouchAction.started -= OnInputCrouch;
        _inputBuffer.SlidingAction.started -= OnInputSliding;
    }

    private void Awake()
    {
        _inputBuffer = GetComponent<InputBuffer>();
        _playerData = GetComponent<PlayerData>();
        _playerState = GetComponent<PlayerState>();
        _playerMove = GetComponent<PlayerMove>();
        _playerJump = GetComponent<PlayerJump>();
        _groundCheck = GetComponent<GroundCheck>();
        _slopeCheck = GetComponent<SlopeCheck>();
        _playerSprint = GetComponent<PlayerSprint>();
        _playerCrouch = GetComponent<PlayerCrouch>();
        _playerSliding = GetComponent<PlayerSliding>();
    }

    private void OnInputMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _currentMoveInput = input;
            if (!_playerSliding._isSliding)
            {
                _playerMove?.Move(input, _playerData, _isGrounded);
            }
        }
        else if (context.canceled)
        {
            _currentMoveInput = Vector2.zero;
            if (!_playerSliding._isSliding)
            {
                _playerMove?.Stop();
            }
        }
    }

    private void OnInputJump(InputAction.CallbackContext context)
    {
        _playerJump?.Jump(_playerData, _isGrounded, _isSlope);
    }

    private void OnInputSprint(InputAction.CallbackContext context)
    {
        _playerSprint?.Sprint(_playerState, _isGrounded);
    }

    private void OnInputCrouch(InputAction.CallbackContext context)
    {
        _playerCrouch?.Crouch(_playerState, _playerData);
    }
    private void OnInputSliding(InputAction.CallbackContext context)
    {
        _playerSliding?.StartSliding(_playerState, _playerData, _isGrounded, _isSlope, _currentMoveInput);
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
        _isSlope = _slopeCheck.OnSlope(_playerData);
        _playerSliding?.UpdateSliding(_playerState, _playerData,_currentMoveInput);
        // �X���C�f�B���O���͒ʏ�̈ړ��X�V���X�L�b�v
        if (!_playerSliding._isSliding)
        {
            _playerMove?.UpdateSpeed(_playerState, _playerData);
            _playerMove?.SetSlope(_isSlope, _slopeCheck.GetSlopeHit());
        }
    }
}
