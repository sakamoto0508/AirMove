using UnityEngine;

public class PlayerWallClimbing : MonoBehaviour
{
    private float _climbSpeed;
    public bool _isClimbing { get; private set; } = false;
    private bool _wallFront;
    private bool _canWallClimb;

    private Vector2 _currentInput;
    private Rigidbody _rb;
    private RaycastHit _wallFrontHit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_canWallClimb)
        {
            StartClimbing();
        }
        else
        {
            if (_isClimbing)
            {
                StopClimbing();
            }
        }
    }

    private void FixedUpdate()
    {
        if (_isClimbing)
        {
            ClimbingMovement();
        }
    }

    private void StartClimbing()
    {
        _isClimbing = true;
    }

    private void ClimbingMovement()
    {
        _rb.linearVelocity=new Vector3(_rb.linearVelocity.x, _climbSpeed, _rb.linearVelocity.z);
    }

    private void StopClimbing()
    {
        _isClimbing = false;
    }

    public void WallClimbingMove(Vector2 input, RaycastHit wallFrontHit)
    {
        _currentInput = input;
        _wallFrontHit = wallFrontHit;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _climbSpeed = playerData.ClimbingSpeed;
    }

    public void SetWallCheck(bool wallFront)
    {
        _wallFront = wallFront;
    }

    public void SetCanWallClimb(bool canWallClimb)
    {
        _canWallClimb = canWallClimb;
    }

    public bool IsWallClimbing()
    {
        return _isClimbing;
    }
}
