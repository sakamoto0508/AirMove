using System.Collections;
using UnityEngine;

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

        if (CheckForObstacles())
        {
            SetNewDestination();
            return;
        }
        MoveToTarget();
        if (Vector3.Distance(transform.position, _targetPosition) < _roamingRangeDistance)
        {
            StartCoroutine(IdleWait());
        }
    }

    /// <summary>
    /// –Ú“I’n‚ÉŒü‚©‚Á‚ÄˆÚ“®
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
    /// ƒ‰ƒ“ƒ_ƒ€‚Èœpœjæ‚ğ‘I‚Ô
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

    private bool CheckForObstacles()
    {
        Vector3 moveDirection=(_targetPosition-transform.position).normalized;
        Vector3 origin=transform.position;
        RaycastHit hit;
        if(Physics.SphereCast(origin,_enemyFieldOfView,moveDirection,
            out hit, _detectionRange))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// Idle‘Ò‹@
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

    public override void Setup(EnemyData data)
    {
        base.Setup(data);

    }

    private void OnDrawGizmos()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        Vector3 origin = transform.position;
        // áŠQ•¨ŒŸ’m”ÍˆÍ‚ğ•`‰æ
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin, _enemyFieldOfView);
        Gizmos.DrawWireSphere(origin + moveDirection * _detectionRange, _enemyFieldOfView);
        // ŒŸ’m•ûŒü‚Ìü‚ğ•`‰æ
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(origin, origin + moveDirection * _detectionRange);
        // –Ú“I’n‚ğ•`‰æ
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_targetPosition, 0.5f);
        Gizmos.DrawLine(transform.position, _targetPosition);
    }
}
