using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class TutorialText : MonoBehaviour
{
    [Header("UI ê›íË")]
    [SerializeField] private RectTransform tutorialUI;
    [SerializeField] private float fadeTime = 0.8f;
    [SerializeField] private Vector2 showPosition = new Vector2(-350f, 120f);
    [SerializeField] private Vector2 hidePosition = new Vector2(350f, 120f);
    private CanvasGroup canvasGroup;
    private Tween currentTween;

    private void Awake()
    {
        canvasGroup = tutorialUI.GetComponent<CanvasGroup>();
        tutorialUI.anchoredPosition = hidePosition;
        canvasGroup.alpha = 0f;
        tutorialUI.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            HideUI();
        }
    }

    private void ShowUI()
    {
        tutorialUI.gameObject.SetActive(true);

        currentTween?.Kill(); // ëOÇÃTweenÇÉLÉÉÉìÉZÉã
        Sequence seq = DOTween.Sequence();
        seq.Append(tutorialUI.DOAnchorPos(showPosition, fadeTime).SetEase(Ease.OutQuart));
        seq.Join(canvasGroup.DOFade(1f, fadeTime));
        currentTween = seq;
    }

    private void HideUI()
    {
        currentTween?.Kill();
        Sequence seq = DOTween.Sequence();
        seq.Append(tutorialUI.DOAnchorPos(hidePosition, fadeTime).SetEase(Ease.InQuart));
        seq.Join(canvasGroup.DOFade(0f, fadeTime));
        seq.OnComplete(() => tutorialUI.gameObject.SetActive(false));
        currentTween = seq;
    }
}
