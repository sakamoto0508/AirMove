using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Title, Playing, PlayEnd, Tutorial }
    public GameState CurrentState { get; private set; } = GameState.Title;
    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "Title";
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private float _waitSeconds = 3f;
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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }  

    private void Start()
    {
        SetStateFromCurrentScene();
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
        StartCoroutine(CoroutineTitle());
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

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SetStateFromScene(scene.name);
    }

    private void SetStateFromCurrentScene()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SetStateFromScene(currentSceneName);
    }

    private void SetStateFromScene(string sceneName)
    {
        GameState targetState = CurrentState;
        if (sceneName == titleSceneName)
        {
            targetState = GameState.Title;
        }
        else if (sceneName == gameSceneName)
        {
            if (CurrentState != GameState.PlayEnd)
            {
                targetState = GameState.Playing;
            }
        }
        else if (sceneName == tutorialSceneName)
        {
            targetState = GameState.Tutorial;
        }
        // �X�e�[�g���ύX�����ꍇ�̂�ChangeState���Ă�
        if (targetState != CurrentState)
        {
            ChangeState(targetState);
        }
    }

    public void LoadSceneWithState(string sceneName,GameState newState)
    {
        ChangeState(newState);
        SceneManager.LoadScene(sceneName);
    }

    public void StartGame()
    {
        LoadSceneWithState(gameSceneName, GameState.Playing);
    }

    public void StartTutorial()
    {
        LoadSceneWithState(tutorialSceneName, GameState.Tutorial);
    }

    public void ReturnToTitle()
    {
        LoadSceneWithState(titleSceneName, GameState.Title);
    }

    // �^�C���A�b�v���ɌĂяo��
    public void TimeUp()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.PlayEnd);
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

    private IEnumerator CoroutineTitle()
    {
        yield return new WaitForSeconds(_waitSeconds);
        ReturnTitle();
    }
}
