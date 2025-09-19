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
        // 最初は非表示
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
            // エンターキーでも確定できるようにする
            nameInputField.onEndEdit.RemoveAllListeners();
            nameInputField.onEndEdit.AddListener(OnNameSubmit);

            // 既存の名前を初期値として設定
            if (PlayerNameManager.Instance != null)
            {
                nameInputField.text = PlayerNameManager.Instance.GetPlayerName();
            }
        }
    }

    /// <summary>
    /// 名前入力UIを表示
    /// </summary>
    public void ShowNameInput(System.Action<string> onConfirmed)
    {
        Debug.Log("ShowNameInput が呼ばれました");

        _onNameConfirmed = onConfirmed;

        if (nameInputPanel != null)
        {
            // 他のボタンは非表示にする
            _startButton.gameObject.SetActive(false);
            _tutorialButton.gameObject.SetActive(false);
            _rankingButton.gameObject.SetActive(false);
            _panelText.gameObject.SetActive(false);
            nameInputPanel.SetActive(true);
            Debug.Log("NameInputPanel を表示しました");
        }

        // 入力フィールドにフォーカスを当てる
        if (nameInputField != null)
        {
            nameInputField.Select();
            nameInputField.ActivateInputField();
        }
    }

    /// <summary>
    /// 名前入力UIを非表示
    /// </summary>
    /// <param name="isCanceled">キャンセルしたときだけボタンを戻す</param>
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

            Debug.Log("NameInputPanel を非表示にしました");
        }
    }

    private void OnConfirmClicked()
    {
        Debug.Log("確定ボタンがクリックされました");
        ConfirmName();
    }

    private void OnCancelClicked()
    {
        Debug.Log("キャンセルボタンがクリックされました");
        HideNameInput(true); // キャンセル時は true
    }

    private void OnNameSubmit(string inputText)
    {
        // エンターキーが押されたときの処理
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            Debug.Log("エンターキーが押されました");
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

        Debug.Log($"確定された名前: {playerName}");

        // プレイヤー名を保存
        if (PlayerNameManager.Instance != null)
        {
            PlayerNameManager.Instance.SetPlayerName(playerName);
            Debug.Log($"PlayerNameManager に名前を保存しました: {playerName}");
        }

        // 確定時はボタンを戻さない
        HideNameInput(false);

        // コールバック実行
        _onNameConfirmed?.Invoke(playerName);
    }

    private void Update()
    {
        // ESCキーでキャンセル
        if (Input.GetKeyDown(KeyCode.Escape) && nameInputPanel != null && nameInputPanel.activeInHierarchy)
        {
            OnCancelClicked();
        }
    }
}
