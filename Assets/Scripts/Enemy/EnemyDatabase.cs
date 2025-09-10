using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDatabase", menuName = "Enemy/Enemy Database")]
public class EnemyDataBase : ScriptableObject
{
    public List<EnemyData> EnemyList;
}
