using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Action EnemyDamageAction;
    public Action<EnemyBase, string, EnemyData.enemyType> EnemyDeathAction;
    public Action EnemyDeathNoArgAction;
    // 敵のタイプ情報を保持
    public string EnemyTypeName { get; private set; }
    public EnemyData.enemyType EnemyType { get; private set; }
    protected enemyState EnemyState = enemyState.Idle;
    protected Transform _roamingRangeMax;
    protected Transform _roamingRangeMin;
    protected GameObject _explosion;
    protected float _roamingRangeDistance;
    protected float _speed;
    protected float _currentSpeed;
    protected float _detectionRange;
    protected float _enemyFieldOfView;
    protected float _idleTime;
    protected int _health;
    protected int _score;

    /// <summary>
    /// エネミーデータのデータをセット
    /// </summary>
    /// <param name="data"></param>
    public virtual void Setup(EnemyData data)
    {
        EnemyTypeName = data.EnemyName;
        EnemyType = data.EnemyType;
        _roamingRangeMax = data.RoamingRangeMax;
        _roamingRangeMin = data.RoamingRangeMin;
        _roamingRangeDistance = data.RoamingRangeDistance;
        _speed = data.MoveSpeed;
        _currentSpeed = data.MoveSpeed;
        _detectionRange = data.DetectionRange;
        _enemyFieldOfView = data.EnemyFieldOfView;
        _idleTime = data.IdleTime;
        _health = data.Health;
        _score = data.Score;
        _explosion = data.Explosion;
    }

    protected virtual void Awake()
    {
        TimeEventManager.TimeStart += TimeStartAction;
        TimeEventManager.TimeStop += TimeStopAction;
    }

    protected virtual void OnDestroy()
    {
        TimeEventManager.TimeStart -= TimeStartAction;
        TimeEventManager.TimeStop -= TimeStopAction;
    }

    /// <summary> ダメージの処理</summary>
    public virtual void TakeDamage()
    {
        if (_health > 0)
        {
            _health--;
            Debug.Log(gameObject.name+"damage");
            EnemyDamageAction?.Invoke();
            if (_health <= 0)
            {
                Debug.Log(gameObject.name+"die");
                Die();
            }
        }
    }

    /// <summary>死ぬ処理 </summary>
    public virtual void Die()
    {
        if (_explosion != null)
        {
            // 位置と回転を敵の位置に合わせて生成
            GameObject explosion = Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(explosion, 2f); // 2秒後に爆発エフェクトを消す
        }
        ScoreManager.Instance.AddScore(this._score);
        EnemyState = enemyState.Die;
        EnemyDeathAction?.Invoke(this, EnemyTypeName, EnemyType);
        EnemyDeathNoArgAction?.Invoke();
        Destroy(this.gameObject);
    }

    public virtual void TimeStopAction()
    {
        //Debug.Log("TimeStopBase");
    }

    public virtual void TimeStartAction()
    {
        //Debug.Log("TimeStartBase");
    }

    public enum enemyState
    {
        Idle,
        Run,
        Die
    }
}
