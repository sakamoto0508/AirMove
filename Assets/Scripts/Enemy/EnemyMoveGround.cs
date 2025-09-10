using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMoveGround : EnemyBase
{
    private NavMeshAgent _agent;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GoToRandPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (EnemyState != enemyState.Idle && !_agent.pathPending &&
            _agent.remainingDistance < _roamingRangeDistance)
        {
            StartCoroutine(IdleWait());
        }
    }

    /// <summary>
    /// ���̒n�_�ւƌ�����
    /// </summary>
    private void GoToRandPoint()
    {
        Vector3 newPos = GetRandomPointInRange();
        _agent.speed = _speed;
        _agent.SetDestination(newPos);
        EnemyState = enemyState.Run;
    }

    /// <summary>
    /// �p�j�͈͓��̃����_���ȍ��W��Ԃ�
    /// </summary>
    private Vector3 GetRandomPointInRange()
    {
        Vector3 randomPos = new Vector3(
            Random.Range(_roamingRangeMin.position.x, _roamingRangeMax.position.x),
            transform.position.y,
            Random.Range(_roamingRangeMin.position.z, _roamingRangeMax.position.z)
            );
        //NavMesh��ɃT���v�����O
        NavMeshHit hit;
        //�w�肵���ʒu�isourcePosition�j�̋߂��ɂ���ANavMesh ��̍ł��߂��_���݂���B
        if (NavMesh.SamplePosition(randomPos, out hit, 2f, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return transform.position; // ���s�����炻�̏�ɗ��܂�
    }

    /// <summary>
    /// Idle��Ԃŏ����ҋ@����
    /// </summary>
    private IEnumerator IdleWait()
    {
        EnemyState = enemyState.Idle;
        yield return new WaitForSeconds(_idleTime);
        GoToRandPoint();
    }
}
