using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingDisplayUI : MonoBehaviour
{
    [Header("Ranking Display")]
    [SerializeField] private Transform rankingParent;
    [SerializeField] private GameObject rankingItemPrefab;
    [SerializeField] private Text titleText;

    [Header("�����L���O�ʒu����")]
    [SerializeField] private Vector2 startPosition = new Vector2(0, -20f); // �ŏ��̃A�C�e���̈ʒu
    [SerializeField] private float verticalSpacing = 45f;                  // �c�̍s�Ԋu
    [SerializeField] private float horizontalSpacing = 0f;                 // ���̊Ԋu

    private List<GameObject> _rankingItems = new List<GameObject>();

    private void Start()
    {
        Debug.Log("RankingDisplayUI: Start");
        Invoke(nameof(DisplayRanking), 0.1f);
    }

    public void DisplayRanking()
    {
        Debug.Log("=== �����L���O�\���J�n ===");
        ClearRankingItemsSafe();

        if (RankingManager.Instance == null)
        {
            Debug.LogError("RankingManager.Instance is null");
            return;
        }

        List<RankingData> rankings = RankingManager.Instance.GetRankings();
        Debug.Log($"�擾���������L���O��: {rankings.Count}");

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

        Debug.Log($"=== �쐬�����B�A�C�e����: {_rankingItems.Count} ===");
    }

    private void CreateRankingItemSafe(int rank, RankingData data)
    {
        if (rankingParent == null)
        {
            Debug.LogError("rankingParent �� null �ł�");
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

            // ---- �ʒu�v�Z ----
            Vector2 offset = new Vector2((rank - 1) * horizontalSpacing, -(rank - 1) * verticalSpacing);
            rectTransform.anchoredPosition = startPosition + offset;

            Text textComponent = item.AddComponent<Text>();
            textComponent.text = $"{GetRankText(rank)} {data.PlayerName} - {data.Score:N0}�_";
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
            Debug.Log($" {rank}�ʃA�C�e���쐬����: {data.PlayerName} {data.Score}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"�A�C�e���쐬�G���[: {e.Message}");
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

    [ContextMenu("�����L���O�ĕ\��")]
    public void RefreshRanking()
    {
        DisplayRanking();
    }
}
