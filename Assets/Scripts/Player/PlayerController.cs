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
    private WallActionChecker _wallActionChecker;
    private PlayerWallClimbing _playerWallClimbing;
    private PlayerClimbJumping _playerClimbJumping;
    private PlayerDashing _playerDashing;
    private PlayerFire _playerFire;
    private PlayerAiming _playerAiming;
    private PlayerAnimation _playerAnimation;
    private Vector2 _currentMoveInput = Vector2.zero;
    public bool _isGrounded { get; private set; } = false;
    public bool _isSlope { get; private set; } = false;
    public bool _canWallJump { get; private set; } = false;
    public bool _canWallClimb { get; private set; } = false;
    public bool _canClimbJump { get; private set; } = false;
    public bool _canDash { get; private set; } = true;
    public bool _newWall { get; private set; } = false;
    public bool _isSliding { get; private set; } = false;
    public bool _isCrouching { get; private set; } = false;
    public bool _isSprint { get; private set; } = false;
    public bool _isDashing { get; private set; } = false;
    public bool _isWallClimbing { get; private set; } = false;
    public bool _isAiming { get; private set; } = false;
    public bool _wallRight { get; private set; } = false;
    public bool _wallLeft { get; private set; } = false;
    public bool _wallFront { get; private set; } = false;
    public bool _isWallRunning { get; private set; } = false;
    public bool _isAboveGround { get; private set; } = false;
    public bool _isIdle {  get; private set; } = true;

    private void RegisterInputAction()
    {
        _inputBuffer.MoveAction.performed += OnInputMove;
        _inputBuffer.MoveAction.canceled += OnInputMove;
        _inputBuffer.JumpAction.started += OnInputJump;
        _inputBuffer.SprintAction.started += OnInputSprint;
        _inputBuffer.CrouchAction.started += OnInputCrouch;
        _inputBuffer.SlidingAction.started += OnInputSliding;
        _inputBuffer.AttackAction.started += OnInputAttack;
        _inputBuffer.PeekAction.started += OnInputPeek;
        _inputBuffer.PeekAction.canceled += OnInputPeek;
    }

    private void OnDestroy()
    {
        _inputBuffer.MoveAction.performed -= OnInputMove;
        _inputBuffer.MoveAction.canceled -= OnInputMove;
        _inputBuffer.JumpAction.started -= OnInputJump;
        _inputBuffer.SprintAction.started -= OnInputSprint;
        _inputBuffer.CrouchAction.started -= OnInputCrouch;
        _inputBuffer.SlidingAction.started -= OnInputSliding;
        _inputBuffer.AttackAction.started -= OnInputAttack;
        _inputBuffer.PeekAction.started -= OnInputPeek;
        _inputBuffer.PeekAction.canceled -= OnInputPeek;
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
        _wallActionChecker = GetComponent<WallActionChecker>();
        _playerWallClimbing = GetComponent<PlayerWallClimbing>();
        _playerClimbJumping = GetComponent<PlayerClimbJumping>();
        _playerDashing = GetComponent<PlayerDashing>();
        _playerFire = GetComponent<PlayerFire>();
        _playerAiming = GetComponent<PlayerAiming>();
        _playerAnimation = GetComponent<PlayerAnimation>();
    }

    private void OnInputMove(InputAction.CallbackContext context)
    {
        if (_playerClimbJumping.ExitingWallClimb)
            return;
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _currentMoveInput = input;
            _playerMove?.Move(input, _playerData);
            _playerWallRunning?.WallRunningMove(input, _wallCheck.GetLeftWallHit(), _wallCheck.GetRightWallHit());
            _playerWallClimbing?.WallClimbingMove(input, _wallCheck.GetFrontWallHit());
            _playerDashing?.SetMoveInput(input,_playerData);
        }
        else if (context.canceled)
        {
            _currentMoveInput = Vector2.zero;
            _playerMove?.Stop();
            _playerWallRunning?.MoveStop();
            _playerDashing?.MoveDirectionStop();
        }
    }

    private void OnInputJump(InputAction.CallbackContext context)
    {
        if (_wallFront)
        {
            _playerClimbJumping?.ClimbJumping(_wallCheck.GetFrontWallHit());
        }
        else if (_isWallRunning && _canWallJump)
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
        if (PlayerState.State.Air == _playerState.CurrentState && _canDash)
        {
            _playerDashing?.Dash(_playerData);
        }
        else if (_playerSliding._isSliding)
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

    private void OnInputAttack(InputAction.CallbackContext context)
    {
        _playerFire?.Fire(_playerData);
    }

    private void OnInputPeek(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            _playerAiming?.StartAim();
        }
        else if (context.canceled)
        {
            _playerAiming?.StopAim();
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RegisterInputAction();
        _playerMove?.StartSetVariables(_playerData);
        _playerWallRunning?.StartSetVariables(_playerData);
        _playerWallClimbing?.StartSetVariables(_playerData);
        _wallActionChecker?.StartSetVariables(_playerData);
        _playerClimbJumping?.StartSetVariables(_playerData);
        _wallCheck?.StartSetVariables(_playerData);
        _playerDashing?.StartSetVariables(_playerData);
        _playerFire?.StartSetVariables(_playerData);
    }

    // Update is called once per frame
    void Update()
    {
        _playerState?.StateMachine(_isDashing, _isWallClimbing, _isWallRunning, _isSliding, _isCrouching, _isGrounded, _isSlope, _isSprint, _isIdle);
        _playerState?.AnimationChange(_playerState.CurrentState);
        _isGrounded = _groundCheck.IsGrounded(_playerData);
        _isSlope = _slopeCheck.OnSlope(_playerData);
        _isSliding = _playerSliding.IsSliding();
        _isWallRunning = _playerWallRunning.IsWallRunning();
        _isWallClimbing = _playerWallClimbing.IsWallClimbing();
        _isDashing = _playerDashing.IsDashing();
        _isIdle = _playerMove.IsIdle();
        _isAiming=_playerAiming.IsAiming();
        _playerMove?.SetGrounded(_isGrounded);
        _playerMove?.SetSliding(_playerSliding._isSliding);
        _playerMove?.SetDashing(_isDashing);
        _playerSliding?.SetIsSlope(_isSlope);
        _wallRight = _wallCheck.CheckForRightWall(_playerData);
        _wallLeft = _wallCheck.CheckForLeftWall(_playerData);
        _wallFront = _wallCheck.CheckForFrontWall(_playerData);
        _playerWallRunning.SetWallCheck(_wallLeft, _wallRight);
        _playerWallJumping.SetWallCheck(_wallLeft, _wallRight);
        _playerWallClimbing.SetWallCheck(_wallFront);
        _isAboveGround = _wallCheck.AboveGround(_playerData);
        _playerWallRunning.SetAboveGround(_isAboveGround);
        _playerMove?.UpdateSpeed(_playerState, _playerData);
        _playerMove?.SetSlope(_isSlope, _slopeCheck.GetSlopeHit());
        _canWallJump = _wallActionChecker.CanWallMove(_wallLeft, _wallRight, _currentMoveInput, _isAboveGround);
        _canWallClimb = _wallActionChecker.CanWallClimb(_wallFront, _currentMoveInput, _wallCheck.GetWallLookAngle(_playerData, _wallCheck.GetFrontWallHit()));
        _playerWallRunning.SetExitWall(_playerWallJumping.ReturnExitingWall());
        _playerWallClimbing.SetExitingWallClimb(_playerClimbJumping.ReturnExitingWallClimb());
        _playerWallRunning.SetCanWallMove(_canWallJump);
        _playerWallClimbing.SetCanWallClimb(_canWallClimb);
        _newWall = _wallCheck.CheckNewWall(_wallCheck.GetFrontWallHit(), _playerWallClimbing._lastWall, _playerWallClimbing._lastWallNormal);
        _playerClimbJumping.ResetClimbJump(_wallFront, _newWall, _isGrounded);
        _playerDashing.CanDash(_isGrounded, _isSlope, _newWall);
        _canDash = _playerDashing.ReturnCanDash();
        _playerAnimation.SetIsAiming(_isAiming);
    }
}
