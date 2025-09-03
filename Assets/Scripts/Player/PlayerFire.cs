using System.Collections;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{
    private float _fireRate;
    private float _fireRange;
    private float _reloadTime;
    private int _magazineSize;
    private bool _fireTimerIsActive = false;
    private Transform _firePosition;
    private WaitForSeconds _fireRateWait;
    private RaycastHit _hit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _fireRateWait= new WaitForSeconds(_fireRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire(PlayerData playerData)
    {
        if (_fireTimerIsActive)
        {
            return;
        }
        Debug.Log("Fire");
        if(Physics.Raycast(playerData.FirePosition.position, playerData.FirePosition.forward, out _hit, _fireRange))
        {
            BulletHit();
        }
        StartCoroutine(FireTimer());
    }

    private void BulletHit()
    {
        Debug.Log("Hit: " + _hit.collider.name);
    }

    private IEnumerator FireTimer()
    {
        _fireTimerIsActive = true;
        yield return _fireRateWait;
        _fireTimerIsActive = false;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _fireRate = playerData.FireRate;
        _reloadTime = playerData.ReloadTime;
        _magazineSize = playerData.MagazineSize;
        _firePosition = playerData.FirePosition;
        _fireRange = playerData.FireRange;
    }
}
