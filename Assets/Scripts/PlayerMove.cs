using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector2 _currentInput;
    private float _currentSpeed;
    private Transform _playerCamera;
    private bool _isGrounded;
    private float _groundDrag;
    private float _airMultiplier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_playerCamera == null) return;
        if (_isGrounded)
        {
            GroundMove();
        }
        else if (!_isGrounded)
        {
            AirMove();
        }
    }

    private void GroundMove()
    {
        Vector3 inputDir = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        float yVel = _rb.linearVelocity.y;
        Vector3 moveXz = inputDir.normalized * _currentSpeed;
        _rb.linearVelocity = new Vector3(moveXz.x, yVel, moveXz.z);
        GroundDamping();
    }

    private void AirMove()
    {
        Vector3 inputDir = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        float yVel = _rb.linearVelocity.y;
        Vector3 moveXz = inputDir.normalized * _airMultiplier * _currentSpeed;
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

    public void Move(Vector2 input, PlayerData playerData, bool isGround, float speed)
    {
        _currentInput = input;
        _currentSpeed = speed;
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

}
