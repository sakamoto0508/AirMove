using UnityEngine;

public class PlayerPeek : MonoBehaviour
{
    public bool _isPeeking { get; private set; } = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPeek()
    {
        Debug.Log("Peek");
        _isPeeking = true;
    }

    public void StopPeek()
    {
        Debug.Log("Stop Peek");
        _isPeeking = false;
    }

    public bool IsPeeking()
    {
        return _isPeeking;
    }
}
