using Unity.Cinemachine;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public bool _isAiming { get; private set; } = false;
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private Camera _cameraFPS;
    [SerializeField] private int _priorityHigh = 1;
    [SerializeField] private int _priorityLow = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraMain.depth = _priorityHigh;
        _cameraFPS.depth = _priorityLow;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAim()
    {
        Debug.Log("Aiming");
        _isAiming = true;
        _cameraMain.depth = _priorityLow;
        _cameraFPS.depth = _priorityHigh;
    }

    public void StopAim()
    {
        Debug.Log("Stop Aiming");
        _isAiming = false;
        _cameraMain.depth = _priorityHigh;
        _cameraFPS.depth = _priorityLow;
    }

    public bool IsAiming()
    {
        return _isAiming;
    }
}
