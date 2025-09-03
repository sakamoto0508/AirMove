using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraManager : MonoBehaviour
{
    private CinemachineCamera _cinemachineCamera;
    private Coroutine _fovCoroutine;
    private Coroutine _tiltCoroutine;
    public float _defaultFov { get; private set; } = 80f;
    public float _defaultTilt { get; private set; } =0f;
    [SerializeField] private float _smoothCameraTime = 0.1f;
    private void Start()
    {
        _cinemachineCamera = FindAnyObjectByType<CinemachineCamera>();
    }
    /// <summary>
    /// �J������FOV��ύX
    /// </summary>
    /// <param name="endValue"></param>
    public void DoFov(float targetFov)
    {
        if (_fovCoroutine != null)
        {
            StopCoroutine(_fovCoroutine);
        }
        _fovCoroutine = StartCoroutine(SmoothCameraFov(targetFov));
    }

    /// <summary>
    /// �J�����̌X����ύX
    /// </summary>
    /// <param name="zTilt"></param>
    public void DoTilt(float zTilt)
    {
        if (_tiltCoroutine != null)
        {
            StopCoroutine(_tiltCoroutine);
        }
        _tiltCoroutine = StartCoroutine(SmoothCameraTilt(zTilt));
    }

    private IEnumerator SmoothCameraFov(float targetFov)
    {
        //���݂�FOV���擾
        float startFov = _cinemachineCamera.Lens.FieldOfView;
        float leapTime = 0f;
        while(leapTime < _smoothCameraTime)
        {
            leapTime += Time.deltaTime;
            _cinemachineCamera.Lens.FieldOfView = Mathf.Lerp(startFov, targetFov, leapTime / _smoothCameraTime);
            yield return null;
        }
        _cinemachineCamera.Lens.FieldOfView = targetFov;
        _fovCoroutine = null;
    }

    private IEnumerator SmoothCameraTilt(float zTilt)
    {
        //���݂̌X�����擾
        float startTilt =_cinemachineCamera.Lens.Dutch;
        float learpTime = 0f;
        while (learpTime < _smoothCameraTime)
        {
            learpTime += Time.deltaTime;
            _cinemachineCamera.Lens.Dutch = Mathf.Lerp(startTilt, zTilt, learpTime / _smoothCameraTime);
            yield return null;
        }
        _cinemachineCamera.Lens.Dutch = zTilt;
        _tiltCoroutine = null;
    }
}
