using UnityEngine;
using UnityEngine.Playables;

public class PlayerWallRunning : MonoBehaviour
{
    private Rigidbody _rb;
    private RaycastHit _leftWallHit;
    private RaycastHit _rightWallHit;
    public bool _isWallRunning { get; private set; } = false;
    private bool _wallLeft;
    private bool _wallRight;
    private bool _aboveGround;
    private Transform _playerCamera;
    private Vector2 _currentInput;
    private float _wallRunForce;
    private KeyCode upwardsRunKey = KeyCode.LeftShift;
    private KeyCode downwardsRunKey = KeyCode.LeftControl;
    private bool _upwardsRunning;
    private bool _downwardsRunning;
    private float _climbSpeed;
    private bool _canWallMove;
    private bool _exitingWall;
    private WallRunningCamera _camera;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _camera=FindAnyObjectByType<WallRunningCamera>();
    }

    private void Update()
    {
        _upwardsRunning = Input.GetKey(upwardsRunKey);
        _downwardsRunning = Input.GetKey(downwardsRunKey);
        if (_canWallMove && !_exitingWall)
        {
            if (!_isWallRunning)
            {
                StartWallRun();
            }
        }
        else if(_exitingWall)
        {
            if (_isWallRunning)
            {
                StopWallRun();
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
        _camera.DoFov(90f);
        if (_wallLeft)
        {
            _camera.DoTilt(-5f);
            Debug.Log("WallRunLeft");
        }
        else if (_wallRight)
        {
            _camera.DoTilt(5f);
            Debug.Log("WallRunRight");
        }
    }

    public void StopWallRun()
    {
        if (!_isWallRunning) return;
        _isWallRunning = false;
        _rb.useGravity = true;
        _camera.DoFov(80f);
        _camera.DoTilt(0f);
    }

    private void WallRunningMovement()
    {
        _rb.useGravity = false;
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        Vector3 wallNormal = _wallRight ? _rightWallHit.normal : _leftWallHit.normal;
        Vector3 wallForward = Vector3.Cross(wallNormal, Vector3.up);
        if ((_playerCamera.forward - wallForward).magnitude > (_playerCamera.forward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        _rb.AddForce(wallForward * _wallRunForce, ForceMode.Force);
        if (_upwardsRunning)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _climbSpeed, _rb.linearVelocity.z);
        }
        if (_downwardsRunning)
        {
            _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, -_climbSpeed, _rb.linearVelocity.z);
        }
        // •Ç‚Éˆø‚«Šñ‚¹‚ç‚ê‚é—Í
        if (!(_wallLeft && _currentInput.x > 0) && !(_wallRight && _currentInput.x < 0))
        {
            _rb.AddForce(-wallNormal * 10f, ForceMode.Force);
        }
    }

    public void WallRunningMove(Vector2 input, PlayerData playerData, RaycastHit wallRightHit, RaycastHit wallLeftHit)
    {
        _currentInput = input;
        _wallRunForce = playerData.WallRunForce;
        _leftWallHit = wallLeftHit;
        _rightWallHit = wallRightHit;
        _playerCamera = playerData.MainCamera;
        _climbSpeed = playerData.WallClimbSpeed;
    }

    public void MoveStop()
    {
        _currentInput = Vector2.zero;
        _rb.linearVelocity = Vector3.zero;
        if (_isWallRunning)
        {
            StopWallRun();
        }
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

    public void SetWallCheck(bool leftWall, bool rightWall)
    {
        _wallLeft = leftWall;
        _wallRight = rightWall;
    }

    public void SetCanWallMove(bool canWallMove)
    {
        _canWallMove = canWallMove;
    }

    public void SetExitWall(bool exitingWall)
    {
        _exitingWall = exitingWall;
    }
}
