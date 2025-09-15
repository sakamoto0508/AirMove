using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Title, Playing, Tutorial }
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
                break;
            case GameState.Playing:
                ScoreManager.Instance.ScoreSum = 0;
                TimeManager.Instance.TimerStart();
                break;
            case GameState.Tutorial:
                break;
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
