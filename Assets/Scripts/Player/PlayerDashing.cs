using UnityEngine;

public class PlayerDashing : MonoBehaviour
{
    private float _dashForce;
    private float _dashUpForce;
    private float _dashDuration;
    private Rigidbody _rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Dash(PlayerData playerData)
    {
        Vector3 forceToApply = playerData.MainCamera.forward * _dashForce +
            playerData.MainCamera.up * _dashUpForce;
        _rb.AddForce(forceToApply, ForceMode.Impulse);
        Invoke(nameof(ResetDash), _dashDuration);
    }

    private void ResetDash()
    {

    }

    public void StartSetVariables(PlayerData playerData)
    {
        _dashForce = playerData.DashForce;
        _dashUpForce = playerData.DashUpForce;
        _dashDuration = playerData.DashDuration;
    }
}
