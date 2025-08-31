using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private RaycastHit _wallRightHit;
    private RaycastHit _wallLeftHit;
    private RaycastHit _wallFrontHit;

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

    public bool CheckForFrontWall(PlayerData playerData)
    {
        Debug.DrawRay(transform.position, playerData.SphereCastRadius * playerData.MainCamera.forward, Color.red);
        return Physics.SphereCast(transform.position, playerData.SphereCastRadius, playerData.MainCamera.forward,
            out _wallFrontHit, playerData.DetectingDistance, playerData.WallLayer);
    }

    public float GetWallLookAngle(PlayerData playerData,RaycastHit wallFrontHit)
    {
        return Vector3.Angle(playerData.MainCamera.forward, -wallFrontHit.normal);
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

    /// <summary>
    /// �O���̕ǂ̃��C�L���X�g���擾
    /// </summary>
    /// <returns></returns>
    public RaycastHit GetFrontWallHit()
    {
        return _wallFrontHit;
    }
}
