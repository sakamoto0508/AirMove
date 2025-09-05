using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private PlayerState _playerState;
    private bool _isAiming;
    private const string IS_AIMING = "IsAiming";
    private const string SHOOT_TRIGGER = "Shoot";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        
    }

    public void AnimationAiming()
    {
        _animator.SetBool(SHOOT_TRIGGER, true);
    }

    public void TriggerShot()
    {
        _animator.SetTrigger(SHOOT_TRIGGER);
    }

    /// <summary>
    /// エイミング状態を取得
    /// </summary>
    public void SetIsAiming(bool isAiming)
    {
        _isAiming = isAiming;
    }
}
