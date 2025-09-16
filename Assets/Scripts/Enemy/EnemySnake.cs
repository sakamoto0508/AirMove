using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemySnake : EnemyMoveGround
{
    [SerializeField] private List<Transform> _bodySegments;
    /// <summary>
    /// セグメント間の距離
    /// </summary>
    [SerializeField] private float _segmentDistance = 0.5f;
    /// <summary>
    /// sin波の振幅
    /// </summary>
    [SerializeField] private float _waveAmplitude = 0.3f;
    /// <summary>
    /// sin葉の速さ
    /// </summary>
    [SerializeField] private float _waveFrequency = 5f;
    private Collider[] _childrenColliders;
    private float _waveOffset;
    private float _currentWaveFrequency;

    protected override void Start()
    {
        base.Start();
        _currentWaveFrequency = _waveFrequency;
        _childrenColliders = GetComponentsInChildren<Collider>();
    }
    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        UpdateSnakeBody();
    }

    /// <summary>
    /// サイン波で体を揺らす処理
    /// </summary>
    private void UpdateSnakeBody()
    {
        if (_bodySegments == null || _bodySegments.Count == 0) return;
        _waveOffset += Time.deltaTime * _waveFrequency;
        for (int i = 0; i < _bodySegments.Count; i++)
        {
            Transform segment = _bodySegments[i];
            // 頭から見た位相のずれ
            float phase = _waveOffset - i * _segmentDistance;
            // 頭の right ベクトルを基準に左右に揺らす
            Vector3 offset = transform.right * Mathf.Sin(phase) * _waveAmplitude;
            // セグメントの目標位置（頭から後ろに距離をずらす）
            Vector3 targetPos = transform.position
                                - transform.forward * (i + 1) * _segmentDistance
                                + offset;
            // スムーズに追従
            segment.position = Vector3.Lerp(segment.position, targetPos, 0.5f);
            // 前のセグメントの方向を向かせる
            segment.LookAt(i == 0 ? transform.position : _bodySegments[i - 1].position);
        }
    }

    public override void TimeStopAction()
    {
        base.TimeStopAction();
        _waveFrequency = 0f;
    }

    public override void TimeStartAction()
    {
        base.TimeStartAction();
        _waveFrequency = _currentWaveFrequency;
    }
}
