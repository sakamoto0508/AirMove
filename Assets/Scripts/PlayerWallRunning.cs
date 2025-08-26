using UnityEngine;
using UnityEngine.Playables;

public class PlayerWallRunning : MonoBehaviour
{
    private Rigidbody _rb;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    private bool _isWallRunning;
    private bool _wallLeft;
    private bool _wallRight;
    private bool _aboveGround;
    private Transform _playerCamera;
    private Vector2 _currentInput;
    private float _wallRunForce;
    private float _minJumpHeight;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if ((_wallLeft || _wallRight) && _currentInput.magnitude > 0 && _aboveGround)
        {
            if (!_isWallRunning)
            {
                StartWallRun();
            }
        }
        else
        {
            if (_isWallRunning)
            {
                StopWallRun();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isWallRunning)
        {
            WallRunningMovement();
        }
    }

    public void StartWallRun()
    {
        _isWallRunning = true;
        //_previousState = playerState.CurrentState;
        //_playerState.CurrentState = PlayerState.State.wallrunning;
    }

    public void StopWallRun()
    {
        _isWallRunning = false;
        //_playerState.CurrentState = _previousState;
    }

    private void WallRunningMovement()
    {
        _rb.useGravity = false;
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        Vector3 wallNormal = _wallRight ? _rightWallHit.normal : _leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up);
        _rb.AddForce(wallForward * _wallRunForce, ForceMode.Force);
    }

    public void WallRunningMove(Vector2 input, PlayerData playerData, bool wallLeft, bool wallRight, RaycastHit rightWallHit, RaycastHit leftWallHit)
    {
        _currentInput = input;
        _wallRunForce = playerData.WallRunForce;
        _wallLeft = wallLeft;
        _wallRight = wallRight;
        _leftWallHit = leftWallHit;
        _rightWallHit = rightWallHit;
    }

    public bool IsWallRunning()
    {
        return _isWallRunning;
    }

    public bool AboveGround(PlayerData playerData)
    {
        return !Physics.Raycast(transform.position, Vector3.down, playerData.MinJumpHeight, playerData.GroundLayer);
    }

    public void SetAboveGround(bool aboveGround)
    {
        _aboveGround = aboveGround;
    }
}
