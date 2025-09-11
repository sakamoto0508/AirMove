using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    [SerializeField] private ItemFactoryManager _factoryManager;
    [SerializeField] private float _intervalTime = 10f;
    [SerializeField] private int _maxTotalItems = 5;
    [SerializeField] List<Transform> _spawnPoints=new List<Transform>();
    // ���ݏo�����Ă���A�C�e�����Ǘ�
    private List<ItemBase> _spawnedItems = new List<ItemBase>();
    // �X�|�[���|�C���g�ʂ̃R���[�`���Ǘ��i�C���^�[�o���p�j
    private Dictionary<Transform, Coroutine> _spawnPointCoroutines = new Dictionary<Transform, Coroutine>();
    // �e�X�|�[���|�C���g�ɃA�C�e�������邩�ǂ������Ǘ�
    private Dictionary<Transform, bool> _spawnPointOccupied = new Dictionary<Transform, bool>();

    void Start()
    {
        InitializeSpawnPoints();
        SpawnInitialItems();
    }

    /// <summary>
    /// OnDestroy���Ƀ��X�g�⎫�����N���A
    /// </summary>
    private void OnDestroy()
    {
        // �����Ă���R���[�`�������ׂĒ�~
        foreach (var kvp in _spawnPointCoroutines)
        {
            if (kvp.Value != null)
            {
                StopCoroutine(kvp.Value);
            }
        }
        _spawnPointCoroutines.Clear();
        _spawnPointOccupied.Clear();
        _spawnedItems.Clear();
    }

    /// <summary>
    /// �X�|�[���|�C���g�̏�����
    /// </summary>
    private void InitializeSpawnPoints()
    {
        foreach (Transform spawnPoint in _spawnPoints)
        {
            if (spawnPoint != null)
            {
                _spawnPointOccupied[spawnPoint] = false;
            }
        }
    }

    /// <summary>
    /// �����A�C�e���𐶐�
    /// </summary>
    private void SpawnInitialItems()
    {
        // ���p�\�ȃX�|�[���|�C���g���擾
        List<Transform> availableSpawnPoints = GetAvailableSpawnPoints();
        // �ő�A�C�e�����܂Ő����i���p�\�ȃX�|�[���|�C���g�����l���j
        int itemsToSpawn = Mathf.Min(_maxTotalItems, availableSpawnPoints.Count);
        for (int i = 0; i < itemsToSpawn; i++)
        {
            if (availableSpawnPoints.Count > 0)
            {
                // �����_���ȃX�|�[���|�C���g��I��
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];
                // �A�C�e���𐶐�
                SpawnRandomItemAtPoint(selectedSpawnPoint);
                // �g�p�ς݃��X�g����폜
                availableSpawnPoints.RemoveAt(randomIndex);
            }
        }
    }

    /// <summary>
    /// ���p�\�i�󂢂Ă���j�X�|�[���|�C���g�̃��X�g���擾
    /// </summary>
    private List<Transform> GetAvailableSpawnPoints()
    {
        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform spawnPoint in _spawnPoints)
        {
            if (spawnPoint != null && !_spawnPointOccupied[spawnPoint])
            {
                availablePoints.Add(spawnPoint);
            }
        }
        return availablePoints;
    }

    /// <summary>
    /// �w��X�|�[���|�C���g�Ƀ����_���ȃA�C�e���𐶐�
    /// </summary>
    /// <param name="spawnPoint">�X�|�[���|�C���g</param>
    private void SpawnRandomItemAtPoint(Transform spawnPoint)
    {
        // ���p�\�ȃA�C�e���^�C�v���擾
        List<string> types = _factoryManager.GetAvailableTypes();
        if (types.Count == 0) return;
        // �m���Ɋ�Â��ăA�C�e���^�C�v��I��
        string selectedType = SelectItemByProbability(types);
        // �A�C�e���𐶐�
        ItemBase item = _factoryManager.CreateItem(selectedType, spawnPoint.position);
        if (item != null)
        {
            // �A�C�e�������ꂽ�Ƃ��̃C�x���g�ɓo�^
            item.ItemHitAction += (collectedItem, type, itemType) =>
                HandleItemCollected(collectedItem, type, itemType, spawnPoint);
            // �Ǘ����X�g�ɒǉ�
            _spawnedItems.Add(item);
            _spawnPointOccupied[spawnPoint] = true;
        }
    }

    /// <summary>
    /// �m���Ɋ�Â��ăA�C�e���^�C�v��I��
    /// </summary>
    /// <param name="availableTypes">���p�\�ȃ^�C�v���X�g</param>
    /// <returns>�I�����ꂽ�A�C�e���^�C�v</returns>
    private string SelectItemByProbability(List<string> availableTypes)
    {
        // �S�A�C�e���̊m���̍��v���v�Z
        float totalProbability = 0f;
        foreach (string type in availableTypes)
        {
            ItemData data = _factoryManager.GetItemData(type);
            if (data != null)
            {
                totalProbability += data.SpawnProbability;
            }
        }
        // �����_���Ȓl�𐶐��i0���獇�v�m���܂Łj
        float randomValue = Random.Range(0f, totalProbability);
        float currentProbability = 0f;

        // �m���Ɋ�Â��đI��
        foreach (string type in availableTypes)
        {
            ItemData data = _factoryManager.GetItemData(type);
            if (data != null)
            {
                currentProbability += data.SpawnProbability;
                if (randomValue <= currentProbability)
                {
                    return type;
                }
            }
        }
        // �t�H�[���o�b�N�i�ʏ�͔������Ȃ��j
        return availableTypes[0];
    }

    /// <summary>
    /// �A�C�e�������ꂽ�Ƃ��ɌĂ΂��
    /// </summary>
    private void HandleItemCollected(ItemBase item, string type, ItemData.itemType itemType, Transform spawnPoint)
    {
        // �Ǘ����X�g����폜
        _spawnedItems.Remove(item);
        _spawnPointOccupied[spawnPoint] = false;
        // �C���^�[�o����ɐV�����A�C�e�����X�|�[��
        if (_spawnPointCoroutines.ContainsKey(spawnPoint))
        {
            StopCoroutine(_spawnPointCoroutines[spawnPoint]);
        }
        _spawnPointCoroutines[spawnPoint] = StartCoroutine(RespawnItemAfterInterval(spawnPoint, _intervalTime));
    }

    /// <summary>
    /// �C���^�[�o����ɃA�C�e�����ăX�|�[������R���[�`��
    /// </summary>
    /// <param name="originalSpawnPoint">���̃X�|�[���|�C���g</param>
    /// <param name="interval">�ҋ@����</param>
    private IEnumerator RespawnItemAfterInterval(Transform originalSpawnPoint, float interval)
    {
        yield return new WaitForSeconds(interval);
        // �S�̂̍ő吔�`�F�b�N
        if (_spawnedItems.Count < _maxTotalItems)
        {
            // ���p�\�ȃX�|�[���|�C���g���擾
            List<Transform> availableSpawnPoints = GetAvailableSpawnPoints();
            if (availableSpawnPoints.Count > 0)
            {
                // �����_���ȃX�|�[���|�C���g��I��
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];
                // �A�C�e���𐶐�
                SpawnRandomItemAtPoint(selectedSpawnPoint);
            }
        }
        // �R���[�`���Ǘ�����폜
        if (_spawnPointCoroutines.ContainsKey(originalSpawnPoint))
        {
            _spawnPointCoroutines.Remove(originalSpawnPoint);
        }
    }
}
