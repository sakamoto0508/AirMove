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
    private WallCheck _wallCheck;
    private PlayerWallRunning _playerWallRunning;
    private PlayerWallJumping _playerWallJumping;
    private WallMoveCheck _wallMoveCheck;
    private Vector2 _currentMoveInput = Vector2.zero;
    public bool _isGrounded { get; private set; } = false;
    public bool _isSlope { get; private set; } = false;
    public bool _canWallJump { get; private set; } = false;
    public bool _isSliding { get; private set; } = false;
    public bool _isCrouching { get; private set; } = false;
    public bool _isSprint { get; private set; } = false;
    public bool _wallRight { get; private set; } = false;
    public bool _wallLeft { get; private set; } = false;
    public bool _isWallRunning { get; private set; } = false;
    public bool _isAboveGround { get; private set; } = false;

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
        _wallCheck = GetComponent<WallCheck>();
        _playerWallRunning = GetComponent<PlayerWallRunning>();
        _playerWallJumping = GetComponent<PlayerWallJumping>();
        _wallMoveCheck = GetComponent<WallMoveCheck>();
    }

    private void OnInputMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _currentMoveInput = input;
            _playerMove?.Move(input, _playerData, _isGrounded);
            _playerWallRunning?.WallRunningMove(input, _wallCheck.GetLeftWallHit(), _wallCheck.GetRightWallHit());
        }
        else if (context.canceled)
        {
            _currentMoveInput = Vector2.zero;
            _playerMove?.Stop();
            _playerWallRunning?.MoveStop();
        }
    }

    private void OnInputJump(InputAction.CallbackContext context)
    {
        if (_isWallRunning && _canWallJump)
        {
            _playerWallJumping?.WallJump(_wallCheck.GetLeftWallHit(), _wallCheck.GetRightWallHit(), _playerData);
        }
        else
        {
            _playerJump?.Jump(_playerData, _isGrounded, _isSlope);
        }
        if (_playerSliding._isSliding)
        {
            _playerSliding.StopSliding(_playerState);
        }
    }

    private void OnInputSprint(InputAction.CallbackContext context)
    {
        if (!_playerSliding._isSliding)
        {
            _playerSprint?.Sprint(_playerState, _isGrounded);
        }
        _isSprint = _playerSprint.IsSprinting();
    }

    private void OnInputCrouch(InputAction.CallbackContext context)
    {
        if (!_playerSliding._isSliding)
        {
            _playerCrouch?.Crouch(_playerState, _playerData);
        }
        _isCrouching = _playerCrouch.IsCrouching();
    }

    private void OnInputSliding(InputAction.CallbackContext context)
    {
        if (_playerSliding._isSliding)
        {
            _playerSliding.StopSliding(_playerState);
        }
        else
        {
            if (_playerSliding.CanStartSliding(_playerState, _isGrounded, _isSlope, _currentMoveInput))
            {
                _playerSliding.StartSliding(_playerState, _playerData);
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RegisterInputAction();
        _playerWallRunning.StartSetVariables(_playerData);
    }

    // Update is called once per frame
    void Update()
    {
        _playerState?.StateMachine(_isWallRunning, _isSliding, _isCrouching, _isGrounded, _isSlope, _isSprint);
        _isGrounded = _groundCheck.IsGrounded(_playerData);
        _isSlope = _slopeCheck.OnSlope(_playerData);
        _isSliding = _playerSliding.IsSliding();
        _isWallRunning = _playerWallRunning.IsWallRunning();
        _playerMove?.SetSliding(_playerSliding._isSliding);
        _playerSliding?.SetIsSlope(_isSlope);
        _wallRight = _wallCheck.CheckForRightWall(_playerData);
        _wallLeft = _wallCheck.CheckForLeftWall(_playerData);
        _playerWallRunning.SetWallCheck(_wallLeft, _wallRight);
        _playerWallJumping.SetWallCheck(_wallLeft, _wallRight);
        _isAboveGround = _wallCheck.AboveGround(_playerData);
        _playerWallRunning.SetAboveGround(_isAboveGround);
        _playerMove?.UpdateSpeed(_playerState, _playerData);
        _playerMove?.SetSlope(_isSlope, _slopeCheck.GetSlopeHit());
        _canWallJump = _wallMoveCheck.CanWallMove(_wallLeft, _wallRight, _currentMoveInput, _isAboveGround);
        _playerWallRunning.SetExitWall(_playerWallJumping.ReturnExitingWall());
        _playerWallRunning.SetCanWallMove(_canWallJump);

    }
}
