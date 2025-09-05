using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public bool _isAiming { get; private set; } = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAim()
    {
        Debug.Log("Aiming");
        _isAiming = true;
    }

    public void StopAim()
    {
        Debug.Log("Stop Aiming");
        _isAiming = false;
    }

    public bool IsAiming()
    {
        return _isAiming;
    }
}
