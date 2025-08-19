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


    public void Jump(PlayerData playerData, bool isGrounded)
    {
        if (!isGrounded || !_canJump) return;
        _rb.AddForce(Vector3.up * playerData.JumpForce, ForceMode.Impulse);
        _canJump = false;
        Invoke(nameof(ResetJump), playerData.JumpCooldown);
    }

    private void ResetJump()
    {
        _canJump = true;
    }
}
