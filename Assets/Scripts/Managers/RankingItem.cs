using UnityEngine;
using UnityEngine.UI;

public class RankingItem : MonoBehaviour
{
    [Header("Ranking Item UI")]
    [SerializeField] private Text rankText;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text scoreText;

    /// <summary>
    /// ランキングデータを設定
    /// </summary>
    public void SetRankingData(int rank, string playerName, int score)
    {
        if (rankText != null)
        {
            // 順位に応じて表示を変える（1位、2位、3位は特別表示）
            switch (rank)
            {
                case 1:
                    rankText.text = "1st";
                    rankText.color = Color.yellow; // 金色
                    break;
                case 2:
                    rankText.text = "2nd";
                    rankText.color = Color.gray; // 銀色
                    break;
                case 3:
                    rankText.text = "3rd";
                    rankText.color = new Color(0.8f, 0.4f, 0f); // 銅色
                    break;
                default:
                    rankText.text = rank + "th";
                    rankText.color = Color.white;
                    break;
            }
        }

        if (playerNameText != null)
        {
            playerNameText.text = playerName;
            // 1位は名前も強調
            if (rank == 1)
            {
                playerNameText.color = Color.yellow;
            }
            else
            {
                playerNameText.color = Color.white;
            }
        }

        if (scoreText != null)
        {
            scoreText.text = score.ToString("N0"); // 3桁区切りで表示
            // 1位はスコアも強調
            if (rank == 1)
            {
                scoreText.color = Color.yellow;
            }
            else
            {
                scoreText.color = Color.white;
            }
        }
    }
}