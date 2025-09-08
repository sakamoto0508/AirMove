using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _database;
    private Dictionary<string,EnemyData> _enemyDict=new Dictionary<string, EnemyData>();
    private void Awake()
    {
        foreach (var data in _database.EnemyList)
        {
            if (!_enemyDict.ContainsKey(data.TypeName))
            {
                _enemyDict[data.TypeName] = data;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
