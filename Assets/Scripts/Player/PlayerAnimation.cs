using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private PlayerState _playerState;
    private bool _isAiming;
    private const string IS_AIMING = "IsAiming";
    private const string SHOOT_TRIGGER = "Shoot";
    private const string RELOAD_TRIGGER = "Reload";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerState = GetComponent<PlayerState>();
    }

    private void Update()
    {
        AnimationAiming();
    }

    public float GetAnimationLength(string aniName)
    {
        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == aniName)
            {
                return clip.length;
            }
        }
        return 0f;
    }

    public void AnimationAiming()
    {
        _animator.SetBool(IS_AIMING, _isAiming);
    }

    public void TriggerShot()
    {
        _animator.SetTrigger(SHOOT_TRIGGER);
    }

    public void TriggerReload()
    {
        _animator.SetTrigger(RELOAD_TRIGGER);
    }

    /// <summary>
    /// エイミング状態を取得
    /// </summary>
    public void SetIsAiming(bool isAiming)
    {
        _isAiming = isAiming;
    }
}
