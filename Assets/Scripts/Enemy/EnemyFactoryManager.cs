using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _database;
    private Dictionary<string,EnemyData> _enemyDict=new Dictionary<string, EnemyData>();
    private void Awake()
    {
        InitializeEnemyDictionary();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //エネミーのデータを追加
    private void InitializeEnemyDictionary()
    {
        foreach (var data in _database.EnemyList)
        {
            if (!_enemyDict.ContainsKey(data.TypeName))
            {
                _enemyDict[data.TypeName] = data;
            }
        }
    }


}
