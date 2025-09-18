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

        // NameInputUI��������Ȃ��ꍇ�͌���
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
        // ���O����UI���\�����Ȃ� TitleButtonManager �͉������Ȃ�
        if (nameInputUI != null && nameInputUI.gameObject.activeInHierarchy)
        {
            return;
        }

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

    // �C���F���O���͂�҂��Ă���Q�[���J�n
    public void OnStartGameClicked()
    {
        Debug.Log("�X�^�[�g�{�^�����N���b�N����܂���");

        // ���O����UI��\�����Ă��� �Q�[�����J�n
        if (nameInputUI != null)
        {
            Debug.Log("���O����UI��\�����܂�");
            nameInputUI.ShowNameInput(OnNameConfirmed);
        }
        else
        {
            Debug.LogError("NameInputUI��������܂���I");
            // ���O����UI���Ȃ��ꍇ�͒��ڃQ�[���J�n
            StartGame();
        }
    }

    private void OnNameConfirmed(string playerName)
    {
        Debug.Log($"���O���m�肳��܂���: {playerName}");
        // ���O���m�肳�ꂽ��Q�[���J�n
        StartGame();
    }

    private void StartGame()
    {
        Debug.Log("�Q�[�����J�n���܂�");

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