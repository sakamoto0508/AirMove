using UnityEngine;
public class PlayerSliding : MonoBehaviour
{
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private float _startYScale;
    private float _slideTimer = 0f;
    private PlayerState.State _previousState;
    private PlayerState _playerState;
    public bool _isSliding { get; private set; } = false;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _playerState = GetComponent<PlayerState>();
        _startYScale = _capsuleCollider.height;
    }

    private void Update()
    {
        if (_isSliding)
        {
            UpdateSlidingTimer();
        }
    }

    public void StartSliding(PlayerState playerState, PlayerData playerData)
    {
        if (_isSliding) return;
        _previousState = playerState.CurrentState;
        playerState.CurrentState = PlayerState.State.sliding;
        _isSliding = true;
        _capsuleCollider.height = playerData.SlidingYScale;
        _slideTimer = playerData.MaxSlidingTime;
    }

    public void StopSliding(PlayerState playerState = null)
    {
        _isSliding = false;
        _capsuleCollider.height = _startYScale;
        PlayerState targetState = playerState ?? _playerState;
        if (targetState != null)
        {
            if (_previousState == PlayerState.State.sprinting)
            {
                targetState.CurrentState = PlayerState.State.sprinting;
            }
            else
            {
                targetState.CurrentState = PlayerState.State.walking;
            }
        }
    }

    private void UpdateSlidingTimer()
    {
        _slideTimer -= Time.deltaTime;
        if (_slideTimer <= 0f)
        {
            StopSliding();
        }
    }

    public bool CanStartSliding(PlayerState playerState, bool isGround, bool isSlope, Vector2 currentInput)
    {
        return (playerState.CurrentState == PlayerState.State.walking ||
               playerState.CurrentState == PlayerState.State.sprinting) &&
               (isGround ||
               isSlope) &&
               currentInput.magnitude > 0.1f &&
               !_isSliding;
    }
}