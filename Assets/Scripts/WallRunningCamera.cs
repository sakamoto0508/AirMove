using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class WallRunningCamera : MonoBehaviour
{
    /// <summary>
    /// �J������FOV��ύX
    /// </summary>
    /// <param name="endValue"></param>
    public void DoFov(float endValue)
    {
        FindAnyObjectByType<CinemachineCamera>().Lens.FieldOfView = 60.25f;
    }

    /// <summary>
    /// �J�����̌X����ύX
    /// </summary>
    /// <param name="zTilt"></param>
    public void DoTilt(float zTilt)
    {
        FindAnyObjectByType<CinemachineCamera>().Lens.Dutch = zTilt;
    }
}
