using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public State CurrentState = State.Walking;
    public enum State
    {
        Walking,
        Sprinting,
        Dashing,
        Wallrunning,
        Wallclimbing,
        Crouching,
        Sliding,
        Air
    }

    public void StateMachine(bool isDashing,bool isWallClimbing, bool isWallRunning,bool isSliding,bool isCrouching,bool isGround,bool isSlope, bool isSprinting)
    {
        if (isDashing)
        {
            CurrentState = State.Dashing;
            return;
        }
        else if (isWallClimbing)
        {
            CurrentState = State.Wallclimbing;
            return;
        }
        else if (isWallRunning)
        {
            CurrentState = State.Wallrunning;
            return;
        }
        else if (isSliding)
        {
            CurrentState = State.Sliding;
        }
        else if (isCrouching)
        {
            CurrentState = State.Crouching;
        }
        else if ((isGround||isSlope) && isSprinting)
        {
            CurrentState = State.Sprinting;
        }
        else if ((isGround||isSlope) && !isSprinting)
        {
            CurrentState = State.Walking;
        }
        else
        {
            CurrentState = State.Air;
        }
    }
}
