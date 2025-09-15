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
    /// �q�I�u�W�F�N�g����̃_���[�W����
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

    // �q�I�u�W�F�N�g�Ƀ��C�L���X�g��t�R���|�[�l���g��ݒ�
    private void SetupChildColliders()
    {
        // �����Ƃ��ׂĂ̎q�I�u�W�F�N�g��Transform���擾
        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            // �������g�͏��O���A�w�肵���^�O�����q�I�u�W�F�N�g�̂ݏ���
            if (child != this.transform && child.CompareTag(_targetTag))
            {
                // Collider�����邩�`�F�b�N
                Collider childCollider = child.GetComponent<Collider>();
                if (childCollider != null)
                {
                    // RamieruChildRaycast�R���|�[�l���g��ǉ��܂��͎擾
                    EnemyRamieruChild childRaycast = child.GetComponent<EnemyRamieruChild>();
                    if (childRaycast == null)
                    {
                        childRaycast = child.gameObject.AddComponent<EnemyRamieruChild>();
                    }
                    // �e�G�l�~�[�̎Q�Ƃ�ݒ�
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
