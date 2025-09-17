using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;

public class RankingManager : MonoBehaviour
{
    public static RankingManager Instance { get; private set; }
    [SerializeField] private int _maxRankingCount = 10;
    private List<RankingData> _rankings = new List<RankingData>();
    private const string RANKING_KEY = "GameRanking";

    [System.Serializable]
    private class RankingList
    {
        public List<RankingData> _rankings;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadRankings();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// スコアをランキングに追加
    /// </summary>
    public void AddScore(string playerName, int score)
    {
        RankingData newData = new RankingData(playerName, score);
        _rankings.Add(newData);
        // スコアが高い順に並び替えて、最大件数を超えた分は切り捨て
        _rankings = _rankings.OrderByDescending(r => r.Score)
                          .Take(_maxRankingCount)
                          .ToList();
        SaveRankings();
    }

    /// <summary>
    /// ランキングを取得
    /// </summary>
    public List<RankingData> GetRankings()
    {
        // 内部リストをそのまま渡さずコピーを返す（外部から直接改変されないようにするため）
        return new List<RankingData>(_rankings);
    }

    /// <summary>
    /// ハイスコア判定
    /// </summary>
    public bool IsHighScore(int score)
    {
        // ランキングがまだ満杯でなければ無条件でハイスコア扱い
        if (_rankings.Count < _maxRankingCount)
            return true;
        // 最下位より高ければハイスコア
        return score > _rankings.Last().Score;
    }

    /// <summary>
    /// 順位を取得
    /// </summary>
    public int GetRank(int score)
    {
        int rank = 1;
        // 高いスコアから順に比較
        foreach (var data in _rankings)
        {
            if (score <= data.Score)
                rank++;
            else
                break;
        }
        return rank;
    }

    /// <summary>
    /// 現在の最高スコアを取得
    /// </summary>
    public int GetHighScore()
    {
        if (_rankings.Count > 0)
            return _rankings[0].Score;
        return 0;
    }

    /// <summary>
    /// ランキングを保存する
    /// </summary>
    private void SaveRankings()
    {
        string json = JsonUtility.ToJson(new RankingList { _rankings = _rankings });
        PlayerPrefs.SetString(RANKING_KEY, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// ランキングを読み込む（PlayerPrefs から JSON を復元）
    /// </summary>
    private void LoadRankings()
    {
        if (PlayerPrefs.HasKey(RANKING_KEY))
        {
            string json = PlayerPrefs.GetString(RANKING_KEY);
            try
            {
                RankingList rankingList = JsonUtility.FromJson<RankingList>(json);
                _rankings = rankingList._rankings ?? new List<RankingData>();
            }
            catch
            {
                _rankings = new List<RankingData>();
            }
        }
    }
}
