using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemyData", menuName = "Enemy/Enemy Data")]
public class EnemyData : ScriptableObject
{
    public string TypeName;
    public GameObject Prefab;
    public float MoveSpeed;
    public int Health;
}
