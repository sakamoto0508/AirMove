// GameManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { Title, Playing, PlayEnd, Tutorial, Ranking }
    public GameState CurrentState { get; private set; } = GameState.Title;
    [Header("Scene Names")]
    [SerializeField] private string titleSceneName = "Title";
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] private string tutorialSceneName = "Tutorial";
    [SerializeField] private string rankingSceneName = "Ranking";
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
            return;
        }
        // �V�[�����[�h�C�x���g�ɍw��
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // ���������[�N��h�����߂ɍw�ǂ�����
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // �����҂��Ă��珉�����������s���iAudioManager�̏��������m���ɂ���j
        StartCoroutine(InitializeGameManager());
    }

    private IEnumerator InitializeGameManager()
    {
        // AudioManager�̏�������҂�
        yield return new WaitForEndOfFrame();
        // �ŏ��̃V�[���ł�BGM�����𖾎��I�ɍs��
        HandleInitialSceneBGM();
        // �Q�[�����J�n���ꂽ�V�[���Ɋ�Â��ď�����Ԃ�ݒ�
        SetStateFromCurrentScene();
    }

    private void HandleInitialSceneBGM()
    {
        if (AudioManager.Instance == null) return;

        string currentSceneName = SceneManager.GetActiveScene().name;
        // �O��BGM���~
        AudioManager.Instance.StopBGM();
        // ���݂̃V�[���ɉ�����BGM���Đ�
        if (currentSceneName == titleSceneName)
        {
            AudioManager.Instance.PlayBGM("TitleBGM");
        }
        else if (currentSceneName == gameSceneName)
        {
            AudioManager.Instance.PlayBGM("GameBGM");
        }
        else if(currentSceneName == tutorialSceneName)
        {
            AudioManager.Instance.PlayBGM("GameBGM");
        }
    }

    public void ChangeState(GameState newState)
    {
        // ��Ԃ����ۂɕύX�����ꍇ�ɂ̂݃n���h���[���g���K�[
        if (CurrentState == newState) return;

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
            case GameState.Ranking:
                HandleRankingState();
                break;
        }
    }

    private void HandleTitleState()
    {
        // �^�C�g����Ԃł̏���
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HandlePlayingState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // �X�R�A���Z�b�g
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ScoreSum = 0;
            ScoreManager.Instance.ResetScoreMultiplier();
        }
        // �^�C�}�[�J�n
        TimeManager.Instance?.TimerStart();
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
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void HandleRankingState()
    {
        // �����L���OUI��T���ċ����X�V
        RankingDisplayUI ui = FindAnyObjectByType<RankingDisplayUI>();
        if (ui != null)
        {
            ui.DisplayRanking();
        }
    }

    private void CheckAndSaveHighScore()
    {
        if (ScoreManager.Instance == null || TimeManager.Instance == null || RankingManager.Instance == null)
            return;

        int finalScore = ScoreManager.Instance.ScoreSum;
        string playerName = PlayerNameManager.Instance != null ?
                            PlayerNameManager.Instance.GetPlayerName() : "Player";
        if (RankingManager.Instance.IsHighScore(finalScore))
        {
            AudioManager.Instance?.PlaySE("HighScore");
            RankingManager.Instance.AddScore(playerName, finalScore);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // AudioManager�����݂��邱�Ƃ��m�F
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager.Instance �� null �ł��B");
            return;
        }
        // �O��BGM���m���ɒ�~
        AudioManager.Instance.StopBGM();
        // �����҂��Ă���BGM���Đ��i�m���ɒ�~�����邽�߁j
        StartCoroutine(PlaySceneBGMDelayed(scene.name));
    }

    private IEnumerator PlaySceneBGMDelayed(string sceneName)
    {
        yield return new WaitForSeconds(0.1f); // �Z���ҋ@����

        // �V�[���ɉ�����BGM���Đ�
        if (sceneName == titleSceneName)
        {
            AudioManager.Instance.SetBGMVolume(0.03f);
            AudioManager.Instance?.PlayBGM("TitleBGM");
        }
        else if (sceneName == gameSceneName)
        {
            AudioManager.Instance.SetBGMVolume(0.03f);
            AudioManager.Instance?.PlayBGM("GameBGM");
        }
        // ���̃V�[���ł�BGM���Đ����Ȃ�
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
        else if (sceneName == rankingSceneName)
        {
            targetState = GameState.Ranking;
        }
        // �X�e�[�g���ύX�����ꍇ�̂�ChangeState���Ă�
        if (targetState != CurrentState)
        {
            ChangeState(targetState);
        }
    }

    public void LoadSceneWithState(string sceneName, GameState newState)
    {
        // �����I��BGM���~���Ă���V�[���ύX
        AudioManager.Instance?.StopBGM();
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

    public void StartRanking()
    {
        LoadSceneWithState(rankingSceneName, GameState.Ranking);
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
        // BGM���~���Ă��烊�g���C
        AudioManager.Instance?.StopBGM();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnTitle()
    {
        // BGM���~���Ă���^�C�g���ɖ߂�
        AudioManager.Instance?.StopBGM();
        SceneManager.LoadScene("Title");
    }

    public void CompleteTutorial()
    {
        if (CurrentState == GameState.Tutorial)
        {
            StartCoroutine(CoroutineTitle());
        }
    }

    private IEnumerator CoroutineTitle()
    {
        yield return new WaitForSeconds(_waitSeconds);
        ReturnTitle();
    }
}
