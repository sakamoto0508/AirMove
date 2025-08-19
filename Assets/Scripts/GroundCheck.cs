using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    /// <summary>
    /// ‘«Œ³‚Ì—]—T‚ğ‚½‚¹‚é‚½‚ß‚Ì’l
    /// </summary>
    [SerializeField] private float _groundCheckDistance = 0.2f;
    public bool IsGrounded(PlayerData playerData)
    {
        return Physics.Raycast(transform.position, Vector3.down,
            playerData.PlayerHeight * 0.5f + _groundCheckDistance, playerData.GroundLayer);
    }
}
