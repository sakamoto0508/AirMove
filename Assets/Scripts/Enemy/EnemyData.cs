using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _enemyName;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _roamingRangeMax;
    [SerializeField] private Transform _roamingRangeMin;
    [SerializeField] private float _moveSpeed;
    /// <summary>
    /// ŒŸ’m”ÍˆÍ
    /// </summary>
    [SerializeField] private float _detectionRange;
    /// <summary>
    /// Ž‹–ìŠp
    /// </summary>
    [SerializeField] private float _enemyFieldOfView;
    [SerializeField] private int _health;
    [SerializeField] private int _maxEnemyValue;
    [SerializeField] private int _score;
    [SerializeField] private enemyType _enemyType;
    public enum enemyType
    {
        Ground,
        Ai
    }

    public string EnemyName => _enemyName;
    public GameObject Prefab => _prefab;
    public Transform RoamingRangeMax => _roamingRangeMax;
    public Transform RoamingRangeMin => _roamingRangeMin;
    public float MoveSpeed => _moveSpeed;
    public float DetectionRange => _detectionRange;
    public float EnemyFieldOfView => _enemyFieldOfView;
    public int Health => _health;
    public int MaxEnemyValue => _maxEnemyValue;
    public int Score => _score;
    public enemyType EnemyType => _enemyType;

}
