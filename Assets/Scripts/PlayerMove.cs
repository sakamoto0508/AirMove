using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private SlopeMoveDirection _slopeMoveDirection;
    private Rigidbody _rb;
    private Vector2 _currentInput;
    private float _currentSpeed;
    private Transform _playerCamera;
    private bool _isGrounded;
    private bool _isSlope;
    private float _groundDrag;
    private float _airMultiplier;
    private Vector3 _moveDirection;
    private float _currentHeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _slopeMoveDirection = GetComponent<SlopeMoveDirection>();
    }

    private void FixedUpdate()
    {
        if (_playerCamera == null) return;
        if (_isSlope)
        {
            OnSlopeMove();
        }
        if (_isGrounded)
        {
            GroundMove();
        }
        else if (!_isGrounded)
        {
            AirMove();
        }
        _rb.useGravity = !_isSlope;
    }

    private void OnSlopeMove()
    {
        //Todo
        if (_rb.angularVelocity.y > 0)
        {
            _rb.AddForce(Vector3.down * 98f, ForceMode.Force);
        }
    }

    private void GroundMove()
    {
        _moveDirection = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        float yVel = _rb.linearVelocity.y;
        Vector3 moveXz = _moveDirection.normalized * _currentSpeed;
        _rb.linearVelocity = new Vector3(moveXz.x, yVel, moveXz.z);
        GroundDamping();
    }

    private void AirMove()
    {
        _moveDirection = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        float yVel = _rb.linearVelocity.y;
        Vector3 moveXz = _moveDirection.normalized * _airMultiplier * _currentSpeed;
        _rb.linearVelocity = new Vector3(moveXz.x, yVel, moveXz.z);
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

    public void Move(Vector2 input, PlayerData playerData, bool isGround)
    {
        _currentInput = input;
        _playerCamera = playerData.MainCamera;
        _groundDrag = playerData.GroundDrag;
        _isGrounded = isGround;
        _airMultiplier = playerData.AirMultiplier;
    }

    public void Stop()
    {
        _currentInput = Vector2.zero;
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

    public void SetSlope(bool isSlope)
    {
        _isSlope = isSlope;
    }
}
