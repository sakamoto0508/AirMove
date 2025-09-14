using System;
using UnityEngine;

public static class TimeEventManager
{
    public static Action TimeStop;
    public static Action TimeStart;

    /// <summary>
    /// 時間停止イベントを発火
    /// </summary>
    public static void InvokeTimeStop()
    {
        TimeStop?.Invoke();
    }

    /// <summary>
    /// 時間開始イベントを発火
    /// </summary>
    public static void InvokeTimeStart()
    {
        TimeStart?.Invoke();
    }

    /// <summary>
    /// 全てのイベントをクリア（シーン切り替え時などに使用）
    /// </summary>
    public static void ClearAllEvents()
    {
        TimeStop = null;
        TimeStart = null;
    }
}
