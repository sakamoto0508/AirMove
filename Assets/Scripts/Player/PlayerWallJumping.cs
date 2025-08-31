using UnityEngine;

public class PlayerWallJumping : MonoBehaviour
{
    private Rigidbody _rb;
    private bool _wallLeft;
    private bool _wallRight;
    public bool ExitingWall { get;private set; } = false;
    private float _exitWallTime;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        if(ExitingWall)
        {
            if (_exitWallTime > 0)
            {
                _exitWallTime -= Time.deltaTime;
            }
            if (_exitWallTime <= 0)
            {
                ExitingWall = false;
            }
        }
    }

    /// <summary>
    /// 壁ジャンプ
    /// </summary>
    /// <param name="wallLeftHit"></param>
    /// <param name="wallRightHit"></param>
    /// <param name="playerData"></param>
    public void WallJump(RaycastHit wallLeftHit, RaycastHit wallRightHit, PlayerData playerData)
    {
        ExitingWall = true;
        _exitWallTime = playerData.ExitWallTime;
        Vector3 wallNormal = _wallRight ? wallRightHit.normal : wallLeftHit.normal; ;
        Vector3 forceToApply = transform.up * playerData.WallJumpUpForce + wallNormal * playerData.WallJumpSideForce;
        _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, 0f, _rb.linearVelocity.z);
        _rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    /// <summary>
    /// 壁の左右の判定をセット
    /// </summary>
    /// <param name="leftWall"></param>
    /// <param name="rightWall"></param>
    public void SetWallCheck(bool leftWall, bool rightWall)
    {
        _wallLeft = leftWall;
        _wallRight = rightWall;
    }


    /// <summary>
    /// 壁から離れているか返す
    /// </summary>
    /// <returns></returns>
    public bool ReturnExitingWall()
    {
        return ExitingWall;
    }
}
