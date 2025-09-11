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
    // 現在出現しているアイテムを管理
    private List<ItemBase> _spawnedItems = new List<ItemBase>();
    // スポーンポイント別のコルーチン管理（インターバル用）
    private Dictionary<Transform, Coroutine> _spawnPointCoroutines = new Dictionary<Transform, Coroutine>();
    // 各スポーンポイントにアイテムがあるかどうかを管理
    private Dictionary<Transform, bool> _spawnPointOccupied = new Dictionary<Transform, bool>();

    void Start()
    {
        InitializeSpawnPoints();
        SpawnInitialItems();
    }

    /// <summary>
    /// OnDestroy時にリストや辞書をクリア
    /// </summary>
    private void OnDestroy()
    {
        // 走っているコルーチンをすべて停止
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
    /// スポーンポイントの初期化
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
    /// 初期アイテムを生成
    /// </summary>
    private void SpawnInitialItems()
    {
        // 利用可能なスポーンポイントを取得
        List<Transform> availableSpawnPoints = GetAvailableSpawnPoints();
        // 最大アイテム数まで生成（利用可能なスポーンポイント数も考慮）
        int itemsToSpawn = Mathf.Min(_maxTotalItems, availableSpawnPoints.Count);
        for (int i = 0; i < itemsToSpawn; i++)
        {
            if (availableSpawnPoints.Count > 0)
            {
                // ランダムなスポーンポイントを選択
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];
                // アイテムを生成
                SpawnRandomItemAtPoint(selectedSpawnPoint);
                // 使用済みリストから削除
                availableSpawnPoints.RemoveAt(randomIndex);
            }
        }
    }

    /// <summary>
    /// 利用可能（空いている）スポーンポイントのリストを取得
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
    /// 指定スポーンポイントにランダムなアイテムを生成
    /// </summary>
    /// <param name="spawnPoint">スポーンポイント</param>
    private void SpawnRandomItemAtPoint(Transform spawnPoint)
    {
        // 利用可能なアイテムタイプを取得
        List<string> types = _factoryManager.GetAvailableTypes();
        if (types.Count == 0) return;
        // 確率に基づいてアイテムタイプを選択
        string selectedType = SelectItemByProbability(types);
        // アイテムを生成
        ItemBase item = _factoryManager.CreateItem(selectedType, spawnPoint.position);
        if (item != null)
        {
            // アイテムが取られたときのイベントに登録
            item.ItemHitAction += (collectedItem, type, itemType) =>
                HandleItemCollected(collectedItem, type, itemType, spawnPoint);
            // 管理リストに追加
            _spawnedItems.Add(item);
            _spawnPointOccupied[spawnPoint] = true;
        }
    }

    /// <summary>
    /// 確率に基づいてアイテムタイプを選択
    /// </summary>
    /// <param name="availableTypes">利用可能なタイプリスト</param>
    /// <returns>選択されたアイテムタイプ</returns>
    private string SelectItemByProbability(List<string> availableTypes)
    {
        // 全アイテムの確率の合計を計算
        float totalProbability = 0f;
        foreach (string type in availableTypes)
        {
            ItemData data = _factoryManager.GetItemData(type);
            if (data != null)
            {
                totalProbability += data.SpawnProbability;
            }
        }
        // ランダムな値を生成（0から合計確率まで）
        float randomValue = Random.Range(0f, totalProbability);
        float currentProbability = 0f;

        // 確率に基づいて選択
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
        // フォールバック（通常は発生しない）
        return availableTypes[0];
    }

    /// <summary>
    /// アイテムが取られたときに呼ばれる
    /// </summary>
    private void HandleItemCollected(ItemBase item, string type, ItemData.itemType itemType, Transform spawnPoint)
    {
        // 管理リストから削除
        _spawnedItems.Remove(item);
        _spawnPointOccupied[spawnPoint] = false;
        // インターバル後に新しいアイテムをスポーン
        if (_spawnPointCoroutines.ContainsKey(spawnPoint))
        {
            StopCoroutine(_spawnPointCoroutines[spawnPoint]);
        }
        _spawnPointCoroutines[spawnPoint] = StartCoroutine(RespawnItemAfterInterval(spawnPoint, _intervalTime));
    }

    /// <summary>
    /// インターバル後にアイテムを再スポーンするコルーチン
    /// </summary>
    /// <param name="originalSpawnPoint">元のスポーンポイント</param>
    /// <param name="interval">待機時間</param>
    private IEnumerator RespawnItemAfterInterval(Transform originalSpawnPoint, float interval)
    {
        yield return new WaitForSeconds(interval);
        // 全体の最大数チェック
        if (_spawnedItems.Count < _maxTotalItems)
        {
            // 利用可能なスポーンポイントを取得
            List<Transform> availableSpawnPoints = GetAvailableSpawnPoints();
            if (availableSpawnPoints.Count > 0)
            {
                // ランダムなスポーンポイントを選択
                int randomIndex = Random.Range(0, availableSpawnPoints.Count);
                Transform selectedSpawnPoint = availableSpawnPoints[randomIndex];
                // アイテムを生成
                SpawnRandomItemAtPoint(selectedSpawnPoint);
            }
        }
        // コルーチン管理から削除
        if (_spawnPointCoroutines.ContainsKey(originalSpawnPoint))
        {
            _spawnPointCoroutines.Remove(originalSpawnPoint);
        }
    }
}
