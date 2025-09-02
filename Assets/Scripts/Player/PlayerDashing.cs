using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
    public bool _isDashing { get; private set; } = false;
    public bool _canDash { get; private set; } = true;
    private float _dashForce;
    private float _dashUpForce;
    private float _dashDuration;
    private float _dashSpeed;
    private float _dashCooldown;
    private float _dashCooldownTimer;
    private Rigidbody _rb;
    private Vector3 _delayedForceToApply;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_dashCooldownTimer > 0)
        {
            _dashCooldownTimer -= Time.deltaTime;
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
        Vector3 forceToApply = playerData.MainCamera.forward * _dashForce + playerData.MainCamera.up * _dashUpForce;
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
    }

    public void CanDash(bool isGrounded, bool isSlope, bool newWall)
    {
        if (isGrounded || isSlope || newWall)
        {
            _canDash = true;
        }
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _dashForce = playerData.DashForce;
        _dashUpForce = playerData.DashUpForce;
        _dashDuration = playerData.DashDuration;
        _dashSpeed = playerData.DashSpeed;
        _dashCooldown = playerData.DashCooldown;
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
