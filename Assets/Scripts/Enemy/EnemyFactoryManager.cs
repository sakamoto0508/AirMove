using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryManager : MonoBehaviour
{
    [SerializeField] private EnemyDatabase _database;
    // �G�̎�ޖ����L�[�ɂ��ăf�[�^���Ǘ����鎫��
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
    /// �f�[�^�x�[�X���̑S�Ă̓G�������ɓo�^
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
    /// �w�肵����ނ̓G�𐶐�����
    /// </summary>
    /// <param name="type">�G�̎�ޖ��iTypeName�j</param>
    /// <param name="position">�����ʒu</param>
    /// <returns>�������ꂽ EnemyBase �R���|�[�l���g</returns>
    public EnemyBase CreateEnemy(string type, Vector3 position)
    {
        // �o�^����Ă���G�f�[�^������
        if (_enemyDict.TryGetValue(type, out var data))
        {
            // �G�̃v���n�u�𐶐�
            GameObject enemyGO = Instantiate(data.Prefab, position, data.Prefab.transform.rotation);
            // EnemyBase ���擾���ăZ�b�g�A�b�v
            EnemyBase enemy = enemyGO.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.Setup(data);
            }
            return enemy;
        }
        Debug.LogError($"�G�^�C�v '{type}' ��������܂���");
        return null;
    }

    /// <summary>
    /// ���p�\�ȓG�^�C�v�̈ꗗ���擾����
    /// </summary>
    public List<string> GetAvailableTypes()
    {
        return new List<string>(_enemyDict.Keys);
    }

    /// <summary>
    /// �w��^�C�v�̓G�f�[�^���擾����
    /// </summary>
    public EnemyData GetEnemyData(string type)
    {
        _enemyDict.TryGetValue(type, out var data);
        return data;
    }
}
