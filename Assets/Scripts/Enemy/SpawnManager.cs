using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private EnemyFactoryManager _factoryManager;
    [Header("Spawn Position Settings")]
    [SerializeField] private Transform _minSpawnPositionGround;
    [SerializeField] private Transform _maxSpawnPositionGround;
    [SerializeField] private Transform _minSpawnPositionAir;
    [SerializeField] private Transform _maxSpawnPositionAir;
    // 現在出現している敵を管理
    private List<EnemyBase> _spawnedEnemies = new List<EnemyBase>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnAllEemiesToMax();
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 書く敵タイプを最大数まで出現させる
    /// </summary>
    private void SpawnAllEemiesToMax()
    {
        List<string> types = _factoryManager.GetAvailableTypes();
        foreach (string type in types)
        {
            EnemyData data = _factoryManager.GetEnemyData(type);
            if (data != null)
            {
                int currentCount = CountEnemiesByType(type);
                int maxCount = data.MaxEnemyValue;

                // 不足分を補充
                for (int i = currentCount; i < maxCount; i++)
                {
                    SpawnEnemy(type, data.EnemyType);
                }
            }
        }
    }

    private void SpawnEnemy(string type, EnemyData.enemyType enemyType)
    {
        Vector3 spawnPos = GetRandomSpawnPosition(enemyType);
        EnemyBase enemy = _factoryManager.CreateEnemy(type, spawnPos);
        if (enemy != null)
        {
            enemy.EnemyDeathAction += HandleEnemyDestroyed;
            _spawnedEnemies.Add(enemy);
        }
    }

    /// <summary>
    /// 敵が倒されたときに呼ばれる
    /// </summary>
    private void HandleEnemyDestroyed(EnemyBase enemy, string type, EnemyData.enemyType enemyType)
    {
        _spawnedEnemies.Remove(enemy);
        EnemyData data = _factoryManager.GetEnemyData(type);
        if (data != null)
        {
            int currentCount = CountEnemiesByType(type);
            if (currentCount < data.MaxEnemyValue)
            {
                SpawnEnemy(type, enemyType);
            }
        }
    }

    /// <summary>
    /// 現在出現している指定タイプの敵数を数える
    /// </summary>
    private int CountEnemiesByType(string type)
    {
        int count = 0;
        foreach (var e in _spawnedEnemies)
        {
            if (e != null && e.EnemyTypeName == type)
                count++;
        }
        return count;
    }

    /// <summary>
    /// 地上 or 空中からランダム位置を返す
    /// </summary>
    private Vector3 GetRandomSpawnPosition(EnemyData.enemyType enemyType)
    {
        if (enemyType == EnemyData.enemyType.Ground)
        {
            return new Vector3(
                Random.Range(_minSpawnPositionGround.position.x, _maxSpawnPositionGround.position.x),
                _minSpawnPositionGround.position.y,
                Random.Range(_minSpawnPositionGround.position.z, _maxSpawnPositionGround.position.z)
            );
        }
        else // Air
        {
            return new Vector3(
                Random.Range(_minSpawnPositionAir.position.x, _maxSpawnPositionAir.position.x),
                Random.Range(_minSpawnPositionAir.position.y, _maxSpawnPositionAir.position.y),
                Random.Range(_minSpawnPositionAir.position.z, _maxSpawnPositionAir.position.z)
            );
        }
    }
}
