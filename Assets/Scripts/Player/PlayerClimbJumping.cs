using UnityEngine;

public class PlayerClimbJumping : MonoBehaviour
{
    private float _wallClimbJumpUpForce;
    private float _wallClimbJumpBackForce;
    private int _climbJumps;
    private int _climbJumpCount = 1;
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ClimbJumping(RaycastHit wallFront)
    {
        if (_climbJumpCount <= 0) return;
        Vector3 forceToApply = transform.up * _wallClimbJumpUpForce + wallFront.normal * _wallClimbJumpBackForce;
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(forceToApply, ForceMode.Impulse);
        _climbJumpCount--;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _wallClimbJumpUpForce = playerData.WallClimbJumpUpForce;
        _wallClimbJumpBackForce = playerData.WallClimbJumpBackForce;
        _climbJumps = playerData.ClimbJumps;
    }

    public void ResetClimbJump(bool wallFront, bool newWall, bool isGrounded)
    {
        if ((wallFront && newWall) || isGrounded)
        {
            _climbJumpCount = _climbJumps;
        }
    }
}
