using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.BoolParameter;

public class ItemEffectUI : MonoBehaviour
{
    public static ItemEffectUI Instance { get; private set; }
    [SerializeField] private GameObject _effectTextPanel;
    [SerializeField] private Text _text;
    [SerializeField] private float _displayTime = 2f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if(_effectTextPanel != null)
        {
            _effectTextPanel.SetActive(false);
        }
    }

    public void ShowEffect(string effectName)
    {
        if (_effectTextPanel != null && _text != null)
        {
            StartCoroutine(DisplayEffect(effectName));
        }
    }

    private IEnumerator DisplayEffect(string effectName)
    {
        _effectTextPanel.SetActive(true);
        _text.text = effectName + " å¯â î≠ìÆÅI";

        yield return new WaitForSeconds(_displayTime);

        _effectTextPanel.SetActive(false);
    }
}
