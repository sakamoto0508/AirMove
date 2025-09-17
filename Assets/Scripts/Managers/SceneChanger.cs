using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance { get; private set; }

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
        }
    }

    public void SceneChange(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ChangeToTitle()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ReturnToTitle();
        }
        else
        {
            SceneManager.LoadScene("Title");
        }
    }

    public void ChangeToGame()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartGame();
        }
        else
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void ChangeToTutorial()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartTutorial();
        }
        else
        {
            SceneManager.LoadScene("Tutorial");
        }
    }

    public void ChangeToRanking()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.StartRanking();
        }
        else
        {
            SceneManager.LoadScene("Ranking");
        }
    }
}