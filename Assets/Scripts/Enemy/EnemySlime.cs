using System.Collections;
using UnityEngine;

public class EnemySlime : MonoBehaviour
{
    [SerializeField] private float _jumpForce = 1f;
    [SerializeField] private float _jumpCoolTime = 3f;
    private Rigidbody _rb;
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        StartCoroutine(JumpLoop());
    }

    private void Jump()
    {
        _rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private IEnumerator JumpLoop()
    {
        while (true)
        {
            Jump();
            yield return new WaitForSeconds(_jumpCoolTime);
        }
    }
}
