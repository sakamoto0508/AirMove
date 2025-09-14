using System;
using UnityEngine;

public static class TimeEventManager
{
    public static Action TimeStop;
    public static Action TimeStart;

    /// <summary>
    /// ���Ԓ�~�C�x���g�𔭉�
    /// </summary>
    public static void InvokeTimeStop()
    {
        TimeStop?.Invoke();
    }

    /// <summary>
    /// ���ԊJ�n�C�x���g�𔭉�
    /// </summary>
    public static void InvokeTimeStart()
    {
        TimeStart?.Invoke();
    }

    /// <summary>
    /// �S�ẴC�x���g���N���A�i�V�[���؂�ւ����ȂǂɎg�p�j
    /// </summary>
    public static void ClearAllEvents()
    {
        TimeStop = null;
        TimeStart = null;
    }
}
