using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SoundData
{
    public string name;      // �Ăяo���p�̖��O
    public AudioClip clip;   // ���ۂ� AudioClip
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("BGM �p AudioSource")]
    [SerializeField] private AudioSource bgmSource;
    [Header("SE �p AudioSource �v���n�u")]
    [SerializeField] private AudioSource sfxSourcePrefab;
    [Header("BGM���X�g")]
    [SerializeField] private List<SoundData> bgmList = new List<SoundData>();
    [Header("���ʉ����X�g")]
    [SerializeField] private List<SoundData> seList = new List<SoundData>();
    [Header("���ʉ��v�[���ݒ�")]
    [SerializeField] private int sfxPoolSize = 10;

    private Dictionary<string, AudioClip> _bgmDict = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _seDict = new Dictionary<string, AudioClip>();
    private List<AudioSource> _sfxPool = new List<AudioSource>();

    private void Awake()
    {
        // �V���O���g���m��
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioManager();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void InitializeAudioManager()
    {
        // BGM�p AudioSource �̊m��
        EnsureBGMSource();

        // BGM�������ɓo�^
        _bgmDict.Clear();
        foreach (var bgm in bgmList)
        {
            if (!_bgmDict.ContainsKey(bgm.name) && bgm.clip != null)
            {
                _bgmDict.Add(bgm.name, bgm.clip);
            }
        }

        // SE �������ɓo�^
        _seDict.Clear();
        foreach (var se in seList)
        {
            if (!_seDict.ContainsKey(se.name) && se.clip != null)
            {
                _seDict.Add(se.name, se.clip);
            }
        }

        // �v�[����������
        CreateSFXPool();
    }

    private void EnsureBGMSource()
    {
        // ������bgmSource���L�����`�F�b�N
        if (bgmSource == null || bgmSource.Equals(null))
        {
            // BGM�p��AudioSource���q�I�u�W�F�N�g�Ƃ��č쐬
            GameObject bgmObj = new GameObject("BGM_Source");
            bgmObj.transform.SetParent(transform);
            bgmSource = bgmObj.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }
    }

    private void CreateSFXPool()
    {
        // �����̃v�[�����N���A
        foreach (var source in _sfxPool)
        {
            if (source != null)
            {
                DestroyImmediate(source.gameObject);
            }
        }
        _sfxPool.Clear();

        // �V�����v�[�����쐬
        for (int i = 0; i < sfxPoolSize; i++)
        {
            if (sfxSourcePrefab != null)
            {
                var sfxSource = Instantiate(sfxSourcePrefab, transform);
                sfxSource.playOnAwake = false;
                sfxSource.loop = false; // ���[�v�֎~�i���ʉ��p�j
                sfxSource.clip = null;  // clip ����ɂ��Ă���
                _sfxPool.Add(sfxSource);
            }
        }
    }


    public void PlayBGM(AudioClip clip)
    {
        if (clip == null) return;

        // BGM��p��AudioSource���m���ɗp��
        EnsureBGMSource();

        if (bgmSource == null)
        {
            Debug.LogError("bgmSource �̍쐬�Ɏ��s���܂����B");
            return;
        }

        // SE�pAudioSource�ł͍Đ����Ȃ��ibgmSource��p�j
        foreach (var sfx in _sfxPool)
        {
            if (sfx != null && sfx.isPlaying && sfx.clip == clip)
            {
                sfx.Stop();
                sfx.clip = null;
            }
        }

        // ����BGM���Đ����Ȃ牽�����Ȃ�
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;

        bgmSource.Stop();
        bgmSource.clip = clip;
        bgmSource.loop = true; // �K�����[�v
        bgmSource.Play();
    }


    /// <summary>
    /// BGM �Đ��i���O�w��j
    /// </summary>
    public void PlayBGM(string bgmName)
    {
        if (_bgmDict.TryGetValue(bgmName, out var clip))
        {
            PlayBGM(clip);
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽ BGM '{bgmName}' �͓o�^����Ă��܂���B");
        }
    }

    /// <summary>
    /// BGM ��~
    /// </summary>
    public void StopBGM()
    {
        if (bgmSource != null && !bgmSource.Equals(null))
        {
            bgmSource.Stop();
            bgmSource.clip = null;
        }
    }

    /// <summary>
    /// BGM ���ʒ���
    /// </summary>
    public void SetBGMVolume(float volume)
    {
        EnsureBGMSource();
        if (bgmSource != null)
        {
            bgmSource.volume = Mathf.Clamp01(volume);
        }
    }

    /// <summary>
    /// ���ʉ��Đ��i���O�w��j
    /// </summary>
    public void PlaySE(string seName, float volume = 1f)
    {
        if (_seDict.TryGetValue(seName, out var clip))
        {
            var src = GetAvailableSfxSource();
            if (src != null)
            {
                src.clip = clip;
                src.volume = Mathf.Clamp01(volume);
                src.Play();
            }
        }
        else
        {
            Debug.LogWarning($"�w�肳�ꂽ SE '{seName}' �͓o�^����Ă��܂���B");
        }
    }

    /// <summary>
    /// �󂢂Ă��� SE �p AudioSource ��T��
    /// </summary>
    private AudioSource GetAvailableSfxSource()
    {
        foreach (var s in _sfxPool)
        {
            if (s != null && !s.isPlaying) return s;
        }

        // �S�����܂��Ă���V�������
        if (sfxSourcePrefab != null)
        {
            var extra = Instantiate(sfxSourcePrefab, transform);
            extra.playOnAwake = false;
            _sfxPool.Add(extra);
            return extra;
        }

        return null;
    }

    /// <summary>
    /// SE ���ʒ���
    /// </summary>
    public void SetSEVolume(float volume)
    {
        foreach (var s in _sfxPool)
        {
            if (s != null)
            {
                s.volume = Mathf.Clamp01(volume);
            }
        }
    }
}
