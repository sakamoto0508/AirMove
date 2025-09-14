using System.Collections;
using UnityEngine;
using static EnemyBase;

public class EnemyMoveAir : EnemyBase
{
    private Vector3 _targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetNewDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyState == enemyState.Idle)
            return;
        else
        {
            MoveToTarget();
        }
        if (Vector3.Distance(transform.position, _targetPosition) < _roamingRangeDistance)
        {
            StartCoroutine(IdleWait());
        }
    }

    /// <summary>
    /// 目的地に向かって移動
    /// </summary>
    private void MoveToTarget()
    {
        Vector3 dir = (_targetPosition - transform.position).normalized;
        transform.position += dir * _speed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 2f);
    }

    /// <summary>
    /// ランダムな徘徊先を選ぶ
    /// </summary>
    private void SetNewDestination()
    {
        _targetPosition = new Vector3(
            Random.Range(_roamingRangeMin.position.x, _roamingRangeMax.position.x),
            Random.Range(_roamingRangeMin.position.y, _roamingRangeMax.position.y),
            Random.Range(_roamingRangeMin.position.z, _roamingRangeMax.position.z)
        );
        EnemyState = enemyState.Run;
    }

    /// <summary>
    /// Idle待機
    /// </summary>
    private IEnumerator IdleWait()
    {
        EnemyState = enemyState.Idle;
        yield return new WaitForSeconds(_idleTime);
        SetNewDestination();
        MoveToTarget();
    }

    public override void TimeStopAction()
    {
        base.TimeStopAction();
        _speed = 0f;
    }

    public override void TimeStartAction()
    {
        base.TimeStartAction();
        _speed = _currentSpeed;
    }
}
