using UnityEngine;

public class PlayerNameManager : MonoBehaviour
{
    public static PlayerNameManager Instance { get; private set; }
    public string CurrentPlayerName { get; private set; } = "Player";
    private const string PLAYER_NAME_KEY = "PlayerName";
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadPlayerName();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// プレイヤー名を設定
    /// </summary>
    public void SetPlayerName(string playerName)
    {
        if (string.IsNullOrEmpty(playerName))
        {
            CurrentPlayerName = "Player";
        }
        else
        {
            CurrentPlayerName = playerName;
        }
        SavePlayerName();
    }

    /// <summary>
    /// プレイヤー名を取得
    /// </summary>
    public string GetPlayerName()
    {
        return CurrentPlayerName;
    }

    /// <summary>
    /// プレイヤー名を保存
    /// </summary>
    private void SavePlayerName()
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, CurrentPlayerName);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// プレイヤー名を読み込み
    /// </summary>
    private void LoadPlayerName()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            CurrentPlayerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player");
        }
    }
}
