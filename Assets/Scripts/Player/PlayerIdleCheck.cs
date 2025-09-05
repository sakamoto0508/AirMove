using UnityEngine;

public class PlayerIdleCheck : MonoBehaviour
{
    public bool IsIdle { get; private set; } = true;
    private Rigidbody _rb;
    private Vector2 _currentInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
