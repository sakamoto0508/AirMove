using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Title, Playing, PlayEnd, Tutorial }
    public GameState CurrentState { get; private set; } = GameState.Title;
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

    private void Start()
    {
        ChangeState(GameState.Title);
    }

    public void ChangeState(GameState newState)
    {
        CurrentState = newState;
        switch (newState)
        {
            case GameState.Title:
                HandleTitleState();
                break;
            case GameState.Playing:
                HandlePlayingState();
                break;
            case GameState.PlayEnd:
                HandlePlayingEndState();
                break;
            case GameState.Tutorial:
                HandleTutorialState();
                break;
        }
    }

    private void HandleTitleState()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM("TitleBGM");
        }
    }

    private void HandlePlayingState()
    {
        // �X�R�A���Z�b�g
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ScoreSum = 0;
            ScoreManager.Instance.ResetScoreMultiplier();
        }
        // �^�C�}�[�J�n
        TimeManager.Instance?.TimerStart();
        // BGM�Đ�
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayBGM("GameBGM");
        }
    }

    private void HandlePlayingEndState()
    {

        // �Q�[���I�[�o�[SE�Đ�
        AudioManager.Instance?.PlaySE("TimeUp");
        //�n�C�X�R�A����ƃ����L���O�o�^
        CheckAndSaveHighScore();
    }

    private void HandleTutorialState()
    {

    }

    private void CheckAndSaveHighScore()
    {
        if (ScoreManager.Instance == null || TimeManager.Instance == null || RankingManager.Instance == null)
            return;

        int finalScore = ScoreManager.Instance.ScoreSum;
        if (RankingManager.Instance.IsHighScore(finalScore))
        {
            AudioManager.Instance?.PlaySE("HighScore");
            RankingManager.Instance.AddScore("Player", finalScore);
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        ChangeState(GameState.Playing);
    }

    public void ReturnTitle()
    {
        SceneManager.LoadScene("Title");
        ChangeState(GameState.Title);
    }
}
