using UnityEngine;
public class PlayerSliding : MonoBehaviour
{
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private float _startYScale;
    private float _slideTimer = 0f;
    private bool _isSlope = false;
    public bool _isSliding { get; private set; } = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _startYScale = _capsuleCollider.height;
    }

    private void Update()
    {
        if (_isSliding)
        {
            UpdateSlidingTimer();
        }
    }

    /// <summary>
    /// スライディングの開始
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="playerData"></param>
    public void StartSliding(PlayerState playerState, PlayerData playerData)
    {
        if (_isSliding) return;
        _isSliding = true;
        _capsuleCollider.height = playerData.SlidingYScale;
        _rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        _slideTimer = playerData.MaxSlidingTime;
    }

    /// <summary>
    /// スライディングの停止
    /// </summary>
    /// <param name="playerState"></param>
    public void StopSliding(PlayerState playerState = null)
    {
        if (_isSliding == false) return;
        _isSliding = false;
        _capsuleCollider.height = _startYScale;
    }

    /// <summary>
    /// スライディングタイマーの更新
    /// </summary>
    private void UpdateSlidingTimer()
    {
        if (!_isSlope || _rb.linearVelocity.y > -0.1f)
        {
            _slideTimer -= Time.deltaTime;
        }
        if (_slideTimer <= 0f)
        {
            StopSliding();
        }
    }

    /// <summary>
    /// スライディングを開始できるかどうか
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="isGround"></param>
    /// <param name="isSlope"></param>
    /// <param name="currentInput"></param>
    /// <returns></returns>
    public bool CanStartSliding(PlayerState playerState, bool isGround, bool isSlope, Vector2 currentInput)
    {
        return (playerState.CurrentState == PlayerState.State.Walking ||
               playerState.CurrentState == PlayerState.State.Sprinting) &&
               (isGround || isSlope) &&
               currentInput.magnitude > 0.1f &&
               !_isSliding;
    }

    /// <summary>
    /// 坂道かどうかの設定
    /// </summary>
    /// <param name="isSlope"></param>
    public void SetIsSlope(bool isSlope)
    {
        _isSlope = isSlope;
    }

    public bool IsSliding()
    {
        return _isSliding;
    }
}