using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingDisplayUI : MonoBehaviour
{
    [Header("Ranking Display")]
    [SerializeField] private Transform rankingParent;
    [SerializeField] private GameObject rankingItemPrefab;
    [SerializeField] private Text titleText;

    [Header("ランキング位置調整")]
    [SerializeField] private Vector2 startPosition = new Vector2(0, -20f); // 最初のアイテムの位置
    [SerializeField] private float verticalSpacing = 45f;                  // 縦の行間隔
    [SerializeField] private float horizontalSpacing = 0f;                 // 横の間隔

    private List<GameObject> _rankingItems = new List<GameObject>();

    private void Start()
    {
        Debug.Log("RankingDisplayUI: Start");
        Invoke(nameof(DisplayRanking), 0.1f);
    }

    public void DisplayRanking()
    {
        Debug.Log("=== ランキング表示開始 ===");
        ClearRankingItemsSafe();

        if (RankingManager.Instance == null)
        {
            Debug.LogError("RankingManager.Instance is null");
            return;
        }

        List<RankingData> rankings = RankingManager.Instance.GetRankings();
        Debug.Log($"取得したランキング数: {rankings.Count}");

        if (titleText != null)
        {
            titleText.text = "RANKING";
        }

        if (rankings.Count == 0)
        {
            return;
        }

        for (int i = 0; i < rankings.Count; i++)
        {
            CreateRankingItemSafe(i + 1, rankings[i]);
        }

        Debug.Log($"=== 作成完了。アイテム数: {_rankingItems.Count} ===");
    }

    private void CreateRankingItemSafe(int rank, RankingData data)
    {
        if (rankingParent == null)
        {
            Debug.LogError("rankingParent が null です");
            return;
        }

        try
        {
            GameObject item = new GameObject($"RankingItem_{rank}");
            item.transform.SetParent(rankingParent, false);

            RectTransform rectTransform = item.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(500, 40);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);

            // ---- 位置計算 ----
            Vector2 offset = new Vector2((rank - 1) * horizontalSpacing, -(rank - 1) * verticalSpacing);
            rectTransform.anchoredPosition = startPosition + offset;

            Text textComponent = item.AddComponent<Text>();
            textComponent.text = $"{GetRankText(rank)} {data.PlayerName} - {data.Score:N0}点";
            textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComponent.fontSize = 18;
            textComponent.alignment = TextAnchor.MiddleCenter;

            if (rank == 1)
            {
                textComponent.color = Color.yellow;
                textComponent.fontStyle = FontStyle.Bold;
            }
            else if (rank == 2)
            {
                textComponent.color = Color.gray;
            }
            else if (rank == 3)
            {
                textComponent.color = new Color(0.8f, 0.4f, 0f);
            }
            else
            {
                textComponent.color = Color.white;
            }

            _rankingItems.Add(item);
            Debug.Log($" {rank}位アイテム作成完了: {data.PlayerName} {data.Score}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"アイテム作成エラー: {e.Message}");
        }
    }

    private string GetRankText(int rank)
    {
        return rank switch
        {
            1 => "1st",
            2 => "2nd",
            3 => "3rd",
            _ => $"{rank}th"
        };
    }

    private void ClearRankingItemsSafe()
    {
        foreach (GameObject item in _rankingItems)
        {
            if (item != null)
            {
                Destroy(item);
            }
        }
        _rankingItems.Clear();
    }

    private void OnDestroy()
    {
        ClearRankingItemsSafe();
    }

    [ContextMenu("ランキング再表示")]
    public void RefreshRanking()
    {
        DisplayRanking();
    }
}
