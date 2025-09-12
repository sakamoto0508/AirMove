using System.Runtime.CompilerServices;
using Unity.Cinemachine;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;

public class PlayerAiming : MonoBehaviour
{
    public bool _isAiming { get; private set; } = false;
    [SerializeField] private Camera _cameraMain;
    [SerializeField] private Camera _cameraFPS;
    [SerializeField] private int _priorityHigh = 1;
    [SerializeField] private int _priorityLow = 0;
    // イージングタイプ
    [SerializeField] private Ease _easeTypeCamera = Ease.InOutQuad;
    [SerializeField] private Ease _easeTypeScope = Ease.InQuart;
    [SerializeField] Image _image;
    private float _transitionDurationSetUpTime;
    private float _transitionDurationSetEndTime;
    private float _feedInTime;
    private float _feedOutTime;
    private bool _isReloading;
    private Coroutine _coroutine;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraMain.depth = _priorityHigh;
        _cameraFPS.depth = _priorityLow;
        //透明化
        if (_image != null)
        {
            //_image.color = new Color(_image.color.r, _image.color.g, _image.color.b, 0f);
            _image.gameObject.SetActive(false);
        }
        // DOTween初期設定（オプション）
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartAim()
    {
        if(_isReloading) { return; };
        _isAiming = true;
        // Sequenceを使って同期的にアニメーション実行
        Sequence aimSequence = DOTween.Sequence();
        aimSequence.SetId("aimTransition");
        // カメラ遷移
        aimSequence.Join(
            DOTween.To(() => _cameraMain.depth, x => _cameraMain.depth = x, _priorityLow, _transitionDurationSetUpTime)
                .SetEase(_easeTypeCamera)
        );
        aimSequence.Join(
            DOTween.To(() => _cameraFPS.depth, x => _cameraFPS.depth = x, _priorityHigh, _transitionDurationSetUpTime)
                .SetEase(_easeTypeCamera)
        );
        //スコープ画像の表示
        _coroutine = StartCoroutine(ShowScope());
    }

    public void StopAim()
    {
        _isAiming = false;
        // 既存のスコープコルーチンを停止
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
        }
        // 既存のアニメーションを停止
        DOTween.Kill("aimTransition");
        Sequence stopSequence = DOTween.Sequence();
        stopSequence.SetId("aimTransition");
        // カメラを元に戻す
        //getter,setter,目標値,時間
        stopSequence.Join(
            DOTween.To(() => _cameraMain.depth, x => _cameraMain.depth = x, _priorityHigh, _transitionDurationSetEndTime)
                .SetEase(_easeTypeCamera)
        );
        stopSequence.Join(
            DOTween.To(() => _cameraFPS.depth, x => _cameraFPS.depth = x, _priorityLow, _transitionDurationSetEndTime)
                .SetEase(_easeTypeCamera)
        );
        // スコープ画像の非表示
        _coroutine = StartCoroutine(CloseScope());
    }

    public bool IsAiming()
    {
        return _isAiming;
    }

    public void StartSetVariables(PlayerData playerData)
    {
        _transitionDurationSetUpTime = playerData.TransitionDurationSetUpTime;
        _transitionDurationSetEndTime = playerData.TransitionDurationSetEndTime;
        _feedInTime = playerData.FeedInTime;
        _feedOutTime = playerData.FeedOutTime;
    }

    private IEnumerator ShowScope()
    {
        yield return new WaitForSeconds(_feedInTime);
        _image.gameObject.SetActive(true);
    }

    private IEnumerator CloseScope()
    {
        yield return new WaitForSeconds(_feedOutTime);
        _image.gameObject.SetActive(false);
    }

    public void SetIsReloading(bool isReloading)
    {
        _isReloading = isReloading;
    }
}
