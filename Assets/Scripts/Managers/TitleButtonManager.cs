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
        // �{�^���C�x���g�𒼐ڃ��\�b�h�ɐݒ�iInspector�ݒ�s�v�j
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
        // ���݂̏�Ԃɉ����ă{�^���̕\��/��\����ݒ�
        switch (currentState)
        {
            case GameManager.GameState.Title:
                // �^�C�g����ʂł́A�Q�[���J�n�A�`���[�g���A���A�����L���O�{�^����\��
                SetButtonActive(StartGameButton, true);
                SetButtonActive(TutorialButton, true);
                SetButtonActive(RankingButton, true);
                SetButtonActive(TitleButton, false);
                break;

            case GameManager.GameState.Playing:
                // �Q�[�����͑S�{�^����\��
                SetButtonActive(StartGameButton, false);
                SetButtonActive(TutorialButton, false);
                SetButtonActive(RankingButton, false);
                SetButtonActive(TitleButton, false);
                break;

            case GameManager.GameState.PlayEnd:
                // �Q�[���I�����͉����\�����Ȃ��i�����I�Ƀ^�C�g���ɖ߂�j
                SetButtonActive(StartGameButton, false);
                SetButtonActive(TutorialButton, false);
                SetButtonActive(RankingButton, false);
                SetButtonActive(TitleButton, false);
                break;

            case GameManager.GameState.Tutorial:
                // �`���[�g���A�����̓^�C�g���ɖ߂�{�^���̂ݕ\��
                SetButtonActive(StartGameButton, false);
                SetButtonActive(TutorialButton, false);
                SetButtonActive(RankingButton, false);
                SetButtonActive(TitleButton, true);
                break;

            case GameManager.GameState.Ranking:
                // �����L���O��ʂł̓^�C�g���ɖ߂�{�^���̂ݕ\��
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

    // �{�^���C�x���g���\�b�h�iSceneChanger���g�p�j
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
