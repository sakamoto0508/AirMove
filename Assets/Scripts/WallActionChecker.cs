using UnityEngine;

public class WallActionChecker : MonoBehaviour
{
    private float _maxLookAngled;
    /// <summary>
    /// •ÇˆÚ“®‚ª‰Â”\‚©”»’è
    /// </summary>
    /// <param name="wallLeft"></param>
    /// <param name="wallRight"></param>
    /// <param name="input"></param>
    /// <param name="AboveGround"></param>
    /// <returns></returns>
    public bool CanWallMove(bool wallLeft,bool wallRight, Vector2 input,bool AboveGround)
    {
        return (wallLeft || wallRight) && input.magnitude > 0 && AboveGround;
    }

    public bool CanWallClimb(bool wallFront, Vector2 input, float wallLookAngle)
    {
        return wallFront && input.y > 0 && wallLookAngle < _maxLookAngled;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _maxLookAngled = playerData.MaxWallLookAngle;
    }
}
