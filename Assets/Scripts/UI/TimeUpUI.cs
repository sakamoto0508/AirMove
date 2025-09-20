using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class TimeUpUI : MonoBehaviour
{
    [SerializeField] private Image _timeUp;

    private void Awake()
    {
        TimeManager.Instance.TimeUpAction += TimeUpImage;
    }

    private void Start()
    {
        _timeUp.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        TimeManager.Instance.TimeUpAction -= TimeUpImage;
    }

    private void TimeUpImage()
    {
        _timeUp.gameObject.SetActive(true);
    }
}
