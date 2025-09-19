using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [SerializeField] private Text _timerText;

    private void Start()
    {
        if (_timerText == null)
        {
            _timerText = GameObject.Find("TimerUI").GetComponent<Text>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _timerText.text = ("Timer:" + TimeManager.Instance._currentTime.ToString("F1"));
    }
}
