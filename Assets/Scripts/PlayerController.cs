using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputBuffer), typeof(PlayerState), typeof(PlayerData))]
public class PlayerController : MonoBehaviour
{
    private InputBuffer _inputBuffer;
    private PlayerData _playerData;
    private PlayerState _playerState;
    private PlayerMove _playerMove;

    private void RegisterInputAction()
    {
        _inputBuffer.MoveAction.performed += OnInputMove;
        _inputBuffer.MoveAction.canceled += OnInputMove;
    }

    private void OnDestroy()
    {
        _inputBuffer.MoveAction.performed -= OnInputMove;
        _inputBuffer.MoveAction.canceled -= OnInputMove;
    }
    
    private void Awake()
    {
        _inputBuffer = GetComponent<InputBuffer>();
        _playerData = GetComponent<PlayerData>();
        _playerState = GetComponent<PlayerState>();
        _playerMove = GetComponent<PlayerMove>();
        
    }
    
    private void OnInputMove(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 input = context.ReadValue<Vector2>();
            _playerMove?.Move(input, _playerData);
        }
        else if (context.canceled)
        {
            _playerMove?.Stop();
        }
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RegisterInputAction();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
