using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    private void Update()
    {
        transform.position = _playerTransform.position;
    }
}
