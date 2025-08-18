using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State
    {
        walking,
        sprinting,
        isAir,
        crouching,
        sliding,
    }
}
