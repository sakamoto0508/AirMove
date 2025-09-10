using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private float _fireRate;
    private float _fireRange;
    private float _reloadTime;
    private int _magazineSize;
    private int _bullets;
    private bool _fireTimerIsActive = false;
    private Transform _firePosition;
    private WaitForSeconds _fireRateWait;
    private PlayerAnimation _fireAnimation;
    private RaycastHit _hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fireRateWait= new WaitForSeconds(_fireRate);
        _fireAnimation=GetComponent<PlayerAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
       //Debug.DrawRay(_firePosition.position, _firePosition.forward * _fireRange, Color.red);
    }

    private void OnDrawGizmos()
    {
        if (_firePosition == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawRay(_firePosition.position, _firePosition.forward * _fireRange);
    }

    /// <summary>
    /// 射撃処理
    /// </summary>
    /// <param name="playerData"></param>
    public void Fire(PlayerData playerData)
    {
        if(_bullets > 0)
        {
            _bullets--;
            if (_fireTimerIsActive)
            {
                return;
            }
            _fireAnimation.TriggerShot();
            if (Physics.Raycast(playerData.FirePosition.position, playerData.FirePosition.forward, out _hit, _fireRange))
            {
                BulletHit();
            }
            // 撃ちアニメーションの長さ分待機
            float animLength = _fireAnimation.GetAnimationLength("Shot");
            StartCoroutine(FireTimer(animLength));
        }
        else
        {
            _fireAnimation.TriggerReload();
            _bullets = _magazineSize;
        }
    }

    /// <summary>
    /// 弾が命中したときの処理
    /// </summary>
    private void BulletHit()
    {
        Debug.Log("Hit: " + _hit.collider.name);
        if((_hit.collider.TryGetComponent(out EnemyBase enemy)))
        {
            enemy.TakeDamage();
        }
    }

    /// <summary>
    /// 射撃後のクールダウン処理
    /// </summary>
    /// <param name="waitTime"></param>
    /// <returns></returns>
    private IEnumerator FireTimer(float waitTime)
    {
        _fireTimerIsActive = true;
        yield return new WaitForSeconds(waitTime);
        _fireTimerIsActive = false;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _fireRate = playerData.FireRate;
        _reloadTime = playerData.ReloadTime;
        _magazineSize = playerData.MagazineSize;
        _bullets = playerData.MagazineSize;
        _firePosition = playerData.FirePosition;
        _fireRange = playerData.FireRange;
    }
}
