using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public State CurrentState = State.walking;
    public enum State
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        sliding,
        air
    }

    public void StateMachine(bool isWallRunning,bool isSliding,bool isCrouching,bool isGround,bool isSlope, bool isSprinting)
    {
        if(isWallRunning)
        {
            CurrentState = State.wallrunning;
            return;
        }
        if (isSliding)
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
