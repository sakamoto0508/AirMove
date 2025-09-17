using UnityEngine;
using UnityEngine.UI;

public class RankingButtonManager : MonoBehaviour
{
    [Header("Ranking UI Buttons")]
    public Button TitleButton;

    private void Start()
    {
        SetupButton();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(UpdateButtonVisibility), 0f, 0.1f);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(UpdateButtonVisibility));
    }

    private void SetupButton()
    {
        if(TitleButton != null)
        {
            TitleButton.onClick.RemoveAllListeners();
            TitleButton.onClick.AddListener(OnTitleClicked);
        }
        UpdateButtonVisibility();
    }

    private void UpdateButtonVisibility()
    {
        if (GameManager.Instance == null) return;

        GameManager.GameState currentState = GameManager.Instance.CurrentState;
        switch(currentState)
        {
            case GameManager.GameState.Ranking:
                // ランキング画面ではタイトルボタンとクリアボタンを表示
                SetButtonActive(TitleButton, true);
                break;

            default:
                // その他の状態では非表示
                SetButtonActive(TitleButton, false);
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

    public void OnTitleClicked()
    {
        if (SceneChanger.Instance != null)
        {
            SceneChanger.Instance.ChangeToTitle();
        }
    }
}
