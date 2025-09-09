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
    // ���ݏo�����Ă���G���Ǘ�
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
    /// �����G�^�C�v���ő吔�܂ŏo��������
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

                // �s�������[
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
    /// �G���|���ꂽ�Ƃ��ɌĂ΂��
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
    /// ���ݏo�����Ă���w��^�C�v�̓G���𐔂���
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
    /// �n�� or �󒆂��烉���_���ʒu��Ԃ�
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
