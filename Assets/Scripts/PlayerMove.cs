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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_playerCamera == null) return;
        Vector3 inputDir = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        float yVel = _rb.linearVelocity.y;
        Vector3 moveXz = inputDir.normalized * _currentSpeed;
        _rb.linearVelocity = new Vector3(moveXz.x, yVel, moveXz.z);

        if(_isGrounded)
        {
            _rb.linearDamping = _groundDrag;
        }
        else
        {
            _rb.linearDamping = 0f;
        }
    }

    public void Move(Vector2 input, PlayerData playerData ,bool isGround)
    {
        _currentInput = input;
        _currentSpeed = playerData.WalkSpeed;
        _playerCamera = playerData.MainCamera;
        _groundDrag = playerData.GroundDrag;
        _isGrounded = isGround;
    }

    public void Stop()
    {
        _currentInput = Vector2.zero;
    }

}
