using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _mainCamera;

    [Header("movement")]
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private float _sprintSpeed = 15f;
    [SerializeField] private float _crouchSpeed = 5f;
    [SerializeField] private float _slidingSpeed = 20f;
    [SerializeField] private float _wallRunningSpeed = 15f;
    [SerializeField] private float _groundDrag = 0.5f;

    [Header("GroundCheck")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Jump")]
    [SerializeField] private float _jumpForce = 5f;
    [SerializeField] private float _jumpSlopeForce = 10f;
    [SerializeField] private float _jumpCooldown = 0.25f;
    [SerializeField] private float _airMultiplier = 0.5f;

    [Header("Crouching")]
    [SerializeField] private float _crouchHeight = 0.5f;

    [Header("Slope Handling")]
    [SerializeField] private float _maxSlopeAngle = 45f;

    [Header("Sliding")]
    [SerializeField] private float _maxSlidingTime = 1.5f;
    [SerializeField] private float _slidingForce = 10f;
    [SerializeField] private float _slidingYScale = 0.5f;

    [Header("WallRunning")]
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private float _wallRunForce = 10f;
    [SerializeField] private float _maxWallRunTime;
    [SerializeField] private float _wallJumpUpForce;
    [SerializeField] private float _wallJumpSideForce;
    [SerializeField] private float _exitWallTime;
    [SerializeField] private float _cameraFOV=90f;
    [SerializeField] private float _cameraTiltAngle=5f;

    [Header("WallClimbing")]
    [SerializeField] private float _climbingSpeed;

    [Header("WallClimbJumping")]
    [SerializeField] private float _wallClimbJumpUpForce;
    [SerializeField] private float _wallClimbJumpSideForce;

    [Header("Detection")]
    [SerializeField] private float _wallCheckDistance;
    [SerializeField] private float _minJumpHeight;
    [SerializeField] private float _wallClimbSpeed = 3f;
    [SerializeField] private float _detectingDistance;
    [SerializeField] private float _sphereCastRadius;
    [SerializeField] private float _maxWallLookAngle;

    //“Ç‚ÝŽæ‚èê—p
    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float CrouchSpeed => _crouchSpeed;
    public float SlidingSpeed => _slidingSpeed;
    public float WallRunningSpeed => _wallRunningSpeed;
    public float GroundDrag => _groundDrag;
    public Transform MainCamera => _mainCamera;
    public float PlayerHeight => _playerHeight;
    public LayerMask GroundLayer => _groundLayer;
    public float JumpForce => _jumpForce;
    public float JumpSlopeForce => _jumpSlopeForce;
    public float JumpCooldown => _jumpCooldown;
    public float AirMultiplier => _airMultiplier;
    public float CrouchHeight => _crouchHeight;
    public float MaxSlopeAngle => _maxSlopeAngle;
    public float MaxSlidingTime => _maxSlidingTime;
    public float SlidingForce => _slidingForce;
    public float SlidingYScale => _slidingYScale;
    public float WallRunForce => _wallRunForce;
    public float MaxWallRunTime => _maxWallRunTime;
    public LayerMask WallLayer => _wallLayer;
    public float WallCheckDistance => _wallCheckDistance;
    public float MinJumpHeight => _minJumpHeight;
    public float WallClimbSpeed => _wallClimbSpeed;
    public float WallJumpUpForce => _wallJumpUpForce;
    public float WallJumpSideForce => _wallJumpSideForce;
    public float ExitWallTime => _exitWallTime;
    public float CameraFOV => _cameraFOV;
    public float CameraTiltAngle => _cameraTiltAngle;
    public float ClimbingSpeed => _climbingSpeed;
    public float DetectingDistance => _detectingDistance;
    public float SphereCastRadius => _sphereCastRadius;
    public float MaxWallLookAngle => _maxWallLookAngle;
    public float WallClimbJumpUpForce => _wallClimbJumpUpForce;
    public float WallClimbJumpSideForce => _wallClimbJumpSideForce;
}
