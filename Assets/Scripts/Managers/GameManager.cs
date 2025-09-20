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
        // シーンロードイベントに購読
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // メモリリークを防ぐために購読を解除
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // 少し待ってから初期化処理を行う（AudioManagerの初期化を確実にする）
        StartCoroutine(InitializeGameManager());
    }

    private IEnumerator InitializeGameManager()
    {
        // AudioManagerの初期化を待つ
        yield return new WaitForEndOfFrame();
        // 最初のシーンでのBGM処理を明示的に行う
        HandleInitialSceneBGM();
        // ゲームが開始されたシーンに基づいて初期状態を設定
        SetStateFromCurrentScene();
    }

    private void HandleInitialSceneBGM()
    {
        if (AudioManager.Instance == null) return;

        string currentSceneName = SceneManager.GetActiveScene().name;
        // 前のBGMを停止
        AudioManager.Instance.StopBGM();
        // 現在のシーンに応じてBGMを再生
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
        // 状態が実際に変更される場合にのみハンドラーをトリガー
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
        // タイトル状態での処理
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void HandlePlayingState()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        // スコアリセット
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.ScoreSum = 0;
            ScoreManager.Instance.ResetScoreMultiplier();
        }
        // タイマー開始
        TimeManager.Instance?.TimerStart();
    }

    private void HandlePlayingEndState()
    {
        // ゲームオーバーSE再生
        AudioManager.Instance?.PlaySE("TimeUp");
        //ハイスコア判定とランキング登録
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
        // ランキングUIを探して強制更新
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
        // AudioManagerが存在することを確認
        if (AudioManager.Instance == null)
        {
            Debug.LogWarning("AudioManager.Instance が null です。");
            return;
        }
        // 前のBGMを確実に停止
        AudioManager.Instance.StopBGM();
        // 少し待ってからBGMを再生（確実に停止させるため）
        StartCoroutine(PlaySceneBGMDelayed(scene.name));
    }

    private IEnumerator PlaySceneBGMDelayed(string sceneName)
    {
        yield return new WaitForSeconds(0.1f); // 短い待機時間

        // シーンに応じてBGMを再生
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
        // 他のシーンではBGMを再生しない
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
        // ステートが変更される場合のみChangeStateを呼ぶ
        if (targetState != CurrentState)
        {
            ChangeState(targetState);
        }
    }

    public void LoadSceneWithState(string sceneName, GameState newState)
    {
        // 明示的にBGMを停止してからシーン変更
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

    // タイムアップ時に呼び出す
    public void TimeUp()
    {
        if (CurrentState == GameState.Playing)
        {
            ChangeState(GameState.PlayEnd);
        }
    }

    public void Retry()
    {
        // BGMを停止してからリトライ
        AudioManager.Instance?.StopBGM();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ReturnTitle()
    {
        // BGMを停止してからタイトルに戻る
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
