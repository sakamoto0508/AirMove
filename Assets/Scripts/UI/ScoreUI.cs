using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private Text _scoreText;
    

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = ("Score:"+ScoreManager.Instance.ScoreSum);
    }
}
