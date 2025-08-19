using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _mainCamera;

    [Header("movement")]
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private float _sprintSpeed = 15f;
    [SerializeField] private float _crouchSpeed = 5f;
    [SerializeField] private float _groundDrag= 0.5f;

    [Header("GroundCheck")]
    [SerializeField] private float _playerHeight;
    [SerializeField] private LayerMask _groundLayer;

    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float CrouchSpeed => _crouchSpeed;
    public float GroundDrag => _groundDrag;
    public Transform MainCamera => _mainCamera;
    public float PlayerHeight => _playerHeight;
    public LayerMask GroundLayer => _groundLayer;
}
