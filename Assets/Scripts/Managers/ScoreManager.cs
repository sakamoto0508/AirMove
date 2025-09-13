using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;
    public int ScoreSum = 0;
    private float _scoreMultiplier = 1.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int score)
    {
        int multipliedScore = Mathf.RoundToInt(score * _scoreMultiplier);
        ScoreSum += multipliedScore;
    }

    public void SetScoreMultiplier(float multiplier)
    {
        _scoreMultiplier = multiplier;
    }

    public void ResetScoreMultiplier()
    {
        _scoreMultiplier = 1.0f;
    }
}
