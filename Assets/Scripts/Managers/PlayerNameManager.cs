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
    /// �v���C���[����ݒ�
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
    /// �v���C���[�����擾
    /// </summary>
    public string GetPlayerName()
    {
        return CurrentPlayerName;
    }

    /// <summary>
    /// �v���C���[����ۑ�
    /// </summary>
    private void SavePlayerName()
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, CurrentPlayerName);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// �v���C���[����ǂݍ���
    /// </summary>
    private void LoadPlayerName()
    {
        if (PlayerPrefs.HasKey(PLAYER_NAME_KEY))
        {
            CurrentPlayerName = PlayerPrefs.GetString(PLAYER_NAME_KEY, "Player");
        }
    }
}
