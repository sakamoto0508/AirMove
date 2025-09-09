using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string EnemyName;
    public GameObject Prefab;
    public float MoveSpeed;
    /// <summary>
    /// ���m�͈�
    /// </summary>
    public float DetectionRange;
    /// <summary>
    /// ����p
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
