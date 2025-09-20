using System.Runtime.InteropServices;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class TimeUpUI : MonoBehaviour
{
    [SerializeField] private Image _timeUp;
    [SerializeField] private float _scaleMultiplier = 1.5f;
    [SerializeField] private float _scaleDuration = 0.3f;
    [SerializeField] private float _scaleBackDuration = 0.2f;
    [SerializeField] private Ease _scaleEase = Ease.OutBack;
    private Vector3 _originalScale;
    private void Awake()
    {
        TimeManager.Instance.TimeUpAction += TimeUpImage;
    }

    private void Start()
    {
        _timeUp.gameObject.SetActive(false);
        _originalScale = _timeUp.transform.localScale;
    }

    private void OnDestroy()
    {
        TimeManager.Instance.TimeUpAction -= TimeUpImage;
        _timeUp.transform.DOKill();
    }

    private void TimeUpImage()
    {
        _timeUp.gameObject.SetActive(true);
        _timeUp.transform.localScale = _originalScale;
        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence
            .Append(_timeUp.transform.DOScale(_originalScale * _scaleMultiplier, _scaleDuration)
                .SetEase(_scaleEase))
            .Append(_timeUp.transform.DOScale(_originalScale, _scaleBackDuration)
                .SetEase(Ease.OutQuad));
    }
}
