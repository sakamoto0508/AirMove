using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;

public class PlayerAiming : MonoBehaviour
{
    public bool _isAiming { get; private set; } = false;
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private Camera _cameraFPS;
    [SerializeField] private int _priorityHigh = 1;
    [SerializeField] private int _priorityLow = 0;
    // �C�[�W���O�^�C�v
    [SerializeField] private Ease _easeType = Ease.InOutQuad;
    private float _transitionDurationSetUpTime;
    private float _transitionDurationSetEndTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraMain.depth = _priorityHigh;
        _cameraFPS.depth = _priorityLow;
        // DOTween�����ݒ�i�I�v�V�����j
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAim()
    {
        Debug.Log("Aiming");
        _isAiming = true;
        //_cameraMain.depth = _priorityLow;
        //_cameraFPS.depth = _priorityHigh;
        // �����̃J�����J�ڃA�j���[�V�������~
        // "cameraTransition"�Ƃ���ID�������ׂĂ�Tween���~
        DOTween.Kill("cameraTransition");
        // ���C���J�����̐[�x���X���[�Y�ɕύX
        DOTween.To(() => _cameraMain.depth,           // getter: ���݂̒l���擾
                  x => _cameraMain.depth = x,         // setter: �l��ݒ�
                  _priorityLow,                       // endValue: �ڕW�l
                  _transitionDurationSetUpTime)                // duration: �J�ڎ���
            .SetId("cameraTransition")                // ID�ݒ�ŊǗ����₷��
            .SetEase(_easeType);
        // FPS�J�����̐[�x���X���[�Y�ɕύX
        DOTween.To(() => _cameraFPS.depth,
                  x => _cameraFPS.depth = x,
                  _priorityHigh,
                  _transitionDurationSetUpTime)
            .SetId("cameraTransition")
            .SetEase(_easeType);
    }

    public void StopAim()
    {
        Debug.Log("Stop Aiming");
        _isAiming = false;
        //_cameraMain.depth = _priorityHigh;
        //_cameraFPS.depth = _priorityLow;
        // �����̃A�j���[�V�������~
        DOTween.Kill("cameraTransition");
        // �J���������̏�Ԃɖ߂�
        DOTween.To(() => _cameraMain.depth,
                  x => _cameraMain.depth = x,
                  _priorityHigh,
                  _transitionDurationSetEndTime)
            .SetId("cameraTransition")
            .SetEase(_easeType);
        DOTween.To(() => _cameraFPS.depth,
                  x => _cameraFPS.depth = x,
                  _priorityLow,
                  _transitionDurationSetEndTime)
            .SetId("cameraTransition")
            .SetEase(_easeType);
    }

    public bool IsAiming()
    {
        return _isAiming;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _transitionDurationSetUpTime = playerData.TransitionDurationSetUpTime;
        _transitionDurationSetEndTime = playerData.TransitionDurationSetEndTime;
    }
}
