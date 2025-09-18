using UnityEngine;
using UnityEngine.UI;

public class TitleButtonManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button TitleButton;
    public Button StartGameButton;
    public Button TutorialButton;
    public Button RankingButton;

    [Header("Name Input")]
    public NameInputUI nameInputUI;

    private void Start()
    {
        SetupButtons();

        // NameInputUIが見つからない場合は検索
        if (nameInputUI == null)
        {
            nameInputUI = FindObjectOfType<NameInputUI>();
        }
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(UpdateButtonVisibility), 0f, 0.1f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateButtonVisibility));
    }

    private void SetupButtons()
    {
        // ボタンイベントを直接メソッドに設定（Inspector設定不要）
        if (StartGameButton != null)
        {
            StartGameButton.onClick.RemoveAllListeners();
            StartGameButton.onClick.AddListener(OnStartGameClicked);
        }
        if (TutorialButton != null)
        {
            TutorialButton.onClick.RemoveAllListeners();
            TutorialButton.onClick.AddListener(OnTutorialClicked);
        }
        if (TitleButton != null)
        {
            TitleButton.onClick.RemoveAllListeners();
            TitleButton.onClick.AddListener(OnTitleClicked);
        }
        if (RankingButton != null)
        {
            RankingButton.onClick.RemoveAllListeners();
            RankingButton.onClick.AddListener(OnRankingClicked);
        }
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        if (GameManager.Instance == null) return;
        // 名前入力UIが表示中なら TitleButtonManager は何もしない
        if (nameInputUI != null && nameInputUI.gameObject.activeInHierarchy)
        {
            return;
        }

        GameManager.GameState currentState = GameManager.Instance.CurrentState;
        // 現在の状態に応じてボタンの表示/非表示を設定
        switch (currentState)
        {
            case GameManager.GameState.Title:
                // タイトル画面では、ゲーム開始、チュートリアル、ランキングボタンを表示
                SetButtonActive(StartGameButton, true);
                SetButtonActive(TutorialButton, true);
                SetButtonActive(RankingButton, true);
                SetButtonActive(TitleButton, false);
                break;
            case GameManager.GameState.Playing:
                // ゲーム中は全ボタン非表示
                SetButtonActive(StartGameButton, false);
                SetButtonActive(TutorialButton, false);
                SetButtonActive(RankingButton, false);
                SetButtonActive(TitleButton, false);
                break;
            case GameManager.GameState.PlayEnd:
                // ゲーム終了時は何も表示しない（自動的にタイトルに戻る）
                SetButtonActive(StartGameButton, false);
                SetButtonActive(TutorialButton, false);
                SetButtonActive(RankingButton, false);
                SetButtonActive(TitleButton, false);
                break;
            case GameManager.GameState.Tutorial:
                // チュートリアル中はタイトルに戻るボタンのみ表示
                SetButtonActive(StartGameButton, false);
                SetButtonActive(TutorialButton, false);
                SetButtonActive(RankingButton, false);
                SetButtonActive(TitleButton, true);
                break;

            case GameManager.GameState.Ranking:
                // ランキング画面ではタイトルに戻るボタンのみ表示
                SetButtonActive(StartGameButton, false);
                SetButtonActive(TutorialButton, false);
                SetButtonActive(RankingButton, false);
                SetButtonActive(TitleButton, true);
                break;
        }
    }

    private void SetButtonActive(Button button, bool isActive)
    {
        if (button != null)
        {
            button.gameObject.SetActive(isActive);
        }
    }

    // 修正：名前入力を待ってからゲーム開始
    public void OnStartGameClicked()
    {
        Debug.Log("スタートボタンがクリックされました");

        // 名前入力UIを表示してから ゲームを開始
        if (nameInputUI != null)
        {
            Debug.Log("名前入力UIを表示します");
            nameInputUI.ShowNameInput(OnNameConfirmed);
        }
        else
        {
            Debug.LogError("NameInputUIが見つかりません！");
            // 名前入力UIがない場合は直接ゲーム開始
            StartGame();
        }
    }

    private void OnNameConfirmed(string playerName)
    {
        Debug.Log($"名前が確定されました: {playerName}");
        // 名前が確定されたらゲーム開始
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("ゲームを開始します");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }

    public void OnTutorialClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartTutorial();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }

    public void OnTitleClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToTitle();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }

    public void OnRankingClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartRanking();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }

    public void OnRetryClicked()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.Retry();
        }
        else
        {
            Debug.LogError("GameManager.Instance is null!");
        }
    }
}