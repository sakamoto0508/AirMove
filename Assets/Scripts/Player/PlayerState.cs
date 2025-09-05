using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public State CurrentState = State.Idle;
    [SerializeField] private Animator _animator;
    private const string CURRENT_STATE = "CurrentState";
    public enum State
    {
        Walking,
        Sprinting,
        Dashing,
        Wallrunning,
        Wallclimbing,
        Crouching,
        Sliding,
        Air,
        Idle
    }

    public void StateMachine(bool isDashing, bool isWallClimbing, bool isWallRunning, bool isSliding, bool isCrouching, bool isGround, bool isSlope, bool isSprinting, bool isIdle)
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
        else if (isIdle)
        {
            CurrentState = State.Idle;
        }
        else if ((isGround || isSlope) && isSprinting)
        {
            CurrentState = State.Sprinting;
        }
        else if ((isGround || isSlope) && !isSprinting)
        {
            CurrentState = State.Walking;
        }
        else
        {
            CurrentState = State.Air;
        }
    }

    public void AnimationChange(State state)
    {
        _animator.SetInteger("State",(int)state);
    }
}
