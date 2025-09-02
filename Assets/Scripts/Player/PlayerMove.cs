using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private float _currentSpeed;
    private float _groundDrag;
    private float _airMultiplier;
    private float _slidingForce;
    //移動速度の調整
    private float _desireMoveSpeed;
    //最終希望移動速度
    private float _lastDesiredMoveSpeed;
    private float _speedIncreaseMultiplier = 1.5f;
    private float _slopeIncreaseMultiplier = 2.5f;
    private bool _isGrounded;
    private bool _isSlope;
    private bool _isSliding = false;
    private bool _isDashing = false;
    private Rigidbody _rb;
    private Vector2 _currentInput;
    private Transform _playerCamera;
    private Vector3 _moveDirection;
    private RaycastHit _slopeHit;

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
        if (_isDashing)
        {
            return;
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

    /// <summary>
    /// スライディングの動き
    /// </summary>
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

    /// <summary>
    /// 坂道の動き
    /// </summary>
    private void OnSlopeMove()
    {
        _rb.AddForce(GetSlopeMoveDirection(_moveDirection) * _currentSpeed * 20f, ForceMode.Force);
        if (_rb.linearVelocity.y > 0)
        {
            _rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
    }

    /// <summary>
    /// 地上の動き
    /// </summary>
    private void GroundMove()
    {
        _rb.AddForce(_moveDirection.normalized * _currentSpeed * 10f, ForceMode.Force);
        GroundDamping();
    }

    /// <summary>
    /// 空中の動き
    /// </summary>
    private void AirMove()
    {
        _rb.AddForce(_moveDirection.normalized * _currentSpeed * 10f * _airMultiplier, ForceMode.Force);
    }

    /// <summary>
    /// 地上にいるときの減衰
    /// </summary>
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

    /// <summary>
    /// 速度制限
    /// </summary>
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

    /// <summary>
    /// 移動
    /// </summary>
    /// <param name="input"></param>
    /// <param name="playerData"></param>
    /// <param name="isGround"></param>
    public void Move(Vector2 input, PlayerData playerData, bool isGround)
    {
        _currentInput = input;
        _playerCamera = playerData.MainCamera;
        _groundDrag = playerData.GroundDrag;
        _isGrounded = isGround;
        _airMultiplier = playerData.AirMultiplier;
        _slidingForce = playerData.SlidingForce;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        _currentInput = Vector2.zero;
        _rb.linearVelocity = Vector3.zero;
    }

    /// <summary>
    /// 速度更新
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="playerData"></param>
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
                    _desireMoveSpeed = playerData.SlidingSpeed;
                }
                else
                {
                    _desireMoveSpeed = playerData.SprintSpeed;
                }
                break;
            case PlayerState.State.wallrunning:
                _desireMoveSpeed = playerData.WallRunningSpeed;
                break;
            case PlayerState.State.wallclimbing:
                _desireMoveSpeed = playerData.ClimbingSpeed;
                break;
            case PlayerState.State.dashing:
                _desireMoveSpeed = playerData.DashSpeed;
                break;
            default:
                _desireMoveSpeed = playerData.WalkSpeed;
                break;
        }
        if (Mathf.Abs(_desireMoveSpeed - _lastDesiredMoveSpeed) > 4f && _currentSpeed != 0)
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

    /// <summary>
    /// 速度を滑らかに変化させる
    /// </summary>
    /// <returns></returns>
    private IEnumerator SmoothlyLerpMoveSpeed()
    {
        float time = 0f;
        float difference = Mathf.Abs(_desireMoveSpeed - _currentSpeed);
        float startValue = _currentSpeed;
        while (time < difference)
        {
            _currentSpeed = Mathf.Lerp(startValue, _desireMoveSpeed, time / difference);
            if (_isSlope)
            {
                float slopeAngle = Vector3.Angle(Vector3.up, _slopeHit.normal);
                float slopeAngleIncrease = 1 + (slopeAngle / 90f);
                time += Time.deltaTime * _speedIncreaseMultiplier * _slopeIncreaseMultiplier * slopeAngleIncrease;
            }
            else
            {
                time += Time.deltaTime;
            }
            yield return null;
        }
        _currentSpeed = _desireMoveSpeed;
    }

    /// <summary>
    /// 坂道の設定
    /// </summary>
    /// <param name="isSlope"></param>
    /// <param name="slopeHit"></param>
    public void SetSlope(bool isSlope, RaycastHit slopeHit)
    {
        _isSlope = isSlope;
        _slopeHit = slopeHit;
    }

    /// <summary>
    /// スライディングの設定
    /// </summary>
    /// <param name="isSliding"></param>
    public void SetSliding(bool isSliding)
    {
        _isSliding = isSliding;
    }

    public void SetDashing(bool isDashing)
    {
        _isDashing = isDashing;
    }

    /// <summary>
    /// 坂道の移動方向を取得
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction, _slopeHit.normal).normalized;
    }
}
