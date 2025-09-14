using System.Collections.Generic;
using UnityEngine;

public class ItemFactoryManager : MonoBehaviour
{
    [SerializeField] private ItemDataBase _itemDataBase;
    private Dictionary<string , ItemData> _itemDict=new Dictionary<string , ItemData>();

    private void Awake()
    {
        InitializeItemDictionary();
    }

    /// <summary>
    /// �����ĂނŁ[�����@�����ɓo�^
    /// </summary>
    private void InitializeItemDictionary()
    {
        foreach(var data in _itemDataBase.ItemList)
        {
            if (!_itemDict.ContainsKey(data.ItemName))
            {
                _itemDict[data.ItemName]= data;
            }
        }
    }

    /// <summary>
    /// �w�肵����ނ̃A�C�e���𐶐�����
    /// </summary>
    /// <param name="type">�A�C�e���̎�ޖ�</param>
    /// <param name="position">�����ʒu</param>
    /// <returns>�������ꂽ ItemBase �R���|�[�l���g</returns>
    public ItemBase CreateItem(string type, Vector3 position)
    {
        // �o�^����Ă���A�C�e���f�[�^������
        if (_itemDict.TryGetValue(type, out var data))
        {
            // �A�C�e���̃v���n�u�𐶐�
            GameObject itemGO = Instantiate(data.ItemPrefab, position, data.ItemPrefab.transform.rotation);
            // ItemBase ���擾���ăZ�b�g�A�b�v
            ItemBase item = itemGO.GetComponent<ItemBase>();
            if (item != null)
            {
                item.SetUp(data);
            }
            else
            {
                Debug.LogError($"ItemBase �R���|�[�l���g��������܂���: {type}");
            }
            return item;
        }
        Debug.LogError($"�A�C�e���^�C�v '{type}' ��������܂���");
        return null;
    }

    /// <summary>
    /// ���p�\�ȃA�C�e���^�C�v�̈ꗗ���擾����
    /// </summary>
    public List<string> GetAvailableTypes()
    {
        return new List<string>(_itemDict.Keys);
    }

    /// <summary>
    /// �w��^�C�v�̃A�C�e���f�[�^���擾����
    /// </summary>
    public ItemData GetItemData(string type)
    {
        _itemDict.TryGetValue(type, out var data);
        return data;
    }
}
