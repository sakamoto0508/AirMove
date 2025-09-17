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
    /// �X�R�A�������L���O�ɒǉ�
    /// </summary>
    public void AddScore(string playerName, int score)
    {
        RankingData newData = new RankingData(playerName, score);
        _rankings.Add(newData);
        // �X�R�A���������ɕ��ёւ��āA�ő匏���𒴂������͐؂�̂�
        _rankings = _rankings.OrderByDescending(r => r.Score)
                          .Take(_maxRankingCount)
                          .ToList();
        SaveRankings();
    }

    /// <summary>
    /// �����L���O���擾
    /// </summary>
    public List<RankingData> GetRankings()
    {
        // �������X�g�����̂܂ܓn�����R�s�[��Ԃ��i�O�����璼�ډ��ς���Ȃ��悤�ɂ��邽�߁j
        return new List<RankingData>(_rankings);
    }

    /// <summary>
    /// �n�C�X�R�A����
    /// </summary>
    public bool IsHighScore(int score)
    {
        // �����L���O���܂����t�łȂ���Ζ������Ńn�C�X�R�A����
        if (_rankings.Count < _maxRankingCount)
            return true;
        // �ŉ��ʂ�荂����΃n�C�X�R�A
        return score > _rankings.Last().Score;
    }

    /// <summary>
    /// ���ʂ��擾
    /// </summary>
    public int GetRank(int score)
    {
        int rank = 1;
        // �����X�R�A���珇�ɔ�r
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
    /// ���݂̍ō��X�R�A���擾
    /// </summary>
    public int GetHighScore()
    {
        if (_rankings.Count > 0)
            return _rankings[0].Score;
        return 0;
    }

    /// <summary>
    /// �����L���O��ۑ�����
    /// </summary>
    private void SaveRankings()
    {
        string json = JsonUtility.ToJson(new RankingList { _rankings = _rankings });
        PlayerPrefs.SetString(RANKING_KEY, json);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �����L���O��ǂݍ��ށiPlayerPrefs ���� JSON �𕜌��j
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
