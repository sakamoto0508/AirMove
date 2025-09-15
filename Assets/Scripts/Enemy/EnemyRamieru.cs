using System.Collections;
using UnityEngine;

public class EnemyRamieru : EnemyMoveAir
{
    [SerializeField] private string _targetTag = "WeakPoint";
    private Animator _animator;
    private SphereCollider _sphereCollider;
    private int _currentHealth;
    private const string HIT_MOTION = ("HitMotion");

    protected override void Awake()
    {
        base.Awake();
        EnemyDamageAction += RamieruDamageAction;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
        _sphereCollider= GetComponent<SphereCollider>();
        _currentHealth = _health;
        SetupChildColliders();
    }

    public override void TakeDamage()
    {
        if (_health > 0 )
        {
            _health--;
            EnemyDamageAction?.Invoke();
            if (_health <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>
    /// 子オブジェクトからのダメージ処理
    /// </summary>
    public void TakeDamageFormChild()
    {
        if (_health > 0)
        {
            _health--;
            if (_health <= 0)
            {
                Die();
            }
        }
    }

    private void RamieruDamageAction()
    {
        _sphereCollider.enabled = false;
        _speed = 0f;
        _animator.SetTrigger(HIT_MOTION);
        float animationLength = AnimationLength(HIT_MOTION);
        StartCoroutine(HitMotion(animationLength));
    }

    private float AnimationLength(string animationName)
    {
        RuntimeAnimatorController ac = _animator.runtimeAnimatorController;
        foreach (var clip in ac.animationClips)
        {
            if (clip.name == animationName)
            {
                return clip.length;
            }
        }
        return 0f;
    }

    private IEnumerator HitMotion(float animationLength)
    {
        yield return new WaitForSeconds(animationLength);
        _health = _currentHealth;
        _sphereCollider.enabled = true;
        _speed = _currentSpeed;
    }

    // 子オブジェクトにレイキャスト受付コンポーネントを設定
    private void SetupChildColliders()
    {
        // 自分とすべての子オブジェクトのTransformを取得
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            // 自分自身は除外し、指定したタグを持つ子オブジェクトのみ処理
            if (child != this.transform && child.CompareTag(_targetTag))
            {
                // Colliderがあるかチェック
                Collider childCollider = child.GetComponent<Collider>();
                if (childCollider != null)
                {
                    // RamieruChildRaycastコンポーネントを追加または取得
                    EnemyRamieruChild childRaycast = child.GetComponent<EnemyRamieruChild>();
                    if (childRaycast == null)
                    {
                        childRaycast = child.gameObject.AddComponent<EnemyRamieruChild>();
                    }
                    // 親エネミーの参照を設定
                    childRaycast.Initialize(this);
                }
            }
        }
    }

    public override void TimeStopAction()
    {
        Debug.Log("TimeStop");
        base.TimeStopAction();
    }

    public override void TimeStartAction()
    {
        Debug.Log("TimeStart");
        base.TimeStartAction();
    }
}
