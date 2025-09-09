using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Action DamageAction;
    protected float _speed;
    protected float _enemyFieldOfView;
    protected int _health;

    //エネミーデータのデータをセット
    public virtual void Setup(EnemyData data)
    {
        _speed = data.MoveSpeed;
        _enemyFieldOfView = data.EnemyFieldOfView;
        _health = data.Health;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ダメージを受ける処理
    public void TakeDamage()
    {
        if(_health > 0)
        {
            _health--;
            DamageAction?.Invoke();
        }
        else
        {
            Die();
        }
    }

    //死ぬ処理
    protected virtual void Die()
    {
        this.gameObject.SetActive(false);
    }

    public enum enemyState
    {
        Idle,
        Run,
        Die
    }
}
