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
    /// あいてむでーたを　辞書に登録
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
    /// 指定した種類のアイテムを生成する
    /// </summary>
    /// <param name="type">アイテムの種類名</param>
    /// <param name="position">生成位置</param>
    /// <returns>生成された ItemBase コンポーネント</returns>
    public ItemBase CreateItem(string type, Vector3 position)
    {
        // 登録されているアイテムデータを検索
        if (_itemDict.TryGetValue(type, out var data))
        {
            // アイテムのプレハブを生成
            GameObject itemGO = Instantiate(data.ItemPrefab, position, data.ItemPrefab.transform.rotation);
            // ItemBase を取得してセットアップ
            ItemBase item = itemGO.GetComponent<ItemBase>();
            if (item != null)
            {
                item.SetUp(data);
            }
            else
            {
                Debug.LogError($"ItemBase コンポーネントが見つかりません: {type}");
            }
            return item;
        }
        Debug.LogError($"アイテムタイプ '{type}' が見つかりません");
        return null;
    }

    /// <summary>
    /// 利用可能なアイテムタイプの一覧を取得する
    /// </summary>
    public List<string> GetAvailableTypes()
    {
        return new List<string>(_itemDict.Keys);
    }

    /// <summary>
    /// 指定タイプのアイテムデータを取得する
    /// </summary>
    public ItemData GetItemData(string type)
    {
        _itemDict.TryGetValue(type, out var data);
        return data;
    }
}
