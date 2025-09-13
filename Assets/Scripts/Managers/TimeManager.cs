using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public Action TimerStop;
    public float Timer;
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
        if (_timerRunning)
        {
            if (Timer >= 0)
            {
                Timer -= Time.deltaTime;
            }
            else
            {
                TimerStop?.Invoke();
                Timer = 0;
                _timerRunning = false;
            }
        }
    }
    public void TimerStart()
    {
        Timer = _initialTimer;
        _timerRunning = true;
    }
}
