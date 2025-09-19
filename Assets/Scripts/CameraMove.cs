using System.Collections;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _acceptableRange;
    [SerializeField] private float _idleTime;
    [SerializeField] private float _cameraFov;
    [SerializeField] private float _detectionRange;
    [SerializeField] private Transform _roamingRangeMin;
    [SerializeField] private Transform _roamingRangeMax;
    private bool _isIdle=false;
    private Vector3 _targetPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isIdle == true) return;
         
        if (CheckForObstacles())
        {
            SetNewDestination();
            return;
        }
        MoveToTarget();
        if (Vector3.Distance(transform.position, _targetPosition) < _acceptableRange)
        {
            StartCoroutine(IdleWait());
        }
    }

    private bool CheckForObstacles()
    {
        Vector3 moveDirection = (_targetPosition - transform.position).normalized;
        Vector3 origin = transform.position;
        RaycastHit hit;
        if (Physics.SphereCast(origin, _cameraFov, moveDirection,
            out hit, _detectionRange))
        {
            return true;
        }
        return false;
    }

    /// <summary>
    /// ランダムな徘徊先を選ぶ
    /// </summary>
    protected void SetNewDestination()
    {
        _targetPosition = new Vector3(
            Random.Range(_roamingRangeMin.position.x, _roamingRangeMax.position.x),
            Random.Range(_roamingRangeMin.position.y, _roamingRangeMax.position.y),
            Random.Range(_roamingRangeMin.position.z, _roamingRangeMax.position.z)
        );
        _isIdle = false;
    }

    /// <summary>
    /// 目的地に向かって移動
    /// </summary>
    private void MoveToTarget()
    {
        Vector3 dir = (_targetPosition - transform.position).normalized;
        transform.position += dir * _moveSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            Quaternion.LookRotation(dir),
            Time.deltaTime * 2f);
    }

    /// <summary>
    /// Idle待機
    /// </summary>
    private IEnumerator IdleWait()
    {
        _isIdle = true;
        yield return new WaitForSeconds(_idleTime);
        SetNewDestination();
        MoveToTarget();
    }
}
