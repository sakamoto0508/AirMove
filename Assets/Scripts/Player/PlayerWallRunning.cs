using System.Collections;
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
    private CameraManager _wallRunningCamera;
    private float _cameraFOV;
    private float _cameraTiltAngle;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _wallRunningCamera = FindAnyObjectByType<CameraManager>();
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
        else if (_exitingWall)
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

    /// <summary>
    /// �Ǒ���J�n
    /// </summary>
    public void StartWallRun()
    {
        _isWallRunning = true;
        _wallRunningCamera.DoFov(_cameraFOV);
        if (_wallLeft)
        {
            _wallRunningCamera.DoTilt(-_cameraTiltAngle);
        }
        else if (_wallRight)
        {
            _wallRunningCamera.DoTilt(_cameraTiltAngle);
        }
    }

    /// <summary>
    /// �Ǒ���I��
    /// </summary>
    public void StopWallRun()
    {
        if (!_isWallRunning) return;
        _isWallRunning = false;
        _rb.useGravity = true;
        _wallRunningCamera.DoFov(_wallRunningCamera._defaultFov);
        _wallRunningCamera.DoTilt(_wallRunningCamera._defaultTilt);
    }

    /// <summary>
    /// �Ǒ��蒆�̓���
    /// </summary>
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
        // �ǂɈ����񂹂����
        if (!(_wallLeft && _currentInput.x > 0) && !(_wallRight && _currentInput.x < 0))
        {
            _rb.AddForce(-wallNormal * 10f, ForceMode.Force);
        }
    }


    public void WallRunningMove(Vector2 input, RaycastHit wallRightHit, RaycastHit wallLeftHit)
    {
        _currentInput = input;
        _leftWallHit = wallLeftHit;
        _rightWallHit = wallRightHit;
    }

    /// <summary>
    /// �Ǒ����~
    /// </summary>
    public void MoveStop()
    {
        _currentInput = Vector2.zero;
        _rb.linearVelocity = Vector3.zero;
        if (_isWallRunning)
        {
            StopWallRun();
        }
    }

    /// <summary>
    /// �Ǒ��蒆���Ԃ�
    /// </summary>
    /// <returns></returns>
    public bool IsWallRunning()
    {
        return _isWallRunning;
    }

    /// <summary>
    /// �n�ʂ��痣��Ă��邩�Ԃ�
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool AboveGround(PlayerData playerData)
    {
        return !Physics.Raycast(transform.position, Vector3.down, playerData.MinJumpHeight, playerData.GroundLayer);
    }

    /// <summary>
    /// �n�ʂ��痣��Ă��邩�Z�b�g
    /// </summary>
    /// <param name="aboveGround"></param>
    public void SetAboveGround(bool aboveGround)
    {
        _aboveGround = aboveGround;
    }

    /// <summary>
    /// �ǂ̍��E�̔�����Z�b�g
    /// </summary>
    /// <param name="leftWall"></param>
    /// <param name="rightWall"></param>
    public void SetWallCheck(bool leftWall, bool rightWall)
    {
        _wallLeft = leftWall;
        _wallRight = rightWall;
    }

    /// <summary>
    /// �ǈړ����\���Z�b�g
    /// </summary>
    /// <param name="canWallMove"></param>
    public void SetCanWallMove(bool canWallMove)
    {
        _canWallMove = canWallMove;
    }

    /// <summary>
    /// �ǂ��痣��Ă��邩�Z�b�g
    /// </summary>
    /// <param name="exitingWall"></param>
    public void SetExitWall(bool exitingWall)
    {
        _exitingWall = exitingWall;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _cameraFOV = playerData.CameraFOV;
        _cameraTiltAngle = playerData.CameraTiltAngle;
        _playerCamera = playerData.MainCamera;
        _climbSpeed = playerData.WallClimbSpeed;
        _wallRunForce = playerData.WallRunForce;
    }
}
