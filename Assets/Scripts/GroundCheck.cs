using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    /// <summary>
    /// �����̗]�T���������邽�߂̒l
    /// </summary>
    [SerializeField] private float _groundCheckDistance = 0.2f;
    public bool IsGrounded(PlayerData playerData)
    {
        return Physics.Raycast(transform.position, Vector3.down,
            playerData.PlayerHeight * 0.5f + _groundCheckDistance, playerData.GroundLayer);
    }
}
