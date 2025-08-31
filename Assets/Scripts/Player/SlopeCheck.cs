using UnityEngine;

public class SlopeCheck : MonoBehaviour
{
    private RaycastHit _slopeHit;
    [SerializeField] private float _checkDistance = 0.3f;

    /// <summary>
    /// 坂道か判定
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
    /// 坂道のレイキャストを取得
    /// </summary>
    /// <returns></returns>
    public RaycastHit GetSlopeHit()
    {
        return _slopeHit;
    }
}
