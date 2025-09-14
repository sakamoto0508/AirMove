using System;
using UnityEngine;

public class TheWorldItem : ItemBase
{
    public override void OnEffectStart()
    {
        base.OnEffectStart();
        TimeEventManager.InvokeTimeStop();
        Debug.Log("TimeStop");
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.StopTimer();
        }
    }

    public override void OnEffectEnd()
    {
        base.OnEffectEnd();
        TimeEventManager.InvokeTimeStart();
        Debug.Log("effectend");
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.ResumeTimer();
        }
    }

    public override void SetUp(ItemData itemData)
    {
        base.SetUp(itemData);
    }
}
