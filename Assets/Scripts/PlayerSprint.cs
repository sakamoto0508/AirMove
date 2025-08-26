using UnityEngine;
public class PlayerSprint : MonoBehaviour
{
    private bool _isSprinting;
    public void Sprint(PlayerState playerState, bool grounded)
    {
        if (playerState.CurrentState == PlayerState.State.crouching ||
           playerState.CurrentState == PlayerState.State.sliding)
            return;
        if (playerState.CurrentState == PlayerState.State.walking)
        {
            _isSprinting = true;
        }
        else if (playerState.CurrentState == PlayerState.State.sprinting)
        {
            _isSprinting= false;
        }
    }

    public bool IsSprinting()
    {
        return _isSprinting;
    }
}