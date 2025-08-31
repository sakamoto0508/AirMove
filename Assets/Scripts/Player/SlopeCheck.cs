using UnityEngine;

public class SlopeCheck : MonoBehaviour
{
    private RaycastHit _slopeHit;
    [SerializeField] private float _checkDistance = 0.3f;

    /// <summary>
    /// �⓹������
    /// </summary>
    /// <param name="playerData"></param>
    /// <returns></returns>
    public bool OnSlope(PlayerData playerData)
    {
        if (Physics.Raycast(transform.position, Vector3.down,
            out _slopeHit, playerData.PlayerHeight * 0.5f + _checkDistance))
        {
            float angle = Vector3.Angle(Vector3.up, _slopeHit.normal);
            return angle < playerData.MaxSlopeAngle && angle != 0f;
        }
        return false;
    }

    /// <summary>
    /// �⓹�̃��C�L���X�g���擾
    /// </summary>
    /// <returns></returns>
    public RaycastHit GetSlopeHit()
    {
        return _slopeHit;
    }
}
