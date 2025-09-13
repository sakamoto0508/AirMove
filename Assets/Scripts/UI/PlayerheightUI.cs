using UnityEngine;
using UnityEngine.UI;

public class PlayerHeightUI : MonoBehaviour
{
    [SerializeField] private Text _heightUI;
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponentInParent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_rb != null)
        {
            float height = _rb.position.y;
            _heightUI.text = $"çÇÇ≥: {height:F1} m";
        }
    }
}
