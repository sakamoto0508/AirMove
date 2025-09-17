using UnityEngine;
using UnityEngine.UI;

public class TitleButtonManager : MonoBehaviour
{
    [Header("UI Buttons")]
    public Button TitleButton;
    public Button StartGameButton;
    public Button TutorialButton;
    public Button RankingButton;
    private void Start()
    {
        SetupButtons();
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

    // ボタンイベントメソッド（SceneChangerを使用）
    public void OnStartGameClicked()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.ChangeToGame();
        }
        else
        {
            Debug.LogError("SceneChanger.Instance is null!");
        }
    }

    public void OnTutorialClicked()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.ChangeToTutorial();
        }
        else
        {
            Debug.LogError("SceneChanger.Instance is null!");
        }
    }

    public void OnTitleClicked()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.ChangeToTitle();
        }
        else
        {
            Debug.LogError("SceneChanger.Instance is null!");
        }
    }

    public void OnRankingClicked()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.ChangeToRanking();
        }
        else
        {
            Debug.LogError("SceneChanger.Instance is null!");
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
