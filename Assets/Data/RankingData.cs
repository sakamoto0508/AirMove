using UnityEngine;

[System.Serializable]
public class RankingData
{
    public string PlayerName;
    public int Score;
    public RankingData(string name,int playerScore)
    {
        PlayerName = name;
        Score = playerScore;
    }

    public RankingData()
    {
        PlayerName = "";
        Score = 0;
    }
}
