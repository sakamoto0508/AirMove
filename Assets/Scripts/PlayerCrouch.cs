using UnityEngine;

public class PlayerCrouch : MonoBehaviour
{
    private CapsuleCollider _capsuleCollider;
    private float _startHeight;

    private void Start()
    {
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _startHeight=_capsuleCollider.height;
        
    }

    public void Crouch(PlayerState playerState, PlayerData playerData)
    {
        if (playerState.CurrentState == PlayerState.State.crouching)
        {
            playerState.CurrentState = PlayerState.State.walking;
            _capsuleCollider.height = _startHeight;
        }
        else
        {
            playerState.CurrentState = PlayerState.State.crouching;
            _capsuleCollider.height = playerData.CrouchHeight;
        }
    }
}
