using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    private CapsuleCollider _capsuleCollider;
    private float _startHeight;
    private bool _isCrouching;
    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _startHeight=_capsuleCollider.height;
        
    }

    public void Crouch(PlayerState playerState, PlayerData playerData)
    {
        if (playerState.CurrentState == PlayerState.State.Crouching)
        {
            _isCrouching = false;
            //playerState.CurrentState = PlayerState.State.walking;
            _capsuleCollider.height = _startHeight;
        }
        else
        {
            _isCrouching = true;
            //playerState.CurrentState = PlayerState.State.crouching;
            _capsuleCollider.height = playerData.CrouchHeight;
        }
    }
    public bool IsCrouching()
    {
        return _isCrouching;
    }
}
