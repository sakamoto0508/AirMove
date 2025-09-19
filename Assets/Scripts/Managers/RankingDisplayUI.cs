using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankingDisplayUI : MonoBehaviour
{
    [Header("Ranking Display")]
    [SerializeField] private Transform _rankingParent;
    [SerializeField] private GameObject _rankingItemPrefab;
    [SerializeField] private Text _titleText;
    [SerializeField] private int _fontSize;

    [Header("�����L���O�ʒu����")]
    [SerializeField] private Vector2 _startPosition = new Vector2(0, -20f); // �ŏ��̃A�C�e���̈ʒu
    [SerializeField] private float _verticalSpacing = 45f;                  // �c�̍s�Ԋu
    [SerializeField] private float _horizontalSpacing = 0f;                 // ���̊Ԋu

    private List<GameObject> _rankingItems = new List<GameObject>();

    private void Start()
    {
        Invoke(nameof(DisplayRanking), 0.1f);
    }

    public void DisplayRanking()
    {
        ClearRankingItemsSafe();
        if (RankingManager.Instance == null)
        {
            return;
        }
        List<RankingData> rankings = RankingManager.Instance.GetRankings();
        if (_titleText != null)
        {
            _titleText.text = "RANKING";
        }
        if (rankings.Count == 0)
        {
            return;
        }
        for (int i = 0; i < rankings.Count; i++)
        {
            CreateRankingItemSafe(i + 1, rankings[i]);
        }
    }

    private void CreateRankingItemSafe(int rank, RankingData data)
    {
        if (_rankingParent == null)
        {
            return;
        }
        try
        {
            GameObject item = new GameObject($"RankingItem_{rank}");
            item.transform.SetParent(_rankingParent, false);
            RectTransform rectTransform = item.AddComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(500, 40);
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(1, 1);
            rectTransform.pivot = new Vector2(0.5f, 1);
            // ---- �ʒu�v�Z ----
            Vector2 offset = new Vector2((rank - 1) * _horizontalSpacing, -(rank - 1) * _verticalSpacing);
            rectTransform.anchoredPosition = _startPosition + offset;
            Text textComponent = item.AddComponent<Text>();
            textComponent.text = $"{GetRankText(rank)} {data.PlayerName} - {data.Score:N0}�_";
            textComponent.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            textComponent.fontSize = _fontSize;
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
