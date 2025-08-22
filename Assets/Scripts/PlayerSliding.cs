using UnityEngine;

public class PlayerSliding : MonoBehaviour
{
    private Rigidbody _rb;
    private CapsuleCollider _capsuleCollider;
    private float _startYScale;
    private float _slideTimer = 0f;
    public bool _isSliding { get; private set; } = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _capsuleCollider = GetComponent<CapsuleCollider>();
        _startYScale = _capsuleCollider.height;
    }

    /// <summary>
    /// �X���C�f�B���O�̊J�n����
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="playerData"></param>
    /// <param name="isGround"></param>
    /// <param name="isSlope"></param>
    /// <param name="moveInput"></param>
    public void StartSliding(PlayerState playerState, PlayerData playerData, bool isGround, bool isSlope, Vector2 moveInput)
    {
        if (!CanStartSliding(playerState, isGround, isSlope, moveInput)) return;

        playerState.CurrentState = PlayerState.State.sliding;
        _isSliding = true;
        _slideTimer = 0f;
        _capsuleCollider.height = playerData.SlideYScale;
        Vector3 slideDirection = playerData.MainCamera.forward;
        if (moveInput.magnitude > 0.1f)
        {
            slideDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            slideDirection = transform.TransformDirection(slideDirection);
        }
        _rb.AddForce(slideDirection * playerData.SlidingForce, ForceMode.Impulse);
    }

    /// <summary>
    /// �X���C�f�B���O�̒�~����
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="playerData"></param>
    public void StopSliding(PlayerState playerState, PlayerData playerData)
    {
        EndSlide(playerState);
    }

    /// <summary>
    /// �X���C�f�B���O���J�n�ł��邩�ǂ����̔���
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="isGrounded"></param>
    /// <param name="isSlope"></param>
    /// <param name="moveInput"></param>
    /// <returns></returns>
    private bool CanStartSliding(PlayerState playerState, bool isGrounded, bool isSlope, Vector2 moveInput)
    {
        return (playerState.CurrentState == PlayerState.State.sprinting ||
               playerState.CurrentState == PlayerState.State.walking) &&
                (isGrounded || isSlope) &&
                 moveInput != Vector2.zero &&
                 !_isSliding;
    }

    /// <summary>
    /// �X���C�f�B���O���̍X�V����
    /// </summary>
    /// <param name="playerState"></param>
    /// <param name="playerData"></param>
    public void UpdateSliding(PlayerState playerState, PlayerData playerData, Vector2 moveInput)
    {
        if (!_isSliding) return;

        _slideTimer += Time.deltaTime;
        if (_slideTimer >= playerData.MaxSlidingTime)
        {
            EndSlide(playerState);
            return;
        }
        //�X���C�f�B���O���̌�������
        Vector3 velocity = _rb.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0, velocity.z);
        if (horizontalVelocity.magnitude > playerData.WalkSpeed / 2f)
        {
            Vector3 decelerationForce = -horizontalVelocity.normalized * playerData.SlidingDeceleration;
            _rb.AddForce(decelerationForce, ForceMode.Force);
        }
        else
        {
            // �X���C�f�B���O���̑��x�����s���x�ȉ��̏ꍇ�A�X���C�f�B���O���I��
            EndSlide(playerState);
            return;
        }

        if (moveInput.magnitude > 0.1f)
        {
            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;
            moveDirection = transform.TransformDirection(moveDirection);
            // �i�s����
            Vector3 velocityDir = horizontalVelocity.normalized;
            //���E����
            Vector3 sideDir = Vector3.Cross(Vector3.up, velocityDir);
            //���͂̍��E�����𒊏o
            float sideInput = Vector3.Dot(moveDirection, sideDir);
            _rb.AddForce(sideDir * sideInput * playerData.SlidingSideControl, ForceMode.Force);
        }
    }

    /// <summary>
    /// �X���C�f�B���O�̏I������
    /// </summary>
    /// <param name="playerState"></param>
    private void EndSlide(PlayerState playerState)
    {
        _isSliding = false;
        playerState.CurrentState = PlayerState.State.walking;
        _capsuleCollider.height = _startYScale;
        //TODO�ړ��L�[�����Z�b�g
    }
}
