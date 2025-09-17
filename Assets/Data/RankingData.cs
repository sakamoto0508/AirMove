using UnityEngine;

[System.Serializable]
public class RankingData : MonoBehaviour
{
    public string PlayerName;
    public int Score;
    public RankingData(string name,int playerScore)
    {
        PlayerName = name;
        Score = playerScore;
    }
}
