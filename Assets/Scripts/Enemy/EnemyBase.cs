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
    protected float _roamingRangeDistance;
    protected float _speed;
    protected float _currentSpeed;
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
        _enemyFieldOfView = data.EnemyFieldOfView;
        _idleTime = data.IdleTime;
        _health = data.Health;
        _score = data.Score;
    }

    private void Awake()
    {
        TimeEventManager.TimeStart += TimeStartAction;
        TimeEventManager.TimeStop += TimeStopAction;
    }

    private void OnDestroy()
    {
        TimeEventManager.TimeStart -= TimeStartAction;
        TimeEventManager.TimeStop -= TimeStopAction;
    }

    /// <summary> ダメージの処理</summary>
    public void TakeDamage()
    {
        if (_health > 0)
        {
            _health--;
            EnemyDamageAction?.Invoke();
            if (_health <= 0)
            {
                Die();
            }
        }
    }

    /// <summary>死ぬ処理 </summary>
    protected virtual void Die()
    {
        ScoreManager.Instance.AddScore(this._score);
        EnemyState = enemyState.Die;
        EnemyDeathAction?.Invoke(this, EnemyTypeName, EnemyType);
        EnemyDeathNoArgAction?.Invoke();
        Destroy(this.gameObject);
    }

    public virtual void TimeStopAction()
    {
        Debug.Log("TimeStopBase");
    }

    public virtual void TimeStartAction()
    {
        Debug.Log("TimeStartBase");
    }

    public enum enemyState
    {
        Idle,
        Run,
        Die
    }
}
