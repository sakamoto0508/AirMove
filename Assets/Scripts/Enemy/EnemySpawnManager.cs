using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
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
    /// �e�G�^�C�v���ő吔�܂ŏo��������
    /// </summary>
    private void SpawnAllEemiesToMax()
    {
        //�o�^����Ă���S�G�^�C�v���擾
        List<string> types = _factoryManager.GetAvailableTypes();
        foreach (string type in types)
        {
            //�e�^�C�v�̃f�[�^���擾
            EnemyData data = _factoryManager.GetEnemyData(type);
            if (data != null)
            {
                //���łɏo�����Ă��鐔���m�F
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

    /// <summary>
    /// �w�肵���G�𐶐�
    /// </summary>
    /// <param name="type"></param>
    /// <param name="enemyType"></param>
    private void SpawnEnemy(string type, EnemyData.enemyType enemyType)
    {
        //�G�^�C�v�ɉ����������_�����W���擾
        Vector3 spawnPos = GetRandomSpawnPosition(enemyType);
        //�G�𐶐�
        EnemyBase enemy = _factoryManager.CreateEnemy(type, spawnPos);
        if (enemy != null)
        {
            //�G�����S�����Ƃ��̃C�x���g�ɓo�^
            enemy.EnemyDeathAction += HandleEnemyDestroyed;
            //�Ǘ����X�g�ɒǉ�
            _spawnedEnemies.Add(enemy);
        }
    }

    /// <summary>
    /// �G���|���ꂽ�Ƃ��ɌĂ΂��
    /// </summary>
    private void HandleEnemyDestroyed(EnemyBase enemy, string type, EnemyData.enemyType enemyType)
    {
        //�Ǘ����X�g����폜
        _spawnedEnemies.Remove(enemy);
        //�f�[�^���擾���ĕs�������m�F
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
