using UnityEngine;

public class EnemyHitBox : MonoBehaviour
{
    private EnemyBase _enemyBase;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _enemyBase = GetComponentInParent<EnemyBase>();
    }

    public void OnRaycastHit()
    {
        if (_enemyBase != null)
        {
            _enemyBase.TakeDamage();
        }
    }
}
