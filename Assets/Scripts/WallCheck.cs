using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private RaycastHit _wallRightHit;
    private RaycastHit _wallLeftHit;

    /// <summary>
    /// �E���ɕǂ����邩����
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool CheckForRightWall(PlayerData playerData)
    {
        Debug.DrawRay(transform.position, playerData.MainCamera.right * playerData.WallCheckDistance, Color.red);

        return Physics.Raycast(transform.position, playerData.MainCamera.right,
            out _wallRightHit, playerData.WallCheckDistance, playerData.WallLayer);
    }

    /// <summary>
    /// �����ɕǂ����邩����
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool CheckForLeftWall(PlayerData playerData)
    {
        Debug.DrawRay(transform.position, -playerData.MainCamera.right * playerData.WallCheckDistance, Color.red);
        return Physics.Raycast(transform.position, -playerData.MainCamera.right,
            out _wallLeftHit, playerData.WallCheckDistance, playerData.WallLayer);
    }

    /// <summary>
    /// �n�ʂ��痣��Ă��邩����
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool AboveGround(PlayerData playerData)
    {
        return !Physics.Raycast(transform.position, Vector3.down,
            playerData.MinJumpHeight, playerData.GroundLayer);
    }

    /// <summary>
    /// �E���̕ǂ̃��C�L���X�g���擾
    /// </summary>
    /// <returns></returns>
    public RaycastHit GetRightWallHit()
    {
        return _wallRightHit;
    }

    /// <summary>
    /// �����̕ǂ̃��C�L���X�g���擾
    /// </summary>
    /// <returns></returns>
    public RaycastHit GetLeftWallHit()
    {
        return _wallLeftHit;
    }
}
