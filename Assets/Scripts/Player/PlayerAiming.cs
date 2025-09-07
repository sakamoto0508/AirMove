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
    // イージングタイプ
    [SerializeField] private Ease _easeType = Ease.InOutQuad;
    private float _transitionDurationSetUpTime;
    private float _transitionDurationSetEndTime;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraMain.depth = _priorityHigh;
        _cameraFPS.depth = _priorityLow;
        // DOTween初期設定（オプション）
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
        // 既存のカメラ遷移アニメーションを停止
        // "cameraTransition"というIDを持つすべてのTweenを停止
        DOTween.Kill("cameraTransition");
        // メインカメラの深度をスムーズに変更
        DOTween.To(() => _cameraMain.depth,           // getter: 現在の値を取得
                  x => _cameraMain.depth = x,         // setter: 値を設定
                  _priorityLow,                       // endValue: 目標値
                  _transitionDurationSetUpTime)                // duration: 遷移時間
            .SetId("cameraTransition")                // ID設定で管理しやすく
            .SetEase(_easeType);
        // FPSカメラの深度をスムーズに変更
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
        // 既存のアニメーションを停止
        DOTween.Kill("cameraTransition");
        // カメラを元の状態に戻す
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
