using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Action EnemyDamageAction;
    public Action<EnemyBase, string, EnemyData.enemyType> EnemyDeathAction;
    // 敵のタイプ情報を保持
    public string EnemyTypeName { get; private set; }
    public EnemyData.enemyType EnemyType { get; private set; }
    protected enemyState EnemyState { get; private set; } = enemyState.Idle;
    protected float _speed;
    protected float _enemyFieldOfView;
    protected int _health;
    protected int _score;

    /// <summary>
    /// エネミーデータのデータをセット
    /// </summary>
    /// <param name="data"></param>
    public virtual void Setup(EnemyData data)
    {
        _speed = data.MoveSpeed;
        _enemyFieldOfView = data.EnemyFieldOfView;
        _health = data.Health;
        _score = data.Score;
        EnemyTypeName = data.EnemyName;
        EnemyType = data.EnemyType;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary> ダメージの処理</summary>
    public void TakeDamage()
    {
        if (_health > 0)
        {
            _health--;
            Debug.Log(_health);
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
        this.gameObject.SetActive(false);
        EnemyState = enemyState.Die;
        EnemyDeathAction?.Invoke(this, EnemyTypeName, EnemyType);
    }

    public enum enemyState
    {
        Idle,
        Run,
        Die
    }
}
