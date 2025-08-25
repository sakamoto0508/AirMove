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

    //“Ç‚ÝŽæ‚èê—p
    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float CrouchSpeed => _crouchSpeed;
    public float SlidingSpeed => _slidingSpeed;
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
    
}
