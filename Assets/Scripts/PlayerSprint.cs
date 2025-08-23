using UnityEngine;
public class PlayerSprint : MonoBehaviour
{
    public void Sprint(PlayerState playerState, bool grounded)
    {
        if (playerState.CurrentState == PlayerState.State.crouching ||
           playerState.CurrentState == PlayerState.State.sliding)
            return;
        if (playerState.CurrentState == PlayerState.State.walking)
        {
            playerState.CurrentState = PlayerState.State.sprinting;
        }
        else if (playerState.CurrentState == PlayerState.State.sprinting)
        {
            playerState.CurrentState = PlayerState.State.walking;
        }
    }
}