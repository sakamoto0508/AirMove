using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private float _fireRate;
    private float _fireRange;
    private float _reloadTime;
    private float _animationSpeed;
    private int _magazineSize;
    private int _magazineSizeUp;
    private int _magazineSizeSum;
    private int _bullets;
    private bool _fireTimerIsActive = false;
    private bool _isReloading = false;
    private Transform _firePosition;
    private PlayerAnimation _fireAnimation;
    private PlayerAiming _playerAiming;
    private RaycastHit _hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fireAnimation = GetComponent<PlayerAnimation>();
        _playerAiming = GetComponent<PlayerAiming>();
        _magazineSizeUp = 0;
    }

    private void OnDrawGizmos()
    {
        if (_firePosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_firePosition.position, _firePosition.forward * _fireRange);
    }

    /// <summary>
    /// �ˌ�����
    /// </summary>
    /// <param name="playerData"></param>
    public void Fire(PlayerData playerData)
    {
        if (_isReloading)
        {
            return;
        }
        if (_bullets > 0)
        {
            if (_fireTimerIsActive)
            {
                return;
            }
            _bullets--;
            _fireAnimation.TriggerShot();
            if (Physics.Raycast(playerData.FirePosition.position, playerData.FirePosition.forward, out _hit, _fireRange))
            {
                BulletHit();
            }
            // �����A�j���[�V�����̒������ҋ@
            float animLength = _fireAnimation.GetAnimationLength("Shot");
            StartCoroutine(FireTimer(animLength));
        }
        else
        {
            StartReload();
        }
    }

    /// <summary>
    /// �e�����������Ƃ��̏���
    /// </summary>
    private void BulletHit()
    {
        if ((_hit.collider.TryGetComponent(out EnemyBase enemy)))
        {
            enemy.TakeDamage();
        }
    }

    private void StartReload()
    {
        if (_isReloading || _bullets == _magazineSizeSum)
        {
            return;
        }
        if (_playerAiming != null && _playerAiming.IsAiming())
        {
            _playerAiming.StopAim();
        }
        _fireAnimation.TriggerReload();
        float animLength = _fireAnimation.GetAnimationLength("Reload");
        StartCoroutine(ReloadCoroutine(animLength));
    }

    private void UpdateMagazineSizeSum()
    {
        _magazineSizeSum = _magazineSize + _magazineSizeUp;
    }

    /// <summary>
    /// �ˌ���̃N�[���_�E������
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator FireTimer(float waitTime)
    {
        _fireTimerIsActive = true;
        yield return new WaitForSeconds(waitTime);
        _fireTimerIsActive = false;
    }

    /// <summary>
    /// �����[�h�����̃R���[�`��
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReloadCoroutine(float waitTime)
    {
        _isReloading = true;
        yield return new WaitForSeconds(waitTime); // �����[�h���Ԃ��g�p
        _bullets = _magazineSizeSum;
        _isReloading = false;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _fireRate = playerData.FireRate;
        _reloadTime = playerData.ReloadTime;
        _magazineSize = playerData.MagazineSize;
        _bullets = playerData.MagazineSize;
        _firePosition = playerData.FirePosition;
        _fireRange = playerData.FireRange;
        UpdateMagazineSizeSum();
    }

    public bool IsReloading()
    {
        return _isReloading;
    }

    public void SetMagazineSizeUp(int magazinsize)
    {
        _magazineSizeUp += magazinsize;
        UpdateMagazineSizeSum();
    }
}
