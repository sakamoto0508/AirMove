using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class WallRunningCamera : MonoBehaviour
{
    /// <summary>
    /// カメラのFOVを変更
    /// </summary>
    /// <param name="endValue"></param>
    public void DoFov(float endValue)
    {
        FindAnyObjectByType<CinemachineCamera>().Lens.FieldOfView = 60.25f;
    }

    /// <summary>
    /// カメラの傾きを変更
    /// </summary>
    /// <param name="zTilt"></param>
    public void DoTilt(float zTilt)
    {
        FindAnyObjectByType<CinemachineCamera>().Lens.Dutch = zTilt;
    }
}
