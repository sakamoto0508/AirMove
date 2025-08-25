using System.Collections;
using Unity.VisualScripting;
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
    //à⁄ìÆë¨ìxÇÃí≤êÆ
    private float _desireMoveSpeed;
    //ç≈èIäÛñ]à⁄ìÆë¨ìx
    private float _lastDesiredMoveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_playerCamera == null) return;
        Vector3 inputDir = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        inputDir.y = 0;
        _moveDirection = inputDir;
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
        _rb.useGravity = !_isSlope;
        if (!_isSliding)
        {
            SpeedControl();
        }
    }

    private void SlidingMovement()
    {
        if (_isSlope)
        {
            _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _slidingForce, ForceMode.Force);
            if (_rb.linearVelocity.y > 0)
            {
                _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }
        else if (!_isSlope || _rb.angularVelocity.y > -0.1f)
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
                _rb.linearVelocity = _rb.linearVelocity.normalized * _currentSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            if (flatVel.magnitude > _currentSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * _currentSpeed;
                _rb.linearVelocity = new Vector3(limitedVel.x, _rb.linearVelocity.y, limitedVel.z);
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
                _desireMoveSpeed = playerData.WalkSpeed;
                break;
            case PlayerState.State.sprinting:
                _desireMoveSpeed = playerData.SprintSpeed;
                break;
            case PlayerState.State.crouching:
                _desireMoveSpeed = playerData.CrouchSpeed;
                break;
            case PlayerState.State.sliding:
                if (_isSlope && _rb.angularVelocity.y < 0.1f)
                {
                    _desireMoveSpeed = playerData.SprintSpeed;
                }
                else
                {
                    _desireMoveSpeed = playerData.SprintSpeed;
                }
                break;
            default:
                _desireMoveSpeed = playerData.WalkSpeed;
                break;
        }

        if(Mathf.Abs(_desireMoveSpeed-_lastDesiredMoveSpeed)>4f&& _currentSpeed != 0)
        {
            StopAllCoroutines();
            StartCoroutine(SmoothlyLerpMoveSpeed());
        }
        else
        {
            _currentSpeed = _desireMoveSpeed;
        }
        _lastDesiredMoveSpeed = _desireMoveSpeed;
    }

    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0f;
        float difference = Mathf.Abs(_desireMoveSpeed - _currentSpeed);
        float startValue = _currentSpeed;
        while (time < difference)
        {
            _currentSpeed=Mathf.Lerp(startValue, _desireMoveSpeed, time / difference);
            time += Time.deltaTime;
            yield return null;
        }
        _currentSpeed = _desireMoveSpeed;
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
