using UnityEngine;
using UnityEngine.UI;

public class NameInputUI : MonoBehaviour
{
    [Header("Name Input UI")]
    [SerializeField] private GameObject nameInputPanel;
    [SerializeField] private InputField nameInputField;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _tutorialButton;
    [SerializeField] private Button _rankingButton;
    [SerializeField] private GameObject _panelText;

    private System.Action<string> _onNameConfirmed;

    private void Start()
    {
        SetupUI();
        // �ŏ��͔�\��
        HideNameInput(false);
    }

    private void SetupUI()
    {
        if (confirmButton != null)
        {
            confirmButton.onClick.RemoveAllListeners();
            confirmButton.onClick.AddListener(OnConfirmClicked);
        }

        if (cancelButton != null)
        {
            cancelButton.onClick.RemoveAllListeners();
            cancelButton.onClick.AddListener(OnCancelClicked);
        }

        if (nameInputField != null)
        {
            // �G���^�[�L�[�ł��m��ł���悤�ɂ���
            nameInputField.onEndEdit.RemoveAllListeners();
            nameInputField.onEndEdit.AddListener(OnNameSubmit);

            // �����̖��O�������l�Ƃ��Đݒ�
            if (PlayerNameManager.Instance != null)
            {
                nameInputField.text = PlayerNameManager.Instance.GetPlayerName();
            }
        }
    }

    /// <summary>
    /// ���O����UI��\��
    /// </summary>
    public void ShowNameInput(System.Action<string> onConfirmed)
    {
        _onNameConfirmed = onConfirmed;
        if (nameInputPanel != null)
        {
            // ���̃{�^���͔�\���ɂ���
            _startButton.gameObject.SetActive(false);
            _tutorialButton.gameObject.SetActive(false);
            _rankingButton.gameObject.SetActive(false);
            _panelText.gameObject.SetActive(false);
            nameInputPanel.SetActive(true);
        }
        // ���̓t�B�[���h�Ƀt�H�[�J�X�𓖂Ă�
        if (nameInputField != null)
        {
            nameInputField.Select();
            nameInputField.ActivateInputField();
        }
    }

    /// <summary>
    /// ���O����UI���\��
    /// </summary>
    /// <param name="isCanceled">�L�����Z�������Ƃ������{�^����߂�</param>
    public void HideNameInput(bool isCanceled)
    {
        if (nameInputPanel != null)
        {
            nameInputPanel.SetActive(false);

            if (isCanceled)
            {
                _startButton.gameObject.SetActive(true);
                _tutorialButton.gameObject.SetActive(true);
                _rankingButton.gameObject.SetActive(true);
                _panelText.gameObject.SetActive(true) ;
            }
        }
    }

    private void OnConfirmClicked()
    {
        ConfirmName();
    }

    private void OnCancelClicked()
    {
        HideNameInput(true); // �L�����Z������ true
    }

    private void OnNameSubmit(string inputText)
    {
        // �G���^�[�L�[�������ꂽ�Ƃ��̏���
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            ConfirmName();
        }
    }

    private void ConfirmName()
    {
        string playerName = nameInputField != null ? nameInputField.text : "Player";
        if (string.IsNullOrEmpty(playerName.Trim()))
        {
            playerName = "Player";
        }
        // �v���C���[����ۑ�
        if (PlayerNameManager.Instance != null)
        {
            PlayerNameManager.Instance.SetPlayerName(playerName);
        }
        // �m�莞�̓{�^����߂��Ȃ�
        HideNameInput(false);
        // �R�[���o�b�N���s
        _onNameConfirmed?.Invoke(playerName);
    }

    private void Update()
    {
        // ESC�L�[�ŃL�����Z��
        if (Input.GetKeyDown(KeyCode.Escape) && nameInputPanel != null && nameInputPanel.activeInHierarchy)
        {
            OnCancelClicked();
        }
    }
}
