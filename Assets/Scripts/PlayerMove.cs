using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    private Rigidbody _rb;
    private Vector2 _currentInput;
    private float _currentSpeed;
    private Transform _playerCamera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (_playerCamera == null) return;
        Vector3 inputDir = _playerCamera.forward * _currentInput.y + _playerCamera.right * _currentInput.x;
        float yVel = _rb.linearVelocity.y;
        Vector3 moveXz = inputDir.normalized * _currentSpeed;
        _rb.linearVelocity = new Vector3(moveXz.x, yVel, moveXz.z);
    }

    public void Move(Vector2 input, PlayerData playerData)
    {
        _currentInput = input;
        _currentSpeed = playerData.WalkSpeed;
        _playerCamera = playerData.MainCamera;
    }

    public void Stop()
    {
        _currentInput = Vector2.zero;
    }

}
