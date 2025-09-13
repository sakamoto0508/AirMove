using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text _scoreText;

    private void Start()
    {
        if (_scoreText == null)
        {
            _scoreText = GameObject.Find("ScoreUI").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = ("Score:" + ScoreManager.Instance.ScoreSum);
    }
}
