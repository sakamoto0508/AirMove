using UnityEngine;

public class PlayerSprint : MonoBehaviour
{
    public void Sprint(PlayerState playerState, bool grounded)
    {
        if (playerState.CurrentState == PlayerState.State.crouching)
            return;
        if (playerState.CurrentState == PlayerState.State.walking)
        {
            playerState.CurrentState = PlayerState.State.sprinting;
        }
        else
        {
            playerState.CurrentState = PlayerState.State.walking;
        }
    }
}
