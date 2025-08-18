using UnityEngine;

public class PlayerData : MonoBehaviour
{
    [Header("Reference")]
    [SerializeField] private Transform _mainCamera;

    [Header("movement")]
    [SerializeField] private float _walkSpeed = 10f;
    [SerializeField] private float _sprintSpeed = 15f;
    [SerializeField] private float _crouchSpeed = 5f;

    public float WalkSpeed => _walkSpeed;
    public float SprintSpeed => _sprintSpeed;
    public float CrouchSpeed => _crouchSpeed;
    public Transform MainCamera => _mainCamera;
}
