using UnityEngine;

public class WallCheck : MonoBehaviour
{
    private RaycastHit _rightWallHit;
    private RaycastHit _leftWallHit;

    public bool CheckForRightWall(PlayerData playerData)
    {
        Debug.DrawRay(transform.position, playerData.MainCamera.right * playerData.WallCheckDistance, Color.red);

        return Physics.Raycast(transform.position, playerData.MainCamera.right,
            out _rightWallHit, playerData.WallCheckDistance, playerData.WallLayer);
    }

    public bool CheckForLeftWall(PlayerData playerData)
    {
        Debug.DrawRay(transform.position, -playerData.MainCamera.right * playerData.WallCheckDistance, Color.red);
        return Physics.Raycast(transform.position, -playerData.MainCamera.right,
            out _leftWallHit, playerData.WallCheckDistance, playerData.WallLayer);
    }

    public bool AboveGround(PlayerData playerData)
    {
        return !Physics.Raycast(transform.position, Vector3.down,
            playerData.MinJumpHeight, playerData.GroundLayer);
    }

    public RaycastHit GetRightWallHit()
    {
        return _rightWallHit;
    }
    public RaycastHit GetLeftWallHit()
    {
        return _leftWallHit;
    }
}
