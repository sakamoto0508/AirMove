using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public State CurrentState = State.walking;
    public enum State
    {
        walking,
        sprinting,
        dashing,
        wallrunning,
        wallclimbing,
        crouching,
        sliding,
        air
    }

    public void StateMachine(bool isDashing,bool isWallClimbing, bool isWallRunning,bool isSliding,bool isCrouching,bool isGround,bool isSlope, bool isSprinting)
    {
        if (isDashing)
        {
            CurrentState = State.dashing;
            return;
        }
        else if (isWallClimbing)
        {
            CurrentState = State.wallclimbing;
            return;
        }
        else if (isWallRunning)
        {
            CurrentState = State.wallrunning;
            return;
        }
        else if (isSliding)
        {
            CurrentState = State.sliding;
        }
        else if (isCrouching)
        {
            CurrentState = State.crouching;
        }
        else if ((isGround||isSlope) && isSprinting)
        {
            CurrentState = State.sprinting;
        }
        else if ((isGround||isSlope) && !isSprinting)
        {
            CurrentState = State.walking;
        }
        else
        {
            CurrentState = State.air;
        }
    }
}
