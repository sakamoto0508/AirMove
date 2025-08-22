using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _canJump = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }


    public void Jump(PlayerData playerData, bool isGrounded, bool isSlope)
    {
        if ((isGrounded || isSlope) && _canJump)
        {
            _canJump = false;
            _rb.linearVelocity=new Vector3 (_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
            _rb.AddForce(Vector3.up * playerData.JumpForce, ForceMode.Impulse);
            Invoke(nameof(ResetJump), playerData.JumpCooldown);
        }
    }

    private void ResetJump()
    {
        _canJump = true;
    }
}
