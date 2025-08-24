using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector2 _currentInput;
    private float _currentSpeed;
    private Transform _playerCamera;
    private bool _isGrounded;
    private bool _isSlope;
    private float _groundDrag;
    private float _airMultiplier;
    private Vector3 _moveDirection;
    private RaycastHit _slopeHit;
    private bool _isSliding = false;
    private float _slidingForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_playerCamera == null) return;
        _moveDirection = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        if (_isSliding)
        {
            SlidingMovement();
        }
        else if (_isSlope)
        {
            OnSlopeMove();
        }
        else if (_isGrounded)
        {
            GroundMove();
        }
        else if (!_isGrounded)
        {
            AirMove();
        }
        SpeedControl();
    }

    private void SlidingMovement()
    {
        _moveDirection = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        _moveDirection = _moveDirection.normalized;
        if (_moveDirection.magnitude > 0.1f)
        {
            _rb.AddForce(_moveDirection * _slidingForce, ForceMode.Force);
        }
    }

    private void OnSlopeMove()
    {
        _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _currentSpeed * 20f, ForceMode.Force);
        if (_rb.linearVelocity.y > 0)
        {
            _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
    }

    private void GroundMove()
    {
        _rb.AddForce(_moveDirection.normalized * _currentSpeed * 10f, ForceMode.Force);
        GroundDamping();
    }

    private void AirMove()
    {
        _rb.AddForce(_moveDirection.normalized * _currentSpeed * 10f * _airMultiplier, ForceMode.Force);
    }

    private void GroundDamping()
    {
        if (_isGrounded)
        {
            _rb.linearDamping = _groundDrag;
        }
        else
        {
            _rb.linearDamping = 0f;
        }
    }

    private void SpeedControl()
    {
        if (_isSlope)
        {
            if (_rb.linearVelocity.magnitude > _currentSpeed)
            {
                _rb.linearVelocity=_rb.linearVelocity.normalized * _currentSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            if (flatVel.magnitude > _currentSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _currentSpeed;
                _rb.linearVelocity=new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
            }
        }
    }

    public void Move(Vector2 input, PlayerData playerData, bool isGround)
    {
        _currentInput = input;
        _playerCamera = playerData.MainCamera;
        _groundDrag = playerData.GroundDrag;
        _isGrounded = isGround;
        _airMultiplier = playerData.AirMultiplier;
        _slidingForce = playerData.SlidingForce;
    }

    public void Stop()
    {
        _currentInput = Vector2.zero;
        _rb.linearVelocity = Vector3.zero;
    }

    public void UpdateSpeed(PlayerState playerState, PlayerData playerData)
    {
        if (playerState == null || playerData == null) return;

        switch (playerState.CurrentState)
        {
            case PlayerState.State.walking:
                _currentSpeed = playerData.WalkSpeed;
                break;
            case PlayerState.State.sprinting:
                _currentSpeed = playerData.SprintSpeed;
                break;
            case PlayerState.State.crouching:
                _currentSpeed = playerData.CrouchSpeed;
                break;
            default:
                _currentSpeed = playerData.WalkSpeed;
                break;
        }
    }

    public void SetSlope(bool isSlope, RaycastHit slopeHit)
    {
        _isSlope = isSlope;
        _slopeHit = slopeHit;
    }

    public void SetSliding(bool isSliding)
    {
        _isSliding = isSliding;
    }

    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }
}
