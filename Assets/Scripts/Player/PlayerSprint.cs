using UnityEngine;
public class PlayerSprint : MonoBehaviour
{
    private bool _isSprinting;
    public void Sprint(PlayerState playerState, bool grounded)
    {
        if (playerState.CurrentState == PlayerState.State.Crouching ||
           playerState.CurrentState == PlayerState.State.Sliding)
            return;
        if (playerState.CurrentState == PlayerState.State.Walking)
        {
            _isSprinting = true;
        }
        else if (playerState.CurrentState == PlayerState.State.Sprinting)
        {
            _isSprinting= false;
        }
    }

    public bool IsSprinting()
    {
        return _isSprinting;
    }
}