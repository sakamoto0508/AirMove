using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public GameObject Prefab;
    public float MoveSpeed;
    /// <summary>
    /// ŒŸ’m”ÍˆÍ
    /// </summary>
    public float DetectionRange;
    /// <summary>
    /// Ž‹–ìŠp
    /// </summary>
    public float EnemyFieldOfView;
    public int Health;
    public int MaxEnemyValue;
    public int Score;
    public enemyType EnemyType;
    public enum enemyType
    {
        Ground,
        Ai
    }
}
