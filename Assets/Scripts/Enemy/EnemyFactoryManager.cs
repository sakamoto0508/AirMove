using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _database;
    // 敵の種類名をキーにしてデータを管理する辞書
    private Dictionary<string, EnemyData> _enemyDict = new Dictionary<string, EnemyData>();

    private void Awake()
    {
        InitializeEnemyDictionary();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// データベース内の全ての敵を辞書に登録
    /// </summary>
    private void InitializeEnemyDictionary()
    {
        foreach (var data in _database.EnemyList)
        {
            if (!_enemyDict.ContainsKey(data.EnemyName))
            {
                _enemyDict[data.EnemyName] = data;
            }
        }
    }

    /// <summary>
    /// 指定した種類の敵を生成する
    /// </summary>
    /// <param name="type">敵の種類名（TypeName）</param>
    /// <param name="position">生成位置</param>
    /// <returns>生成された EnemyBase コンポーネント</returns>
    public EnemyBase CreateEnemy(string type, Vector3 position)
    {
        // 登録されている敵データを検索
        if (_enemyDict.TryGetValue(type, out var data))
        {
            // 敵のプレハブを生成
            GameObject enemyGO = Instantiate(data.Prefab, position, data.Prefab.transform.rotation);
            // EnemyBase を取得してセットアップ
            EnemyBase enemy = enemyGO.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Setup(data);
            }
            return enemy;
        }
        Debug.LogError($"敵タイプ '{type}' が見つかりません");
        return null;
    }

    /// <summary>
    /// 利用可能な敵タイプの一覧を取得する
    /// </summary>
    public List<string> GetAvailableTypes()
    {
        return new List<string>(_enemyDict.Keys);
    }

    /// <summary>
    /// 指定タイプの敵データを取得する
    /// </summary>
    public EnemyData GetEnemyData(string type)
    {
        _enemyDict.TryGetValue(type, out var data);
        return data;
    }
}
