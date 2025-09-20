using System;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public Action TimeUpAction;
    public float _currentTime { get; private set; }
    [SerializeField] private float _initialTimer = 180f;
    private bool _timerRunning = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _timerRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerRunning && _currentTime >= 0)
        {
            _currentTime -= Time.deltaTime;
            if (_currentTime < 0)
            {
                _currentTime = 0;
                TimeUpAction?.Invoke();
                TimeUp();
            }
        }
    }
    public void TimerStart()
    {
        _currentTime = _initialTimer;
        _timerRunning = true;
    }

    public void StopTimer()
    {
        _timerRunning = false;
    }

    public void ResumeTimer()
    {
        _timerRunning = true;
    }

    public void ResetTimer()
    {
        _currentTime = _initialTimer;
        _timerRunning = false;
    }

    public void TimeUp()
    {
        _timerRunning = false;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.TimeUp();
        }
    }
}
