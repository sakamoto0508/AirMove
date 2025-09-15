using System;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance;
    public Action TimerUpAction;
    public float Timer;
    [SerializeField] private float _initialTimer = 180f;
    private bool _timerRunning = false;
    private bool _timerPause = false;

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
        _timerPause = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timerRunning && !_timerPause)
        {
            if (Timer >= 0)
            {
                Timer -= Time.deltaTime;
            }
            else
            {
                TimerUpAction?.Invoke();
                Timer = 0;
                _timerRunning = false;
            }
        }
    }
    public void TimerStart()
    {
        Timer = _initialTimer;
        _timerRunning = true;
        _timerPause = false;
    }

    public void StopTimer()
    {
        _timerPause = true;
    }

    public void ResumeTimer()
    {
        _timerPause = false;
    }

    /// <summary>
    /// シーン切り替え時にイベントをクリア
    /// </summary>
    private void OnDestroy()
    {
        if (Instance == this)
        {
            TimeEventManager.ClearAllEvents();
        }
    }
}
