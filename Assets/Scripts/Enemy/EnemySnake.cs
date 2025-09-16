using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class EnemySnake : EnemyMoveGround
{
    [SerializeField] private List<Transform> _bodySegments;
    /// <summary>
    /// �Z�O�����g�Ԃ̋���
    /// </summary>
    [SerializeField] private float _segmentDistance = 0.5f;
    /// <summary>
    /// sin�g�̐U��
    /// </summary>
    [SerializeField] private float _waveAmplitude = 0.3f;
    /// <summary>
    /// sin�t�̑���
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
    /// �T�C���g�ő̂�h�炷����
    /// </summary>
    private void UpdateSnakeBody()
    {
        if (_bodySegments == null || _bodySegments.Count == 0) return;
        _waveOffset += Time.deltaTime * _waveFrequency;
        for (int i = 0; i < _bodySegments.Count; i++)
        {
            Transform segment = _bodySegments[i];
            // �����猩���ʑ��̂���
            float phase = _waveOffset - i * _segmentDistance;
            // ���� right �x�N�g������ɍ��E�ɗh�炷
            Vector3 offset = transform.right * Mathf.Sin(phase) * _waveAmplitude;
            // �Z�O�����g�̖ڕW�ʒu�i��������ɋ��������炷�j
            Vector3 targetPos = transform.position
                                - transform.forward * (i + 1) * _segmentDistance
                                + offset;
            // �X���[�Y�ɒǏ]
            segment.position = Vector3.Lerp(segment.position, targetPos, 0.5f);
            // �O�̃Z�O�����g�̕�������������
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
