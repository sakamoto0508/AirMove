using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public State CurrentState = State.walking;
    public enum State
    {
        walking,
        sprinting,
        crouching,
        sliding,
    }
}
