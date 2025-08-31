using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private RaycastHit _wallRightHit;
    private RaycastHit _wallLeftHit;
    private RaycastHit _wallFrontHit;
    private float _minWallNormalAngleChange;
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
    /// �O���ɕǂ����邩����
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool CheckForFrontWall(PlayerData playerData)
    {
        Debug.DrawRay(transform.position, playerData.SphereCastRadius * playerData.MainCamera.forward, Color.red);
        return Physics.SphereCast(transform.position, playerData.SphereCastRadius, playerData.MainCamera.forward,
            out _wallFrontHit, playerData.DetectingDistance, playerData.WallLayer);
    }

    /// <summary>
    /// �ǂ����Ă���p�x���擾
    /// </summary>
    /// <param name="playerData"></param>
    /// <param name="wallFrontHit"></param>
    /// <returns></returns>
    public float GetWallLookAngle(PlayerData playerData, RaycastHit wallFrontHit)
    {
        return Vector3.Angle(playerData.MainCamera.forward, -wallFrontHit.normal);
    }

    /// <summary>
    /// �V�����ǂɐG�ꂽ������
    /// </summary>
    /// <param name="wallFrontHit"></param>
    /// <param name="lastWall"></param>
    /// <param name="lastWallNormal"></param>
    /// <param name="minWallNormalAngleChange"></param>
    /// <returns></returns>
    public bool CheckNewWall(RaycastHit wallFrontHit, Transform lastWall, Vector3 lastWallNormal)
    {
        return wallFrontHit.transform != lastWall ||
            Mathf.Abs(Vector3.Angle(lastWallNormal, wallFrontHit.normal)) > _minWallNormalAngleChange;
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

    public void StartSetVariables(PlayerData playerData)
    {
        _minWallNormalAngleChange = playerData.MinWallNormalAngleChange;
    }
}
