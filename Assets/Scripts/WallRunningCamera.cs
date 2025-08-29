using UnityEngine;
using DG.Tweening;
using Unity.Cinemachine;

public class WallRunningCamera : MonoBehaviour
{
    public void DoFov(float endValue)
    {
        FindAnyObjectByType<CinemachineCamera>().Lens.FieldOfView = 60.25f;
    }

    public void DoTilt(float zTilt)
    {
        FindAnyObjectByType<CinemachineCamera>().Lens.Dutch = zTilt;
    }
}
