using UnityEngine;

public class WallMoveCheck : MonoBehaviour
{
    /// <summary>
    /// �ǈړ����\������
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
}
