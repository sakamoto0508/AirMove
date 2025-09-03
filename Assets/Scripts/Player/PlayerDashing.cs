using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
    public bool _isDashing { get; private set; } = false;
    public bool _canDash { get; private set; } = true;
    public bool _useCameraForward = true;
    public bool _allowAllDirection = false;
    public bool _resetVel = true;
    private float _dashForce;
    private float _dashUpForce;
    private float _dashDuration;
    private float _dashSpeed;
    private float _dashCooldown;
    private float _dashCooldownTimer;
    private float _dashFOV;
    private Rigidbody _rb;
    private Transform _playerCamera;
    private Vector3 _delayedForceToApply;
    private Vector3 _moveDirection;
    private Vector2 _currentInput;
    private CameraManager _cameraManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _cameraManager = FindAnyObjectByType<CameraManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (_playerCamera != null)
        {
            _moveDirection = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        }
    }

    public void Dash(PlayerData playerData)
    {
        if (_dashCooldownTimer > 0)
        {
            return;
        }
        else
        {
            _dashCooldownTimer = _dashCooldown;
        }
        _canDash = false;
        _isDashing = true;
        _cameraManager.DoFov(_dashFOV);
        Vector3 dashDirection;
        if (_currentInput.magnitude > 0.1f)
        {
            dashDirection = _moveDirection.normalized;
        }
        else
        {
            dashDirection = _useCameraForward ? _playerCamera.forward : transform.forward;
        }
        Vector3 forceToApply = dashDirection * _dashForce + playerData.MainCamera.up * _dashUpForce;
        _delayedForceToApply = forceToApply;
        Invoke(nameof(DelayedDashForced), 0.02f);
        Invoke(nameof(ResetDush), _dashDuration);
    }

    private void DelayedDashForced()
    {
        _rb.AddForce(_delayedForceToApply, ForceMode.Impulse);
    }

    private void ResetDush()
    {
        _isDashing = false;
        _cameraManager.DoFov(_cameraManager._defaultFov);
    }

    public void CanDash(bool isGrounded, bool isSlope, bool newWall)
    {
        if (isGrounded || isSlope || newWall)
        {
            _canDash = true;
        }
    }

    public void SetMoveInput(Vector2 input, PlayerData playerData)
    {
        _currentInput = input;
        _playerCamera = playerData.MainCamera;
    }

    public void MoveDirectionStop()
    {
        _currentInput = Vector2.zero;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _dashForce = playerData.DashForce;
        _dashUpForce = playerData.DashUpForce;
        _dashDuration = playerData.DashDuration;
        _dashSpeed = playerData.DashSpeed;
        _dashCooldown = playerData.DashCooldown;
        _playerCamera = playerData.MainCamera;
        _dashFOV = playerData.DashFOV;
    }

    public bool IsDashing()
    {
        return _isDashing;
    }

    public bool ReturnCanDash()
    {
        return _canDash;
    }
}
