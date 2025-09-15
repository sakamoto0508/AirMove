using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private string _enemyName;
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Transform _roamingRangeMax;
    [SerializeField] private Transform _roamingRangeMin;
    /// <summary>
    /// 指定の距離の誤差範囲
    /// </summary>
    [SerializeField] private float _roamingRangeDistance;
    [SerializeField] private float _moveSpeed;
    /// <summary>
    /// 検知範囲
    /// </summary>
    [SerializeField] private float _detectionRange;
    /// <summary>
    /// スフィアキャストの半径
    /// </summary>
    [SerializeField] private float _enemyFieldOfView;
    [SerializeField] private float _idleTime;
    [SerializeField] private int _health;
    [SerializeField] private int _maxEnemyValue;
    [SerializeField] private int _score;
    [SerializeField] private enemyType _enemyType;
    public enum enemyType
    {
        Ground,
        Air
    }

    public string EnemyName => _enemyName;
    public GameObject Prefab => _prefab;
    public Transform RoamingRangeMax => _roamingRangeMax;
    public Transform RoamingRangeMin => _roamingRangeMin;
    public float RoamingRangeDistance => _roamingRangeDistance;
    public float MoveSpeed => _moveSpeed;
    public float DetectionRange => _detectionRange;
    public float EnemyFieldOfView => _enemyFieldOfView;
    public float IdleTime => _idleTime;
    public int Health => _health;
    public int MaxEnemyValue => _maxEnemyValue;
    public int Score => _score;
    public enemyType EnemyType => _enemyType;

}
