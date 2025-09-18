using UnityEngine;
using UnityEngine.UI;

public class RankingItem : MonoBehaviour
{
    [Header("Ranking Item UI")]
    [SerializeField] private Text rankText;
    [SerializeField] private Text playerNameText;
    [SerializeField] private Text scoreText;

    /// <summary>
    /// �����L���O�f�[�^��ݒ�
    /// </summary>
    public void SetRankingData(int rank, string playerName, int score)
    {
        if (rankText != null)
        {
            // ���ʂɉ����ĕ\����ς���i1�ʁA2�ʁA3�ʂ͓��ʕ\���j
            switch (rank)
            {
                case 1:
                    rankText.text = "1st";
                    rankText.color = Color.yellow; // ���F
                    break;
                case 2:
                    rankText.text = "2nd";
                    rankText.color = Color.gray; // ��F
                    break;
                case 3:
                    rankText.text = "3rd";
                    rankText.color = new Color(0.8f, 0.4f, 0f); // ���F
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
            // 1�ʂ͖��O������
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
            scoreText.text = score.ToString("N0"); // 3����؂�ŕ\��
            // 1�ʂ̓X�R�A������
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