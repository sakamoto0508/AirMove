using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected float _speed;
    protected int _health;

    //�G�l�~�[�f�[�^�̃f�[�^���Z�b�g
    public virtual void Setup(EnemyData data)
    {
        _speed = data.MoveSpeed;
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

    //�_���[�W���󂯂鏈��
    public void TakeDamage()
    {
        if(_health > 0)
        {
            _health--;
        }
        else
        {
            Die();
        }
    }

    //���ʏ���
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
